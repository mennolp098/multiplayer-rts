using UnityEngine;
using Pathfinding;

public class UnitPath : MonoBehaviour {

    public const float NEXT_WAYPOINT_DISTANCE = 1.15f;


    private Path currentPath;
    private Seeker seeker;
    private CharacterController controller;

    private int currentPathPoint = 0;
    private bool movementState = true;
    private bool pathReached = false;

    //make unit stat class
    private float movementSpeed = 300f;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!movementState || currentPath == null || pathReached)
            return;

        Vector3 dir = (currentPath.vectorPath[currentPathPoint] - transform.position).normalized;
        dir *= movementSpeed * Time.deltaTime;
        controller.SimpleMove(dir);

        if (Vector3.Distance(transform.position, currentPath.vectorPath[currentPathPoint]) < NEXT_WAYPOINT_DISTANCE)
        {
            currentPathPoint++;
            if (currentPathPoint >= currentPath.vectorPath.Count)
                pathReached = true;
        }
    }

    private void OnDrawGizmos()
    {
        if(currentPath != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(currentPath.vectorPath[currentPath.vectorPath.Count - 1], 1f);
        }
    }


    public void SetDestination(Vector3 point)
    {
        seeker.StartPath(transform.position, point, ReceivePath);
    }

    private void ReceivePath(Path path)
    {
        if (path.error)
        {
            Debug.Log("pathing error: " + path.error);
        }
        else
        {
            currentPath = path;
            currentPathPoint = 0;
            pathReached = false;
        }
    }
    
    public void SetMovementActive(bool state)
    {
        movementState = state;
    }
    
}
