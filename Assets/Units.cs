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

[RequireComponent (typeof(LineRenderer))]
public class Units : MonoBehaviour
{
    [SerializeField] public static List<Units> AllUnits = new List<Units>();

    [SerializeField] public Transform breakEffect;
    public Groups MyGroup;
    private LineRenderer lineRenderer;

    private float randomDelay = 1f;
    private float attactCoolTime = 1f;
    private float attactCoolTimeNext = 0f; 
    private float attactShowing = 0.1f;

    public bool Broken = false;

    public float Hp
    {
        get {
            return hp;
        }
        set
        {
            hp = value;
            if (hp <= 0)
            {
                BreakTrigger();
            }
        }
    }

    private float hp = 12;

    private void BreakTrigger()
    {
         if (!Broken)
        {
            Broken = true;
            breakEffect.gameObject.SetActive(true);
            Color old = GetComponent<MeshRenderer>().material.color;
            GetComponent<MeshRenderer>().material.color = Color.Lerp(old, Color.black, 0.8f);
        }

    }

    public void Start()
    {
        AllUnits.Add(this);
        lineRenderer = GetComponent<LineRenderer>();
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
        MyGroup = groups;
        groups.AllUnits.Add(this); // 
    }

    public virtual void Combat()
    {
        if (Broken) { return; }

        Groups enemy = (MyGroup == Main.Instance.Blue) ? Main.Instance.Red : Main.Instance.Blue;
        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, this.transform.position);

        if (Time.time > attactCoolTimeNext)
        {
            foreach (var item in enemy.AllUnits)
            {
                if ((this.transform.position - item.transform.position).sqrMagnitude <= Main.Instance.Range * Main.Instance.Range)
                {
                    if (item.hp <= 0) { continue; }

                    StopAllCoroutines();
                    StartCoroutine(Attact(randomDelay, attactShowing, this, item));
                    break;
                }
            }
        }


    }

    public virtual IEnumerator Attact (float delay, float last, Units me, Units them)
    {
        delay = Random.Range(0, delay);

        yield return new WaitForSeconds(delay);
        lineRenderer.SetPosition(0, me.transform.position);
        lineRenderer.SetPosition(1, them.transform.position);
        them.Hp --;

        yield return new WaitForSeconds(last);
        lineRenderer.SetPosition(0, me.transform.position);
        lineRenderer.SetPosition(1, me.transform.position);

        attactCoolTimeNext = attactCoolTime + Time.time;

    }

    


}

