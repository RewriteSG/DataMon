using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Commands;
public class DataMonAI : MonoBehaviour
{
    public AI_State AI_state;
    public Transform Target;
    IndividualDataMon.DataMon DataMon;
    [SerializeField] private float NextWaypointDist = 3;
    public float PatrollingDistance= 3, WanderingDistance = 8;
    public float ProductionDistance = 1;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool doingSomething = false;

    Seeker seeker;
    Rigidbody2D rb;
    [HideInInspector]public GameObject patrollingAnchor;

    [HideInInspector] public AggroSystem aggroSystem;
    public AI_Tasks CurrentTask;

    public float DataMonProductionTime, CurrentProductionProgress, GlitchDamage;
    int DataBytesUsed;
    GameObject ItemCrafted;
    GlitchObject glitch;

    // Start is called before the first frame update
    void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        DataMon = GetComponent<IndividualDataMon.DataMon>();
        DataMon.dataMonAI = this;
        gameObject.AddComponent<AggroSystem>();
        timerToChangeTarget = 999;
        //InvokeRepeating("TestAI", 0, .5f);
        
    }
    private void OnEnable()
    {
        NeutralStartPath = true;
        //StartCoroutine(StartPathing());
    }
    private void Start()
    {
        GameManager.instance.DataMon_StartAI(this);
        GameManager.instance.Entity_Updates += ToUpdate;
        GameManager.instance.Entity_FixedUpdates += ToFixedUpdate;
    }
    float timerToChangeTarget;
    // Update is called once per frame
    void ToUpdate()
    {
        if (reachedEndOfPath && !Target.isNull())
        {
            DataMonDoThings();
        }
        if(AI_state == AI_State.Produce && Target.isNull())
        {
            CancelProduction();
        }
        if (DataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion &&
            Vector3.Distance(transform.position, GameManager.instance.Player.transform.position) > GameManager.instance.MaxDistForCompanionDataMon && 
            AI_state != AI_State.Patrol
            && GameManager.HostileDataMons <= 0 )
        {
            timerToChangeTarget = 999;
            AI_state = AI_State.Patrol;
            
        }
        if (DataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion &&
            Vector3.Distance(transform.position, GameManager.instance.Player.transform.position) > GameManager.instance.MaxDistForCompanionDataMon &&
            AI_state == AI_State.Patrol
            && GameManager.HostileDataMons <= 0)
        {
            CancelProduction();

        }
        if (DataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion && GameManager.HostileDataMons > 0 && AI_state != AI_State.Produce)
        {
            switch (DataMonCommand.command)
            {
                case DataMonCommands.DontAttack:
                    break;

                case DataMonCommands.Patrol:
                case DataMonCommands.TargetEnemy:
                case DataMonCommands.AttackAggressive:
                        AI_state = AI_State.Attack;
                    if (DataMonCommand.ToTarget != null)
                    {
                        timerToChangeTarget = Mathf.Clamp(timerToChangeTarget,0,29);
                        ChangeAttackTargetEnemy(DataMonCommand.ToTarget);
                    }
                    if (timerToChangeTarget > 30)
                    {
                        timerToChangeTarget = 0;
                        ChangeAttackTargetEnemy(GameManager.instance.HostileDataMonsGOs[Random.Range(0, GameManager.instance.HostileDataMonsGOs.Count)]);

                    }
                    else
                    {
                        timerToChangeTarget += Time.deltaTime;
                    }
                    break;
            }
        }
    }
    public void UpdateDatamonAI()
    {
        if (patrollingAnchor == null)
            CreateNewPatrolAnchor();
        switch (AI_state)
        {
            case AI_State.Produce:
                StartAttack();
                break;
            case AI_State.Attack:
                CurrentTask = AI_Tasks.Attacking;
                StartAttack();
                break;
            case AI_State.Patrol:
                if (DataMonCommand.command == DataMonCommands.AttackAggressive)
                    CurrentTask = AI_Tasks.Attacking;
                else
                    CurrentTask = AI_Tasks.Patrolling;
                StartPatrol();
                break;
            case AI_State.Support:
                break;
        }

    }
    public void ChangeAggroTarget()
    {
        Target = aggroSystem.ListOfTargets.FindHighestDamageDealer().transform;
        if(DataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isNeutral)
        {
            DataMon.SetDataMonHostile();
            AI_state = AI_State.Attack;
            GameManager.instance.HostileDataMonsGOs.Add(gameObject);
        }
    }
    public void ChangeAttackTargetEnemy(GameObject enemy)
    {
        Target = enemy.transform;

    }
    public void CreateNewPatrolAnchor()
    {
        if (patrollingAnchor == null && DataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion)
        {
            GameObject newPatrolAnchor = new GameObject(DataMon.dataMon.DataMonName + "'s Companion Anchor");
            newPatrolAnchor.transform.position = (Random.insideUnitCircle.normalized * Random.Range(GameManager.instance.PlayerDataMonPatrolMinDist,
                GameManager.instance.PlayerDataMonPatrolMaxDist)) + (Vector2)GameManager.instance.Player.transform.position;
            //newPatrolAnchor.transform.position += GameManager.instance.Player.transform.position;
            newPatrolAnchor.transform.SetParent(GameManager.instance.Player.transform, true);

            patrollingAnchor = newPatrolAnchor;
        }
        if (patrollingAnchor == null && DataMon.dataMon.MonBehaviourState != DataMonBehaviourState.isCompanion)
        {
            GameObject newPatrolAnchor = new GameObject(DataMon.dataMon.DataMonName + "'s Patrolling Anchor");
            newPatrolAnchor.transform.position = transform.position;
            newPatrolAnchor.transform.parent = RoamingSpawner.DataMonPoolGO.transform;
            patrollingAnchor = newPatrolAnchor;

        }
    }

    public void SetNewPatrollingAnchorPos()
    {
        if (patrollingAnchor == null)
            CreateNewPatrolAnchor();

        if (!patrollingAnchor.activeSelf && DataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion)
        {
            patrollingAnchor.SetActive(true);
            patrollingAnchor.transform.position = (Random.insideUnitCircle.normalized * Random.Range(GameManager.instance.PlayerDataMonPatrolMinDist,
                GameManager.instance.PlayerDataMonPatrolMaxDist)) + (Vector2)GameManager.instance.Player.transform.position;
            //patrollingAnchor.transform.position += GameManager.instance.Player.transform.position;
            patrollingAnchor.transform.SetParent(GameManager.instance.Player.transform, true);
        }
        if (!patrollingAnchor.activeSelf && DataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isNeutral)
        {
            patrollingAnchor.SetActive(true);
            patrollingAnchor.transform.parent = RoamingSpawner.DataMonPoolGO.transform;
            patrollingAnchor.transform.position = transform.position;
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
    Vector2 targetPos;
    public void StartAttack()
    {

        //Vector3.Distance(transform.position, Target.position) > DataMon.dataMon.AttackRange
        if (Target == null)
            return;
        if (AI_state != AI_State.Produce)
            allCollidersInCircle = Physics2D.OverlapCircleAll(transform.position, DataMon.CurrentAttributes.CurrentAttackRange);
        else
            allCollidersInCircle = Physics2D.OverlapCircleAll(transform.position, ProductionDistance);

        DataMon.GetAvailableAttack();

        if (DataMon.dataMon.MonBehaviourState != DataMonBehaviourState.isCompanion && allCollidersInCircle.ColliderArrayHasGameObject(Target.gameObject))
        {
            reachedEndOfPath = true;


            return;
        }else
            reachedEndOfPath = false;
        
        if (DataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion && allCollidersInCircle.ColliderArrayHasGameObject(Target.gameObject,true)&&
            AI_state != AI_State.Produce)
        {
            reachedEndOfPath = true;

            return;
        }
        else
            reachedEndOfPath = false;

        if (DataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isCompanion && allCollidersInCircle.ColliderArrayHasGameObject(Target.gameObject) &&
            AI_state == AI_State.Produce)
        {
            reachedEndOfPath = true;

            return;
        }
        else
            reachedEndOfPath = false;

        if (seeker.IsDone() && Vector2.Distance(Target.position, targetPos)> DataMon.baseAttributes.BaseAttackRange)
        {
            targetPos = Target.position;
            seeker.StartPath(transform.position, targetPos, OnPathingComplete);
        }

    }
    Vector3 randomPatrolDir;
    Vector3 goingToPos;
    public void StartPatrol()
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
        if(DataMon.dataMon.MonBehaviourState == DataMonBehaviourState.isNeutral)
        {
            NeutralPatrol();
            return;
        }
        if (seeker.IsDone() && (Vector3.Distance(transform.position, GameManager.instance.Player.transform.position) > GameManager.instance.MaxDistForCompanionDataMon 
            && Vector2.Distance(patrollingAnchor.transform.position, targetPos) > 1) || NeutralStartPath)
        {
            NeutralStartPath = false;
            targetPos = patrollingAnchor.transform.position;
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
            goingToPos = new Vector3(
                Mathf.Clamp(goingToPos.x, GameManager.instance.DataWorldBorderLeftX,
                GameManager.instance.DataWorldBorderRightX),
                Mathf.Clamp(goingToPos.y, GameManager.instance.DataWorldBorderDownY, GameManager.instance.DataWorldBorderUpY));

            seeker.StartPath(transform.position, goingToPos, OnPathingComplete);
        }
    }
    #endregion
    private void ToFixedUpdate()
    {
        MoveAI();
    }

    Vector2 Dir;
    Vector2 Force;
    float distance;
    Quaternion toRotate;
    private void MoveAI()
    {
        if (AI_state == AI_State.Produce || AI_state == AI_State.Attack)
        {
            Dir = (transform.position - Target.position).normalized;
            toRotate = Quaternion.LookRotation(transform.forward, -Dir);
            print("why u not looking at object");
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, (GameManager.instance.DataMonsRotationSpeed * 3) * Time.fixedDeltaTime);
        }
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else if(AI_state != AI_State.Produce && AI_state != AI_State.Attack)
        {
            reachedEndOfPath = false;
        }
        if (doingSomething && (AI_state == AI_State.Attack || AI_state == AI_State.Produce))
        {
            print("Still Attacking");
            reachedEndOfPath = true;
            return;
        }
        if (AI_state == AI_State.Attack && Target == null)
            return;

        
        
        if (reachedEndOfPath)
        {

            return;
        }

        if (Vector2.Distance(transform.position, GameManager.instance.Player.transform.position) < GameManager.instance.DataMonEnableRbInRadius && rb.bodyType != RigidbodyType2D.Dynamic)
            rb.bodyType = RigidbodyType2D.Dynamic;
        if (Vector2.Distance(transform.position, GameManager.instance.Player.transform.position) > GameManager.instance.DataMonEnableRbInRadius && rb.bodyType != RigidbodyType2D.Static)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }


        Dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        if(Vector2.Distance(transform.position,GameManager.instance.Player.transform.position) < GameManager.instance.DataMonSpawnRadiusFromPlayer)
        {
            Force = Dir * DataMon.baseAttributes.BaseMoveSpeed * Time.fixedDeltaTime;

            rb.AddForce(Force);

        }
        else
        {
            transform.position += (Vector3)Dir*Time.fixedDeltaTime;
        }
        

        if (AI_state != AI_State.Produce && AI_state != AI_State.Attack)
        {
            toRotate = Quaternion.LookRotation(transform.forward, Dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, GameManager.instance.DataMonsRotationSpeed * Time.fixedDeltaTime);

        }
        
        distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance < NextWaypointDist)
        {
            currentWaypoint++;
        }
    }
    float currentAttackingTime;
    void DataMonDoThings()
    {
        
        switch (AI_state)
        {
            case AI_State.Attack:
                if (DataMon == null)
                    return;
                if (DataMon.attackObjects.Count == 0)
                    return;

                if (!doingSomething && Vector2.Distance(Target.position,transform.position) < DataMon.attackObjects[DataMon.currentAttackIndex].attackObject.AttackRange &&
                    DataMon.attackObjects[DataMon.currentAttackIndex].isAvailable)
                {
                    doingSomething = true;
                    currentAttackingTime = 0;
                    DataMon.StartAttack(Target);
                }
                if (!doingSomething)
                    return;

                if (currentAttackingTime >= DataMon.attackObjects[DataMon.currentAttackIndex].AttackDelayDataMonAfterFiring)
                {
                    doingSomething = false;
                }
                else
                    currentAttackingTime += Time.deltaTime;
                // Instantiate Attack
                break;
            case AI_State.Produce:
                if (!doingSomething && Vector2.Distance(Target.position, transform.position) < ProductionDistance)
                {
                    doingSomething = true;
                    rb.velocity = Vector2.zero;
                }
                if (!doingSomething)
                    return;
                if(CurrentProductionProgress>= DataMonProductionTime)
                {
                    CraftedItem();
                    DataBytesUsed = 0;
                    CancelProduction();
                    doingSomething = false;
                }
                else
                {
                    glitch.ShakeGlitch();
                    CurrentProductionProgress += Time.deltaTime;
                }
                // Instantiate Attack
                break;
            case AI_State.Patrol:
                doingSomething = false;
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ProductionDistance);

    }
    private void OnDestroy()
    {
        GameManager.instance.DataMon_UpdateAI -= UpdateDatamonAI;
        GameManager.instance.Entity_Updates -= ToUpdate;
        GameManager.instance.Entity_FixedUpdates -= ToFixedUpdate;

    }
    public void CraftedItem()
    {
        glitch.DamageGlitch(GlitchDamage);
        Instantiate(ItemCrafted, transform.position, transform.rotation);
    }
    public void Produce(AI_Tasks DoTask, float productionTime, int _DataBytesUsed, Transform ResourceTarget, GameObject itemPickupPrefab, float glitchDamage)
    {
        CurrentProductionProgress = 0;
        AI_state = AI_State.Produce;
        DataMonProductionTime = productionTime;
        CurrentTask = DoTask;
        DataBytesUsed = _DataBytesUsed;
        Target = ResourceTarget;
        glitch =Target.gameObject.GetComponent<GlitchObject>();
        ItemCrafted = itemPickupPrefab;
        GlitchDamage = glitchDamage;
        targetPos = Target.position;
        seeker.StartPath(transform.position, targetPos, OnPathingComplete);
    }
    public void CancelProduction()
    {
        doingSomething = false;
        Target = null;
        AI_state = AI_State.Patrol;
        CurrentTask = AI_Tasks.Patrolling;
        for (int i = 0; i < DataBytesUsed; i++)
        {
            Instantiate(GameManager.instance.DatabytesPrefab, transform.position, Quaternion.identity);
        }
    }
}
public enum AI_State
{
    Patrol, Attack, Support, Produce
}
public enum AI_Tasks
{
    Patrolling, Attacking, ProducingHuntingRifle, ProducingShotgun, ProducingAssaultRifle, ProducingAmmo
}