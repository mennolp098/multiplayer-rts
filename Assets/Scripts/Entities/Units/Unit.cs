using UnityEngine;
using System.Collections;

public interface IUnit
{
    void Init(Unit unit);
    void OnSpawn();
    void OnDeath();
    void OnAttack();
    void OnUpdate();
}

public class Unit : Entity
{
    public float movementSpeed;
    public bool canBuild;

    private IUnit _unitBehaviour;
    private UnitPath _pathing;
    private UnitSelectionControls _pathcontroller;

    private Color _originalUnitColor;

    private void Start()
    {
        base.Init();

        _pathing = GetComponent<UnitPath>();
        _pathing.SetUnitInfo(this);

        _unitBehaviour = GetComponent<IUnit>();
        _pathcontroller = Camera.main.GetComponent<UnitSelectionControls>();
        _originalUnitColor = GetComponent<Renderer>().material.color;

        if (_unitBehaviour != null)
            _unitBehaviour.Init(this);
    }

    private new void Update()
    {
        base.Update();
        
            GetComponent<Renderer>().material.color = Color.green;
            GetComponent<Renderer>().material.color = _originalUnitColor;
    }

    public UnitPath pathing
    {
        get
        {
            return _pathing;
        }
    }
}
