
using UnityEngine;
using Pathfinding;

public class UnitPathingController : MonoBehaviour
{
    private RaycastHit hit;
    private void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (Input.GetMouseButtonUp(1))
            {
                GameObject[] units = GameObject.FindGameObjectsWithTag(Tags.SELECTED_ENTITY);
                
                for (int i = 0; i < units.Length; i++)
                {
                    units[i].GetComponent<UnitPath>().SetDestination(hit.point);
                }
            }
        }
    }
}
  