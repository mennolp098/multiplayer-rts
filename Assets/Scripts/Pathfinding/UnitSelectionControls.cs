
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionControls : MonoBehaviour
{
    private RaycastHit _hit;
    private Vector3 _startDragPos;
    private Rect _selection;
    [SerializeField] private Image _dragBox;

    private void Start()
    {
        _startDragPos = new Vector3(0, 0, 0);
        _selection = new Rect();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startDragPos = Input.mousePosition;
        }else if (Input.GetMouseButtonUp(0))
        {
            DeselectSelectedUnits();
            if(selection.width > 20 || selection.width < -20)
            {
                SelectSelectionboxTargets();
            }
            else
            {
                SelectSingleTarget();
            }

            _startDragPos = -Vector3.one;
        }

        if (Input.GetMouseButton(0))
        {
            _selection = new Rect(_startDragPos.x, 
                InvertMouseY(_startDragPos.y), 
                Input.mousePosition.x - _startDragPos.x, 
                InvertMouseY(Input.mousePosition.y) - InvertMouseY(_startDragPos.y));

            if (_selection.width < 0)
            {
                _selection.x += _selection.width;
                _selection.width = -_selection.width;
            }
            if (_selection.height < 0)
            {
                _selection.y += _selection.height;
                _selection.height = -_selection.height;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            MoveSelectedTargets();
        }
    }

    private void SelectSingleTarget()
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit))
        {
            if(_hit.transform.GetComponent<Entity>() != null)
            {
                _hit.transform.GetComponent<Entity>().Select();
            }
        }
    }

    private void SelectSelectionboxTargets()
    {
        Unit[] units = (GameObject.FindObjectsOfType<Unit>() as Unit[]);

        Debug.Log(units.Length);

        for (int i = 0; i < units.Length; i++)
        {
            if (isInSelectionBox(units[i]))
                units[i].Select();
        }
    }

    private void MoveSelectedTargets()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit))
        {
            if (Input.GetMouseButtonUp(1))
            {
                ActionType type = ActionType.None;
                Entity entity = null;

                if (_hit.transform.GetComponent<Entity>() != null)
                {
                    entity = _hit.transform.GetComponent<Entity>();
                }

                GameObject[] units = GameObject.FindGameObjectsWithTag(Tags.SELECTED_ENTITY);

                for (int i = 0; i < units.Length; i++)
                {
                    if(units[i].GetComponent<UnitPath>() != null)
                        units[i].GetComponent<UnitPath>().SetDestination(_hit.point, type, entity);
                }
            }
        }
    }

    private void DeselectSelectedUnits()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(Tags.SELECTED_ENTITY);

        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].GetComponent<Entity>().entityType == EntityType.Unit)
                units[i].GetComponent<Unit>().Deselect();
        }

    }

    bool isInSelectionBox(Unit unit)
    {
        bool result = false;
        if (unit.GetComponent<Renderer>().isVisible)
        {
            Vector3 camPos = Camera.main.WorldToScreenPoint(unit.transform.position);
            camPos.y = Screen.height - camPos.y;
            result = selection.Contains(camPos);
        }
        return result;
    }

    void OnGUI()
    {
        if(_startDragPos != -Vector3.one)
        {
            GUI.color = new Color(1, 1, 1, 0.5f);
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, GUI.color);
            tex.Apply();
            GUI.DrawTexture(_selection, tex);
        }
    }


    public float InvertMouseY(float y)
    {
        return Screen.height - y;
    }

    public Rect selection
    {
        get
        {
            return _selection;
        }
    }

    public Vector2 startMousePos
    {
        get
        {
            return _startDragPos;
        }
    }
}
  