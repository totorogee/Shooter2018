using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groups
{
    public List<Unit> AllUnits = new List<Unit>();
    public List<Formations> Formations = new List<Formations>();
    public Transform Center;

    public int MyMode = 0;

    public Groups()
    {
        GameObject gameObject = new GameObject();
        Center = gameObject.transform;
    }

    public void ModeSwitch(int mode)
    {
        foreach (var item in AllUnits)
        {
            item.gameObject.transform.SetParent(Main.Instance.unitContainer);
        }

        foreach (var item in Formations)
        {
            item.OnKill();
        }
        Formations.Clear();

        switch (mode)
        {
            case 0:
                Formations a = new Formations(Center);
                Formations.Add(a);

                foreach (var item in AllUnits)
                {
                    a.AllUnits.Add(item);
                }
                break;
            case 1:
                Formations b = new Formations(Center);
                Formations.Add(b);
                b.Center.localPosition += new Vector3(1f,0,0); // TEMP

                // TODO: Sorting 

                Formations c = new Formations(Center);
                Formations.Add(c);
                c.Center.localPosition += new Vector3(-1f, 0, 0); // TEMP

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
                e.Center.localPosition += new Vector3(0, 0, 1f); // TEMP

                Formations f = new Formations(Center);
                f.Center.localPosition += new Vector3(1f, 0, 0); // TEMP

                Formations g = new Formations(Center);
                g.Center.localPosition += new Vector3(-1f, 0, 0); // TEMP

                Formations.Add(e);
                Formations.Add(f);
                Formations.Add(g);

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
        ModeSwitch(MyMode);
    }

    public void ToTriangle()
    {
        foreach (var item in Formations)
        {
            item.ToTriangle();
        }
        ModeSwitch(MyMode);
    }

    public void ToCircle()
    {
        foreach (var item in Formations)
        {
            item.ToCircle();
        }
        ModeSwitch(MyMode);
    }

    public void ModeSwitch()
    {
        MyMode += 1;
        MyMode = MyMode % 3;

        ModeSwitch(MyMode);
        UpdateForms();
    }
}
