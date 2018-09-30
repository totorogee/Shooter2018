using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Unit : MonoBehaviour {

    [SerializeField] public static List<Unit> AllUnits = new List<Unit>();
    

    private void Start()
    {
        AllUnits.Add(this);
        Main.Instance.Blue.AllUnits.Add(this);
    }
    
}
