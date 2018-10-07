using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Groups
{
    public List<Units> AllUnits = new List<Units>();
    public List<Formations> Formations = new List<Formations>();
    public Transform Center;
    public List<int> forms = new List<int> { 0, 0, 0 };

    public float MeleePower = 1f;
    public float MidRangePower = 1f;

    public int MyMode = 0;

    public Groups()
    {
        GameObject gameObject = new GameObject();
        Center = gameObject.transform;
    }

    public void ModeUpdate()
    {
        ModeUpdate(MyMode);
    }

    public void MoveUnits()
    {

    }


    public void Combat()
    {
        foreach (var item in AllUnits)
        {
            item.Combat();
        }
    }

    public void ModeUpdate(int mode)
    {
        List<Units> temp = new List<Units>();
        Transform unitContainer = (this == Main.Instance.Blue) ? Main.Instance.BlueContainer : Main.Instance.RedContainer;

        foreach (var item in AllUnits)
        {
            item.gameObject.transform.SetParent(unitContainer);
            if (!item.Broken)
            {
                temp.Add(item);
            }
        }
        AllUnits = temp;

        for (int i = 0; i < Formations.Count; i++)
        {
            forms[i] = (int) Formations[i].MyForm;
            Formations[i].OnKill();
        }
        Formations.Clear();

        switch (mode)
        {
            case 0:
                Formations a = new Formations(Center);
                Formations.Add(a);

                foreach (var item in AllUnits)
                {
                    if(item.Broken) { continue; }
                    a.AllUnits.Add(item);
                }
                a.MyForm = (FormationTypes) forms[0];
                break;
            case 1:
                Formations b = new Formations(Center);
                Formations.Add(b);
                b.Center.localPosition += new Vector3(1.5f,0,0); // TEMP

                // TODO: Sorting 

                Formations c = new Formations(Center);
                Formations.Add(c);
                c.Center.localPosition += new Vector3(-1.5f, 0, 0); // TEMP

                b.MyForm = (FormationTypes)forms[0];
                c.MyForm = (FormationTypes)forms[0];

                for (int i = 0; i < AllUnits.Count ; i++)
                {
                    if (i%2 == 0)
                    {

                        b.AllUnits.Add( AllUnits[i] );
                    }
                    else
                    {

                        c.AllUnits.Add( AllUnits[i] );
                    }
                }
                break;
            case 2:
                Formations e = new Formations(Center);
                e.Center.localPosition += new Vector3(0, 0, 1.5f); // TEMP

                Formations f = new Formations(Center);
                f.Center.localPosition += new Vector3(1.5f, 0, 0); // TEMP

                Formations g = new Formations(Center);
                g.Center.localPosition += new Vector3(-1.5f, 0, 0); // TEMP

                Formations.Add(e);
                Formations.Add(f);
                Formations.Add(g);
                e.MyForm = (FormationTypes)forms[0];
                f.MyForm = (FormationTypes)forms[1];
                g.MyForm = (FormationTypes)forms[2];

                for (int i = 0; i < AllUnits.Count; i++)
                {
                    if (i % 4 == 0)
                    {

                        f.AllUnits.Add(AllUnits[i]);
                    }
                    else if (i%4 == 1)
                    {

                        g.AllUnits.Add(AllUnits[i]);
                    }
                    else
                    {

                        e.AllUnits.Add(AllUnits[i]);
                    }
                }
                break;
        }
    }

    public void UpdateForms()
    {
        foreach (var item in Formations)
        {
            item.UpdateForms();
        }
    }

    public void ToSquare()
    {
        foreach (var item in Formations)
        {
            item.ToSquare();
        }
    }

    public void ToTriangle()
    {
        foreach (var item in Formations)
        {
            item.ToTriangle();
        }
    }

    public void ToCircle()
    {
        foreach (var item in Formations)
        {
            item.ToCircle();
        }
    }

    public void ModeSwitch()
    {
        MyMode += 1;
        MyMode = MyMode % 3;

        ModeUpdate(MyMode);
        UpdateForms();
    }
}
