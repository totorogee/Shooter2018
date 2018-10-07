using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Main : MonoBehaviour
{
    public static Main Instance;

    [SerializeField] protected Text debugSelection;

    [SerializeField] protected Slider debugMidRangeSlider;
    [SerializeField] protected Text debugMidRangeText;

    [SerializeField] protected Slider debugMeleeRangeSlider;
    [SerializeField] protected Text debugMeleeRangeText;

    [SerializeField] protected Units BluePrefab;
    [SerializeField] protected Units RedPrefab;
    [SerializeField] public Transform BlueContainer;
    [SerializeField] public Transform RedContainer;

    [SerializeField] protected float boundary;

    [SerializeField] protected float updateTime = 0.2f;

    
    protected float nextUpdate = 0;

    public float MidRange = 1f;
    public float MeleeRange = 0.2f;

    private Groups selected;

    public Groups Blue;
    public Groups Red;

    private bool combatOn = false;
    private bool forwardPressed = false;

    public void Awake()
    {
        Instance = this;
        Blue = new Groups();
        Blue.Center.localPosition = new Vector3(0, 0, -2);
        Blue.Center.localEulerAngles = new Vector3(0, 180, 0);
        BlueContainer.localEulerAngles = new Vector3(0, 180, 0);
        Red = new Groups();
        Red.Center.localPosition = new Vector3(0, 0, 2);
        selected = Blue;

    nextUpdate = Time.time;
    }


    private void Start()
    {
        Red.MeleePower = 1f;
        Blue.MeleePower = 0f;

        Red.MidRangePower = 0f;
        Blue.MidRangePower = 1f;

        SetMeleeRange(true);
        SetMidRange(true);
        RandomSpwan();
        ToCircle();
        selected = Red;
        ForwardPressed();
        RandomSpwan();
        ToCircle();
        ChangeSelection();
        Combat();


    }

    public void RandomSpwan()
    {
        Units units = (selected == Blue) ? BluePrefab : RedPrefab;
        Transform unitContainer = (selected == Blue) ? BlueContainer : RedContainer ;


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
        if (forwardPressed)
        {
            MoveForward();
        }
    }

    public void TimedUpdate()
    {
        selected.MoveUnits();

        if (combatOn)
        {
            Blue.Combat();
            Red.Combat();
        }

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

    public void Combat()
    {
        combatOn = !combatOn;
    }

    public void SetMidRange(bool init = false)
    {
        if (init)
        {
            debugMidRangeSlider.value = MidRange;
        }

        MidRange = debugMidRangeSlider.value;
        debugMidRangeText.text = MidRange.ToString("F2");
    }

    public void SetMeleeRange(bool init = false)
    {
        if (init)
        {
            debugMeleeRangeSlider.value = MeleeRange;
        }

        MeleeRange = debugMeleeRangeSlider.value;
        debugMeleeRangeText.text = MeleeRange.ToString("F2");
    }

    public void ForwardPressed()
    {
        forwardPressed = !forwardPressed;

    }

    public void MoveForward()
    {
        float distant = Mathf.Infinity;
        Vector3 target = Vector3.zero;

        foreach (var item in Blue.Formations )
        {
            if (item.Broken) { continue; }

            if ( (item.Center.position - Red.Center.position).sqrMagnitude < distant)
            {
                target = item.Center.position;
            }
        }

        Vector3 dir = target - Red.Center.position;
        Red.Center.localPosition += dir.normalized * 0.005f;
    }
}


