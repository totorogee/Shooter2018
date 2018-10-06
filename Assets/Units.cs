using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupOfUnits<T> : Units where T : Units
{
    [SerializeField] public List<T> AllMembers = new List<T>();

    public Transform Center;

    public override void Change()
    {
        foreach (var item in AllMembers)
        {
            item.Change();
        }
    }

    public override void GetUnitsList()
    {
        base.GetUnitsList();
    }

}

public class Units : MonoBehaviour
{
    [SerializeField] public static List<Units> AllUnits = new List<Units>();

    public void Start()
    {
        AllUnits.Add(this);
    }

    public virtual void Change()
    {
        Debug.Log(this.GetType().ToString() + " Changed");
    }

    public virtual void GetUnitsList()
    {

    }

    public virtual void TeampUp( Groups groups)
    {
  
        groups.AllUnits.Add(this); // 
    }
}

