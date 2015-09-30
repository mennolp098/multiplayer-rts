
using UnityEngine;
using Pathfinding;

public class UnitPathingController : MonoBehaviour
{
    private RaycastHit hit;
    private void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (Input.GetMouseButtonUp(0))
            {
                GameObject[] units = GameObject.FindGameObjectsWithTag("SelectedUnit");
                for (int i = 0; i < units.Length; i++)
                {
                    Vector3 destination = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    //destination = new Vector3(Random.Range(0, 20), 6, Random.Range(0, 20));
                    units[i].GetComponent<UnitPath>().SetDestination(destination);
                }

            }
        }
    }
}
  