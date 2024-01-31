using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class DataMonAI : MonoBehaviour
{
    public AI_State AI_state;
    public Transform Target;

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
    }

    Vector2 Dir;
    Vector2 Force;
    // Update is called once per frame
    void Update()
    {
        if (reachedEndOfPath)
        {
            DataMonDoThings();
        }
        if (path == null)
        {
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else if(!doingSomething)
        {
            reachedEndOfPath = false;
        }
        Dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Force = Dir * Speed * Time.deltaTime;
        
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
