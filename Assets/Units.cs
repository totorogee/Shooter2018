using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    private float midAttactCoolTime = 1f;
    private float meleeAttactCoolTime = 0.5f;
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

    public int Neighbour
    {
        get
        {
            return FoundNeighbour();
        }
    }

    private float hp = 12;

    private int FoundNeighbour(float area = 0.4f)
    {
        int result = 0;

        foreach (var item in MyGroup.AllUnits)
        {
            if (!item.Broken)
            {
                if ((this.transform.position - item.transform.position).sqrMagnitude <= area * area)
                {

                    result++;
                }
            }
        }

        return result;
    }

    private void BreakTrigger()
    {
         if (!Broken)
        {
            Broken = true;
            breakEffect.gameObject.SetActive(true);
            Color old = GetComponent<MeshRenderer>().material.color;
            GetComponent<MeshRenderer>().material.color = Color.Lerp(old, Color.black, 0.8f);
            GetComponent<MeshRenderer>().material.DOColor(Color.clear, 5f);
            this.transform.DOScale(0f, 5f);
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
                if ((this.transform.position - item.transform.position).sqrMagnitude <= Main.Instance.MeleeRange * Main.Instance.MeleeRange)
                {
                    if (item.hp <= 0) { continue; }

                    StopAllCoroutines();
                    StartCoroutine(MeleeAttact(randomDelay, attactShowing, this, item));
                    return;
                }
            }

            foreach (var item in enemy.AllUnits)
            {
                if ((this.transform.position - item.transform.position).sqrMagnitude <= Main.Instance.MidRange * Main.Instance.MidRange)
                {
                    if (item.hp <= 0) { continue; }

                    StopAllCoroutines();
                    StartCoroutine(MidAttact(randomDelay, attactShowing, this, item));
                    return;
                }
            }
        }


    }

    public virtual IEnumerator MidAttact (float delay, float last, Units me, Units them)
    {
        delay = Random.Range(0, delay);

        yield return new WaitForSeconds(delay);
        lineRenderer.SetPosition(0, me.transform.position);
        lineRenderer.SetPosition(1, them.transform.position);
        them.Hp --;

        yield return new WaitForSeconds(last);
        lineRenderer.SetPosition(0, me.transform.position);
        lineRenderer.SetPosition(1, me.transform.position);

        attactCoolTimeNext = midAttactCoolTime + Time.time;

    }

    public virtual IEnumerator MeleeAttact(float delay, float last, Units me, Units them)
    {
        delay = Random.Range(0, delay);

        yield return new WaitForSeconds(delay);
        lineRenderer.SetPosition(0, me.transform.position);
        lineRenderer.SetPosition(1, them.transform.position);
        them.Hp -=2 ;
        float push = 1;
        int n = them.Neighbour;
        if (n < 8)
        {
            push = (8 - n);
        }
         float dir = (MyGroup == Main.Instance.Red) ? -1f : 1f;
         them.transform.DOPunchPosition( dir*(them.transform.position - me.transform.position).normalized *push /10f , 1f, 1, 0f);
        

        yield return new WaitForSeconds(last);
        lineRenderer.SetPosition(0, me.transform.position);
        lineRenderer.SetPosition(1, me.transform.position);

        attactCoolTimeNext = meleeAttactCoolTime + Time.time;

    }



}

