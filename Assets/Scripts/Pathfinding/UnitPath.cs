using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Networking;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(NetworkIdentity))]
public class UnitPath : NetworkBehaviour {

    public const float NEXT_WAYPOINT_DISTANCE = 1.15f;

	private NetworkIdentity _myNetworkIdentity;
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
		_myNetworkIdentity = GetComponent<NetworkIdentity>();
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

    public void SetUnitInfo(Unit entity)
    {
        this.movementSpeed = entity.movementSpeed;
		if(_myNetworkIdentity.hasAuthority)
		{
	        entity.OnSelect += ShowLine;
	        entity.OnDeselect += HideLine;
		}
    }

	//Setting destination on unit for all clients
	[ClientRpc]
    public void RpcSetDestination(Vector3 point)
    {
        seeker.StartPath(transform.position, point, ReceivePath);
    }

	//Giving the position to the server so he can tell every client what the new destination will be
	[Command]
	public void CmdGiveDestination(Vector3 point)
	{
		RpcSetDestination(point);
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

    private void ShowLine()
    {
        lineRenderer.enabled = true;
    }

    private void HideLine()
    {
        lineRenderer.enabled = false;
    }

    public void SetMovementActive(bool state)
    {
        movementState = state;
    }
    
}
