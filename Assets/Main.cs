using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Main : MonoBehaviour
{
    public static Main Instance;

    [SerializeField] protected Text debugSelection;

    [SerializeField] protected Units BluePrefab;
    [SerializeField] protected Units RedPrefab;
    [SerializeField] public Transform unitContainer;
    [SerializeField] protected float boundary;

    [SerializeField] protected float updateTime = 0.2f;
    protected float nextUpdate = 0;

   private Groups selected;

    public Groups Blue;
    public Groups Red;

    public void Awake()
    {
        Instance = this;
        Blue = new Groups();
        Blue.Center.localPosition = new Vector3(0, 0, -2);
        Blue.Center.localEulerAngles = new Vector3(0, 180, 0);
        Red = new Groups();
        Red.Center.localPosition = new Vector3(0, 0, 2);
        selected = Blue;

    nextUpdate = Time.time;
    }


    private void Start()
    {
    }

    public void RandomSpwan()
    {
        Units units = (selected == Blue) ? BluePrefab : RedPrefab;


        for (int i = 0; i < 24; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-1f, 1f), unitContainer.position.y, Random.Range(-1f, 1f));
            var temp = Instantiate(units.gameObject, randomPos * boundary, unitContainer.rotation, unitContainer);
            temp.GetComponent<Units>().TeampUp(selected);
            selected.ModeUpdate();
        }
    }

    public void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Time.time + updateTime;
            TimedUpdate();
        }
       
    }

    public void TimedUpdate()
    {
        selected.MoveUnits();
    }

    public void ToCircle()
    {
        selected.ModeUpdate();
        selected.ToCircle();
    }

    public void ToTriangle()
    {
        selected.ModeUpdate();
        selected.ToTriangle();
    }

    public void ToSquare()
    {
        selected.ModeUpdate();
        selected.ToSquare();
    }

    public void Mode()
    {
        selected.ModeSwitch();
    }

    public void ChangeSelection()
    {


        if (selected == null)
        {
            selected = Blue;
            debugSelection.text = "Selection (Blue)";
            return;
        }

        selected = (selected == Blue) ? Red : Blue;
        debugSelection.text = (selected == Blue) ? "Selection (Blue)" : "Selection (Red)";


    }
}


