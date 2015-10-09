using UnityEngine;
using System.Collections;

public class Unit : Entity
{
    public float movementSpeed;

    private void Start()
    {
        base.Init();

        GetComponent<UnitPath>().SetUnitInfo(this);        
    }

    private new void Update()
    {
        base.Update();
    }

}
