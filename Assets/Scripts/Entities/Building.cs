using UnityEngine;

public interface IBuilding
{
    void OnBuild();
    //TODO: method that gets list of aviable units to build
}

[RequireComponent(typeof(IBuilding))]
public class Building : Entity
{
    private const float MAX_BUILD_CORNER_DIFFERENCE = 0.65f;

    [SerializeField] private GameObject[] _cornerPoints;
    [SerializeField] private IBuilding _buildingSpecificBehaviour;

    private bool _built = false;
    private GameObject _buildingModel;

    private void Start()
    {
        base.Init();

        CheckBuildPosition();

        _buildingSpecificBehaviour = GetComponent<IBuilding>();
        _buildingModel = transform.FindChild("Model").gameObject;
    }

    public void Build()
    {
        _built = true;
    }

    private new void Update()
    {
        base.Update();

        if (!_built)
            UnbuiltBehaviour();
    }

    private void UnbuiltBehaviour()
    {
        if (CheckBuildPosition())
            _buildingModel.GetComponent<Renderer>().material.color = Color.green;
        else
            _buildingModel.GetComponent<Renderer>().material.color = Color.red;
    }

    public bool CheckBuildPosition()
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
        return result;
    }

}
