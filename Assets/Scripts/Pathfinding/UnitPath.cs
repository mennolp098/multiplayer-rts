using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Networking;

public enum ActionType
{
    None,
    Move,
    Attack,
    Build
}

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(NetworkIdentity))]
public class UnitPath : NetworkBehaviour {

    public const float NEXT_WAYPOINT_DISTANCE = 1.15f;
    
    private NetworkIdentity _myNetworkIdentity;
    private List<Vector3> _currentPath;
    private Seeker _seeker;
    private CharacterController _controller;
    private LineRenderer _lineRenderer;

    private bool _movementState = true;
    private bool _pathReached = false;
    private ActionType _actionType = ActionType.None;

    //make unit stat class
    private float _movementSpeed = 300f;

    public delegate void UnitMovementEvent(ActionType actionType, Entity target = null);
    public UnitMovementEvent OnDestinationReached;
    public UnitMovementEvent OnNewDestination;

    private void Start()
    {
        _myNetworkIdentity = GetComponent<NetworkIdentity>();
        _seeker = GetComponent<Seeker>();
        _controller = GetComponent<CharacterController>();
        _lineRenderer = GetComponent<LineRenderer>();
        _movementSpeed = GetComponent<Unit>().movementSpeed;
    }

    private void Update()
    {
        if (!_movementState || _currentPath == null || _pathReached)
            return;

        Vector3 dir = (_currentPath[0] - transform.position).normalized;
        dir *= _movementSpeed * Time.deltaTime;
        _controller.SimpleMove(dir);

        if (Vector3.Distance(transform.position, _currentPath[0]) < NEXT_WAYPOINT_DISTANCE)
        {
<<<<<<< HEAD
            _currentPath.Remove(_currentPath[0]);
            UpdatePathLine();
            if (_currentPath.Count <= 0)
=======
            currentPath.Remove(currentPath[0]);

            if(_myNetworkIdentity.hasAuthority)
                UpdatePathLine();

            if (currentPath.Count <= 0)
>>>>>>> 01a62de76d1f391a1c4957e9a307c071c6ec3c63
            {
                _pathReached = true;
                if (OnDestinationReached != null)
                    OnDestinationReached(_actionType);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(_currentPath != null)
        {
            if (_currentPath.Count > 0)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_currentPath[_currentPath.Count - 1], 1f);
            }
        }
    }

    public void SetUnitInfo(Unit entity)
    {
        this._movementSpeed = entity.movementSpeed;
        entity.OnSelect += ShowLine;
        entity.OnDeselect += HideLine;
    }

	//Setting destination on unit for all clients
	[ClientRpc]
    public void RpcSetDestination(Vector3 point)
    {
        _seeker.StartPath(transform.position, point, ReceivePath);
       // _actionType = actionType;

        //TODO: fix type + target
        if (OnNewDestination != null)
            OnNewDestination(ActionType.Move);
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
            _currentPath = path.vectorPath;
            _pathReached = false;
            UpdatePathLine();
        }
    }

    private void UpdatePathLine()
    {
        _lineRenderer.SetVertexCount(_currentPath.Count);
        for (int i = 0; i < _currentPath.Count; i++)
        {
            _lineRenderer.SetPosition(i, _currentPath[i]);
        }
    }

    private void ShowLine()
    {
        _lineRenderer.enabled = true;
    }

    private void HideLine()
    {
        _lineRenderer.enabled = false;
    }

    public void SetMovementActive(bool state)
    {
        _movementState = state;
    }
    
}
