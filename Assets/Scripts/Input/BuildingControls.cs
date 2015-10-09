using UnityEngine;
using System.Collections;

public class BuildingControls : MonoBehaviour {

    [SerializeField] private Building _selectedBuilding;

    private GameObject _buildingPreview;
    private Vector3 _lastPosition;

    private void Start()
    {
        _buildingPreview = _selectedBuilding.gameObject;
    }


    private RaycastHit hit;
    private void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200, LayerMask.GetMask("Ground")))
        {
            _buildingPreview.transform.position = hit.point;

            if (hit.point != _lastPosition)
            {
                _lastPosition = hit.point;
                if (_selectedBuilding.CheckBuildPosition())
                    _selectedBuilding.SetModelColor(Color.green);
                else
                    _selectedBuilding.SetModelColor(Color.red);

            }
        }
    }

}
