using UnityEngine;
using System.Collections;

public class BuildingControls : MonoBehaviour {

    [SerializeField] private Building _selectedBuilding;

    private GameObject _buildingPreview;
    private Vector3 _lastPosition;
    private bool _canBuild;

    private void Start()
    {
        _buildingPreview = _selectedBuilding.gameObject;
    }

    private RaycastHit hit;
    private void LateUpdate() //Late update so units can move before checking building position
    {
        if (_selectedBuilding == null || _buildingPreview == null)
            return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200, LayerMask.GetMask("Ground")))
        {
            _buildingPreview.transform.position = hit.point;

            if (hit.point != _lastPosition)
            {
                _lastPosition = hit.point;
                _canBuild = _selectedBuilding.isBuildPositionValid;
                if (_canBuild)
                    _selectedBuilding.SetModelColor(Color.green);
                else
                    _selectedBuilding.SetModelColor(Color.red);

            }

            if (Input.GetMouseButtonUp(0) && _canBuild)
            {
                //Take resources
                //Send signal to other player(s)
                _selectedBuilding.Place();
                _selectedBuilding = null;
                _buildingPreview = null;
            }
        }
    }

}
