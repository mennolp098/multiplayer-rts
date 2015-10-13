using UnityEngine;
using Pathfinding;

public interface IBuilding
{
    void OnBuild();
    void OnUpdate();
    //TODO: method that gets list of aviable units to build
}

public enum BuildState
{
    Placement,
    Building,
    Built
}

[RequireComponent(typeof(IBuilding))]
public class Building : Entity
{
    private const float MAX_BUILD_CORNER_DIFFERENCE = 0.65f;

    [SerializeField] private GameObject[] _cornerPoints;

    private BuildState _buildState;
    private GameObject _buildingModel;
    private IBuilding _buildingBehaviour;

    public delegate void BuildingStateEvent();
    public BuildingStateEvent OnPlace;
    public BuildingStateEvent OnBuild;

    private void Start()
    {
        base.Init();

        _buildingBehaviour = GetComponent<IBuilding>();
        _buildingModel = transform.FindChild("Model").gameObject;

        if(_buildingBehaviour != null)
            OnBuild += _buildingBehaviour.OnBuild;
    }

    public void Place()
    {
        _buildState = BuildState.Building;
        SetModelColor(Color.white, 0.6f);
        ChangeHealth(ChangeMode.Set, 1);

        //Scan Pathfinding navmesh
        AstarPath.active.UpdateGraphs(GetComponent<GraphUpdateScene>().GetBounds());

        if (OnPlace != null)
            OnPlace();
    }

    public void Build()
    {
        _buildState = BuildState.Built;
        SetModelColor(Color.white, 1f);

        if (OnBuild != null)
            OnBuild();
    }

    private new void Update()
    {
        switch (_buildState)
        {
            case BuildState.Placement:
                return;
            case BuildState.Built:
                _buildingBehaviour.OnUpdate();
                break;
            case BuildState.Building:
                break;
        }
        base.Update();
    }

    //Building methods
    public bool isBuildPositionValid
    {
        get
        {
            bool result = true;
            for (int i = 0; i < _cornerPoints.Length; i++)
            {
                Vector3 down = -_cornerPoints[i].transform.up * MAX_BUILD_CORNER_DIFFERENCE;

                RaycastHit hit;
                if (!Physics.Raycast(_cornerPoints[i].transform.position, down, out hit, MAX_BUILD_CORNER_DIFFERENCE, LayerMask.GetMask("Ground")))
                {
                    Debug.DrawRay(_cornerPoints[i].transform.position, down, Color.red);
                    result = false;
                }
                else
                {
                    Debug.DrawRay(_cornerPoints[i].transform.position, down, Color.blue);
                }
            }

            //TODO: check if collides with other buildings/units
            return result;
        }
    }

    public void SetModelColor(Color color, float alpha = 1f)
    {
        Color newColor = color;
        newColor.a = alpha;
        _buildingModel.GetComponent<Renderer>().material.color = newColor;
    }

    public void AddLabor(int laborForce)
    {
        _currentLabor += laborForce;
        if(_currentLabor >= _requiredLabor)
        {
            _currentLabor = _requiredLabor;
            Build();
        }

        if (OnConstructionProgress != null)
            OnConstructionProgress(laborForce);
    }


    public BuildState buildState
    {
        get
        {
            return _buildState;
        }
    }
}
