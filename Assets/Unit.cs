using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    [SerializeField]
    public static List<Unit> AllUnits = new List<Unit>();
    

    private void Awake()
    {
        AllUnits.Add(this);
    }

}
