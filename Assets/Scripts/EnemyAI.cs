using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour {

    public Transform target;
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    public Path path;

    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    public float nextWaypointDistance = 3f;

    private int currentWaypoint = 0;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            Debug.LogError("No player found");
            return;
        }
        seeker.startPath(transform.position, target.position, onPathComplete);

        StartCoroutine(updatePath());
    }

    IEnumerator updatePath()
    {
        if(target == null)
        {
            return false;
        }

        seeker.startPath(transform.position, target.position, onPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(updatePath());
    }

    public void onPathComplete(Path p)
    {
        Debug.Log("We got a path, did it have an error ?" + p.error);
        if (!path.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count){
            if (pathIsEnded)
                return;
            
            Debug.Log("End of path reached");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        rb.addForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance){
            currentWaypoint++;
            return;
        }
    }
} 
