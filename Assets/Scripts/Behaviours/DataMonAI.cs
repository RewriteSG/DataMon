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

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool doingSomething = false;

    Seeker seeker;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        _dataMon = GetComponent<DataMon.IndividualDataMon.DataMon>();
        InvokeRepeating("TestAI", 0, .5f);
    }

    Vector2 Dir;
    Vector2 Force;
    float distance;
    // Update is called once per frame
    void Update()
    {
        if (reachedEndOfPath && !Target.isNull())
        {
            DataMonDoThings();
        }
    }
    void TestAI()
    {
        if(seeker.IsDone())
            seeker.StartPath(transform.position, Target.position, OnPathingComplete);
        if(Vector3.Distance(transform.position,Target.position)> NextWaypointDist)
        {
            reachedEndOfPath = false;
        }
    }
    private void FixedUpdate()
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
        else if (!doingSomething)
        {
            reachedEndOfPath = false;
        }
        Dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Force = Dir * _dataMon.dataMon.BaseAttributes.BaseMoveSpeed * Time.fixedDeltaTime;

        rb.AddForce(Force);
        distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if(distance < NextWaypointDist)
        {
            currentWaypoint++;
        }
        if (!reachedEndOfPath)
        {
            Quaternion toRotate = Quaternion.LookRotation(transform.forward, Dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, GameManager.instance.DataMonsRotationSpeed*Time.fixedDeltaTime);
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
                // Instantiate Attack
                break;

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, NextWaypointDist);
    }
    public void CommandDataMon(Transform _target, AI_State state)
    {
        AI_state = state;
        Target = _target;
        seeker.StartPath(transform.position, Target.position, OnPathingComplete);
    }
    void OnPathingComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;

        }
    }
}
public enum AI_State
{
    Patrol, Attack, Defense, Produce
}
public static class GameObjectExtensions
{
    public static bool isNull<T>(this T obj) where T : Transform
    {
        return obj.gameObject == null;
    }
}