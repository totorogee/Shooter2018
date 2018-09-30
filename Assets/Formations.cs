using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Forms
{
    Square = 0,
    Triangle = 1,
    Circle = 2
}

public class WeightedVacancy
{
    public Unit Unit;
    public Vector3 Position;
    public float Weight;
}

public class Formations
{
    public List<Unit> AllUnits = new List<Unit>();
    public List<WeightedVacancy> Poses = new List<WeightedVacancy>();
    public Transform Center;
    public Forms MyForm;

    public Formations(Transform GroupTransform)
    {
        GameObject gameObject = new GameObject();
        Center = gameObject.transform;
        Center.SetParent(GroupTransform);
        Center.localPosition = new Vector3(0f, 0f, 0f);
        Center.localEulerAngles = new Vector3(0f, 0f, 0f);
    }

    public void UpdateForms()
    {
        switch (MyForm)
        {
            case Forms.Square:
                ToSquare();
                break;
            case Forms.Triangle:
                ToTriangle();
                break;
            case Forms.Circle:
                ToCircle();
                break;
            default:
                break;
        }
    }

    public void ToSquare()
    {
        Debug.Log("here3");
        Debug.Log(MyForm.ToString());
        MyForm = Forms.Square;
        Debug.Log(MyForm.ToString());

        ToSquare(AllUnits);
    }

    public void ToSquare( List<Unit> Units)
    {
        Debug.Log("here");

        MyForm = Forms.Square;
        int count = Units.Count;
        if (count <= 0)
        {
            return;
        }

        int row = 0;
        while (true)
        {
            if (count <= row * row) { break; }
            row++;
        }

        Vector3 offset = new Vector3(GameSettings.Spacing * row / 2f, 0, GameSettings.Spacing * row / 2f);

        for (int i = 0; i < row * row; i++)
        {
            WeightedVacancy vacancy = new WeightedVacancy();
            vacancy.Position = new Vector3(i % row, 0f, i / row) * GameSettings.Spacing - offset;
            vacancy.Weight = Mathf.Max(i % row, i / row);
            if (i % row == i / row)
            {
                vacancy.Weight += 0.5f;
            }

            Poses.Add(vacancy);
        }

        Poses.Sort((x, y) => x.Weight.CompareTo(y.Weight));

        for (int i = 0; i < count; i++)
        {
            Units[i].transform.SetParent(Center);
            Units[i].transform.DOLocalMove(Poses[i].Position, 1f);
        }
    }

    public void ToTriangle()
    {
        Debug.Log(MyForm.ToString());
        MyForm = Forms.Triangle;
        Debug.Log(MyForm.ToString());
        ToTriangle(AllUnits);
    }

    public void ToTriangle(List<Unit> Units)
    {
        MyForm = Forms.Triangle;
        int count = Units.Count;
        if (count <= 0)
        {
            return;
        }

        int row = 0;
        int poses = 0;
        while (true)
        {
            poses = poses + row;
            if (count <= poses) { break; }
            row++;
        }

        Vector3 offset = new Vector3(0, 0, GameSettings.Spacing * row / 2f);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                WeightedVacancy vacancy = new WeightedVacancy();
                vacancy.Position = new Vector3(((i / 2f) - j), 0f, i) * GameSettings.Spacing - offset;
                vacancy.Weight = i;

                Poses.Add(vacancy);
            }
        }

        Poses.Sort((x, y) => x.Weight.CompareTo(y.Weight));

        for (int i = 0; i < count; i++)
        {
            Units[i].transform.SetParent(Center);
            Units[i].transform.DOLocalMove(Poses[i].Position, 1f);
        }

    }

    public void ToCircle()
    {
        MyForm = Forms.Circle;
        ToCircle(AllUnits);
    }

    public void ToCircle(List<Unit> Units)
    {
        MyForm = Forms.Circle;
        Debug.Log("here2");

        int count = Units.Count;
        if (count <= 0) { return; }

        List<int> numberInShells = Circle.PosesToFomation(count);

        int poses = count;

        for (int i = 0; i < numberInShells.Count; i++)
        {

            int outer = Mathf.Min(numberInShells[i], poses);

            for (int j = 0; j < numberInShells[i]; j++)
            {
                float angle = j * 360f / outer;

                WeightedVacancy vacancy = new WeightedVacancy();
                vacancy.Position = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0f, Mathf.Cos(Mathf.Deg2Rad * angle));
                vacancy.Position = vacancy.Position * GameSettings.Spacing * Circle.OuterToRadius(numberInShells[i]);

                if (outer == 1)
                {
                    vacancy.Position = Vector3.zero;
                }

                vacancy.Weight = i + 0.001f * j;

                Poses.Add(vacancy);
                poses--;

                if (poses < 0) { break; }
            }
        }

        Poses.Sort((x, y) => x.Weight.CompareTo(y.Weight));

        for (int i = 0; i < count; i++)
        {
            Units[i].transform.SetParent(Center);
            Units[i].transform.DOLocalMove(Poses[i].Position, 1f);
        }
    }

    public void OnKill()
    {
        GameObject.Destroy(Center.gameObject);
    }

}
