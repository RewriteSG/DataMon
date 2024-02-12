using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class DataMonAI : MonoBehaviour
{
    public AI_State AI_state;
    public Transform Target;
    DataMon.IndividualDataMon.DataMon _dataMon;
    [SerializeField] private float NextWaypointDist = 3;
    public float PatrollingDistance= 3, WanderingDistance = 8;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool doingSomething = false;

    Seeker seeker;
    Rigidbody2D rb;

    [HideInInspector]public GameObject patrollingAnchor;

    [HideInInspector] public AggroSystem aggroSystem;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        _dataMon = GetComponent<DataMon.IndividualDataMon.DataMon>();
        _dataMon.dataMonAI = this;
        //InvokeRepeating("TestAI", 0, .5f);
    }
    private void OnEnable()
    {
        NeutralStartPath = true;
        StartCoroutine(StartPathing());

    }
    // Update is called once per frame
    void Update()
    {
        if (reachedEndOfPath && !Target.isNull())
        {
            DataMonDoThings();
        }
        if(_dataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion && 
            Vector3.Distance(transform.position, GameManager.instance.Player.transform.position)>GameManager.instance.MaxDistForCompanionDataMon)
        {
            AI_state = AI_State.Patrol;
        }
    }
    IEnumerator StartPathing()
    {
        yield return new WaitForEndOfFrame();
        if (patrollingAnchor == null && _dataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion)
        {
            GameObject newPatrolAnchor = new GameObject(_dataMon.dataMon.DataMonName+ "'s Companion Anchor");
            newPatrolAnchor.transform.position = (Random.insideUnitCircle.normalized*Random.Range(GameManager.instance.PlayerDataMonPatrolMinDist, 
                GameManager.instance.PlayerDataMonPatrolMaxDist)) + (Vector2)GameManager.instance.Player.transform.position;
            //newPatrolAnchor.transform.position += GameManager.instance.Player.transform.position;
            newPatrolAnchor.transform.SetParent(GameManager.instance.Player.transform, true);
            patrollingAnchor = newPatrolAnchor;
        }
        if (patrollingAnchor == null && _dataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isNeutral)
        {
            GameObject newPatrolAnchor = new GameObject(_dataMon.dataMon.DataMonName + "'s Patrolling Anchor");
            newPatrolAnchor.transform.position = transform.position;
            patrollingAnchor = newPatrolAnchor;

        }
        while (this.isActiveAndEnabled)
        {
            switch (AI_state)
            {
                case AI_State.Attack:
                case AI_State.Produce:
                    StartAttack();
                    break;
                case AI_State.Patrol:
                    StartPatrol();
                    break;
                case AI_State.Support:
                    break;
            }
            yield return new WaitForSeconds(0.5f);

        }
    }

    #region DifferentAI
    //void StartProduction()
    //{
    //    if (Vector3.Distance(transform.position, Target.position) > NextWaypointDist)
    //    {
    //        reachedEndOfPath = false;
    //    }
    //    else
    //    {
    //        reachedEndOfPath = true;
    //        return;
    //    }

    //    if (seeker.IsDone())
    //        seeker.StartPath(transform.position, Target.position, OnPathingComplete);

    //}
    Collider2D[] allCollidersInCircle = new Collider2D[] { };
    void StartAttack()
    {
        //Vector3.Distance(transform.position, Target.position) > _dataMon.dataMon.AttackRange
        Dir = (transform.position - Target.position).normalized;
        allCollidersInCircle = Physics2D.OverlapCircleAll(transform.position, _dataMon.dataMonCurrentAttributes.CurrentAttackRange);
        if (!allCollidersInCircle.ColliderArrayHasTag(Target.tag))
        {
            reachedEndOfPath = false;
        }
        else
        {
            reachedEndOfPath = true;

            return;
        }
        if (seeker.IsDone())
            seeker.StartPath(transform.position, Target.position, OnPathingComplete);
        print("working");
    }
    Vector3 randomPatrolDir;
    Vector3 goingToPos;
    void StartPatrol()
    {
        //if (Vector3.Distance(transform.position, goingToPos) > NextWaypointDist)
        //{
        //    reachedEndOfPath = false;
        //}
        //else
        //{
        //    reachedEndOfPath = true;
        //    return;
        //}
        if(_dataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isNeutral)
        {
            NeutralPatrol();
            return;
        }
        if (seeker.IsDone() && Vector3.Distance(transform.position, patrollingAnchor.transform.position) > PatrollingDistance)
        {
            goingToPos = patrollingAnchor.transform.position;
            seeker.StartPath(transform.position, goingToPos, OnPathingComplete);
        }
        else if (seeker.IsDone() && reachedEndOfPath)
        {
            goingToPos = (Random.insideUnitCircle.normalized * (PatrollingDistance + 1)) + (Vector2)transform.position;
            seeker.StartPath(transform.position, goingToPos, OnPathingComplete);
        }
        
    }
    bool NeutralStartPath = true;
    void NeutralPatrol()
    {
        if(seeker.IsDone()&& Vector3.Distance(transform.position, patrollingAnchor.transform.position) > WanderingDistance)
        {
            goingToPos = patrollingAnchor.transform.position;
            seeker.StartPath(transform.position, goingToPos, OnPathingComplete);
        }
        if (seeker.IsDone() && (reachedEndOfPath || NeutralStartPath))
        {
            NeutralStartPath = false;
            goingToPos = (Random.insideUnitCircle.normalized * (PatrollingDistance + 1)) + (Vector2)transform.position;
            goingToPos = new Vector3(Mathf.Clamp(goingToPos.x, 4, 396), Mathf.Clamp(goingToPos.y, 4, 396));
            seeker.StartPath(transform.position, goingToPos, OnPathingComplete);
        }
    }
    #endregion
    private void FixedUpdate()
    {
        MoveAI();
    }

    Vector2 Dir;
    Vector2 Force;
    float distance;
    Quaternion toRotate;
    private void MoveAI()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else if (!doingSomething && AI_state != AI_State.Attack && AI_state != AI_State.Produce)
        {
            reachedEndOfPath = false;
        }

        if(reachedEndOfPath && AI_state == AI_State.Produce || AI_state == AI_State.Attack)
        {
            Dir = (transform.position - Target.position).normalized;
            toRotate = Quaternion.LookRotation(transform.forward, -Dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, (GameManager.instance.DataMonsRotationSpeed*3) * Time.fixedDeltaTime);
        }
        
        if (reachedEndOfPath)
        {
            return;
        }

        Dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Force = Dir * _dataMon.dataMon.BaseAttributes.BaseMoveSpeed * Time.fixedDeltaTime;

        rb.AddForce(Force);
        distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        toRotate= Quaternion.LookRotation(transform.forward, Dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, GameManager.instance.DataMonsRotationSpeed * Time.fixedDeltaTime);

        if (distance < NextWaypointDist)
        {
            currentWaypoint++;
        }
    }

    void DataMonDoThings()
    {
        if (doingSomething)
            return;
        doingSomething = true;
        switch (AI_state)
        {
            case AI_State.Attack:
                doingSomething = false;
                // Instantiate Attack
                break;
            case AI_State.Support:
                doingSomething = false;

                // Instantiate Defense
                break;
            case AI_State.Patrol:
                doingSomething = false;

                // Instantiate Attack
                break;
            case AI_State.Produce:
                doingSomething = false;

                // Instantiate Attack
                break;

        }
    }
   
    public void CommandDataMon(Transform _target, AI_State state)
    {
        AI_state = state;
        Target = _target;
    }
    void OnPathingComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, NextWaypointDist);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, PatrollingDistance);

    }
}
public enum AI_State
{
    Patrol, Attack, Support, Produce
}
