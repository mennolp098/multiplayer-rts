using UnityEngine;
using UnityEngine.Networking;

public enum EntityType
{
    Unit,
    Building
}

public enum ChangeMode
{
    Add,
    Decrease,
    Set
}

public class Entity : MonoBehaviour {

    [SerializeField] protected string _entityName;
    [SerializeField] protected int _maxHitPoints;
    [SerializeField] protected int _requiredLabor;

    protected int _currentHitPoints;
    protected int _teamID;
    protected int _currentLabor = 0;
    protected EntityType _type;
    protected bool _selected;
    protected bool _mouseOver = false;
    protected bool _isInSelectionBox = false;
	protected NetworkIdentity _myNetworkIdentity;

    public delegate void EntityEventDelegate();
    public delegate void EntityValueChangeEvent(int addition);
    public EntityEventDelegate OnSelect;
    public EntityEventDelegate OnDeselect;

    public EntityValueChangeEvent OnDamageReceive;
    public EntityValueChangeEvent OnConstructionProgress;

    protected void Init()
    {
		_myNetworkIdentity = GetComponent<NetworkIdentity>();

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
    public void ChangeHealth(ChangeMode mode, int changeValue)
    {
        //Set
        switch (mode)
        {
            case ChangeMode.Add:
                _currentHitPoints += changeValue;
                break;
            case ChangeMode.Decrease:
                _currentHitPoints -= changeValue;
                break;
            case ChangeMode.Set:
                _currentHitPoints = changeValue;
                break;
        }

        //Checks
        if (_currentHitPoints > _maxHitPoints)
        {
            _currentHitPoints = _maxHitPoints;
        }
        if (_currentHitPoints <= 0)
        {
            _currentHitPoints = 0;
            //Kill
        }

        //Delegates
        if (OnDamageReceive != null)
            OnDamageReceive(changeValue);
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

    }

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

    public EntityType entityType
    {
        get
        {
            return _type;
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

    public int requiredLabor
    {
        get
        {
            return _requiredLabor;
        }
    }

    public int currentLabor
    {
        get
        {
            return _currentLabor;
        }
    }
}
