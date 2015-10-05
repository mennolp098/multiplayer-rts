using UnityEngine;

public enum EntityType
{
    Unity,
    Building
}

public class Entity : MonoBehaviour {

    [SerializeField] protected string _entityName;
    [SerializeField] protected int _hitPoints;
    protected int _teamID;
    protected EntityType _type;

    public void Init()
    {

    }


    //methods
    public void DealDamage(int damage)
    {
        _hitPoints -= _hitPoints;
        if (_hitPoints <= 0)
        {
            _hitPoints = 0;
            //Kill
        }
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
            if (_hitPoints <= 0)
                return false;
            return true;
        }
    }
   

}
