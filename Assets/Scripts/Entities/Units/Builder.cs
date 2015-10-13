using UnityEngine;
using System.Collections;
using System;

public class Builder : MonoBehaviour, IUnit {

    public float buildingCooldown;
    public int laborPower;

    private Unit _unit;
    private bool _building;
    private float _nextBuildTime;
    private Building _buildingToBuild;

    public void Init(Unit unit)
    {
        _unit = unit;
        _unit.pathing.OnDestinationReached += StartBuilding;
        _unit.pathing.OnNewDestination += NewBuildOrder;
    }

    public void OnSpawn()
    {

    }

    public void OnUpdate()
    {
        if (_building)
        {
            if(Time.time >= _nextBuildTime)
            {
                BuildBuilding();
                _nextBuildTime = Time.time + buildingCooldown;
            }
        }
    }

    public void OnAttack()
    {

    }

    public void OnDeath()
    {

    }

    private void BuildBuilding()
    {
        _buildingToBuild.AddLabor(laborPower);
        _buildingToBuild.ChangeHealth(ChangeMode.Set, _buildingToBuild.maxHitPoints * (_buildingToBuild.requiredLabor / _buildingToBuild.currentLabor));
    }


    private void NewBuildOrder(ActionType actionType, Entity target)
    {
        if (actionType == ActionType.Build)
        {
            if (target != null)
                if (target.GetType() == typeof(Building))
                    _buildingToBuild = target as Building;
        }
    }

    private void StartBuilding(ActionType actionType, Entity target)
    {
        if(actionType == ActionType.Build)
        {
            _building = true;
            _nextBuildTime = Time.time + buildingCooldown;
        }
    }
}
