using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Main : MonoBehaviour
{
    public static Main Instance;

    [SerializeField] protected Unit unitPrefab;
    [SerializeField] public Transform unitContainer;
    [SerializeField] protected float boundary;

    public Groups Blue;

    public void Awake()
    {
        Instance = this;
        Blue = new Groups();
        Blue.ModeSwitch();
    }

    public void RandomSpwan()
    {
        Vector3 randomPos = new Vector3(Random.Range(-1f, 1f), unitContainer.position.y, Random.Range(-1f, 1f));
        Instantiate(unitPrefab.gameObject, randomPos * boundary, Quaternion.identity, unitContainer);
        Blue.ModeUpdate();
    }

    public void ToCircle()
    {
        Blue.ModeUpdate();
        Blue.ToCircle();
    }

    public void ToTriangle()
    {
        Blue.ModeUpdate();
        Blue.ToTriangle();
    }

    public void ToSquare()
    {
        Blue.ModeUpdate();
        Blue.ToSquare();
    }

    public void Mode()
    {
        Blue.ModeSwitch();
    }
}


