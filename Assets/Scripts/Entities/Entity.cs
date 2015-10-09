using UnityEngine;

public enum EntityType
{
    Unit,
    Building
}

public class Entity : MonoBehaviour {

    [SerializeField] protected string _entityName;
    [SerializeField] protected int _maxHitPoints;
    protected int _currentHitPoints;
    protected int _teamID;
    protected EntityType _type;
    protected bool _selected;

    public delegate void EntityEventDelegate();
    public EntityEventDelegate OnDamageReceive;
    public EntityEventDelegate OnSelect;
    public EntityEventDelegate OnDeselect;

    public void Init()
    {
        if (GetType() == typeof(Unit))
            _type = EntityType.Unit;
        else if (GetType() == typeof(Building))
            _type = EntityType.Building;

        if(_entityName == null || _entityName == "")
        {
            if (_type == EntityType.Building)
                _entityName = "Unamed Building";
            else
                _entityName = "Unnamed Unit";
        }

        _currentHitPoints = _maxHitPoints;
        _selected = false;
    }

    //methods
    public void DealDamage(int damage)
    {
        _currentHitPoints -= damage;
        if (_currentHitPoints <= 0)
        {
            _currentHitPoints = 0;
            //Kill
        }

        if (OnDamageReceive != null)
            OnDamageReceive();
    }

    public void Select()
    {
        transform.tag = Tags.SELECTED_ENTITY;
        _selected = true;

        if (OnSelect != null)
            OnSelect();
    }

    public void Deselect()
    {
        transform.tag = Tags.UNSELECTED_ENTITY;
        _selected = false;

        if (OnDeselect != null)
            OnDeselect();
    }


    //Unity events
    protected void Update()
    {
        if (Input.GetMouseButtonUp(0))
            if (_mouseOver)
                Select();
            else
                Deselect();
    }

    private bool _mouseOver = false;
    private void OnMouseEnter()
    {
        _mouseOver = true;
    }

    private void OnMouseExit()
    {
        _mouseOver = false;
    }


    //Properties
    public string entityName
    {
        get
        {
            return _entityName;
        }
    }    

    public int teamID
    {
        get
        {
            return _teamID;
        }
        set
        {
            _teamID = value;
            //Update faction colors
        }
    }

    public bool isAlive
    {
        get
        {
            if (_currentHitPoints <= 0)
                return false;
            return true;
        }
    }
   
    public int currentHitPoints
    {
        get
        {
            return _currentHitPoints;
        }
    }

    public int maxHitPoints
    {
        get
        {
            return _maxHitPoints;
        }
    }


}
