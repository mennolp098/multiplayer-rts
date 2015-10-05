using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(LineRenderer))]
public class UnitPath : MonoBehaviour {

    public const float NEXT_WAYPOINT_DISTANCE = 1.15f;

    private List<Vector3> currentPath;
    private Seeker seeker;
    private CharacterController controller;
    private LineRenderer lineRenderer;

    private bool movementState = true;
    private bool pathReached = false;

    //make unit stat class
    private float movementSpeed = 300f;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (!movementState || currentPath == null || pathReached)
            return;

        Vector3 dir = (currentPath[0] - transform.position).normalized;
        dir *= movementSpeed * Time.deltaTime;
        controller.SimpleMove(dir);

        if (Vector3.Distance(transform.position, currentPath[0]) < NEXT_WAYPOINT_DISTANCE)
        {
            currentPath.Remove(currentPath[0]);
            UpdatePathLine();
            if (currentPath.Count <= 0)
            {
                lineRenderer.enabled = false;
                pathReached = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(currentPath != null)
        {
            if (currentPath.Count > 0)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(currentPath[currentPath.Count - 1], 1f);
            }
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
            currentPath = path.vectorPath;
            pathReached = false;

            lineRenderer.enabled = true;
            UpdatePathLine();
        }
    }

    private void UpdatePathLine()
    {
        lineRenderer.SetVertexCount(currentPath.Count);
        for (int i = 0; i < currentPath.Count; i++)
        {
            lineRenderer.SetPosition(i, currentPath[i]);
        }
    }


    public void SetMovementActive(bool state)
    {
        movementState = state;
    }
    
}
