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
        }
    }

}
