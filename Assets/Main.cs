using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeightedVacancy
{
    public Unit Unit;
    public Vector3 Position;
    public float Weight;
}

public class Formation
{
    public List<WeightedVacancy> Poses = new List<WeightedVacancy>();
}

public class Main : MonoBehaviour
{

    [SerializeField] protected Unit unitPrefab;
    [SerializeField] protected Transform unitContainer;
    [SerializeField] protected float boundary;
    [SerializeField] protected float spacing = 0.2f;


    public void RandomSpwan()
    {
        Vector3 randomPos = new Vector3(Random.Range(-1f, 1f), unitContainer.position.y, Random.Range(-1f, 1f));
        Instantiate(unitPrefab.gameObject, randomPos * boundary, Quaternion.identity, unitContainer);
    }

    public void Square()
    {
        int count = Unit.AllUnits.Count;
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

        Formation formation = new Formation();
        Vector3 offset = new Vector3(spacing * row / 2f, 0, spacing * row / 2f);

        for (int i = 0; i < row * row; i++)
        {
            WeightedVacancy vacancy = new WeightedVacancy();
            vacancy.Position = new Vector3(i % row, 0f, i / row) * spacing - offset;
            vacancy.Weight = Mathf.Max(i % row, i / row);
            if (i % row == i / row)
            {
                vacancy.Weight += 0.5f;
            }

            formation.Poses.Add(vacancy);
        }

        formation.Poses.Sort((x, y) => x.Weight.CompareTo(y.Weight));

        for (int i = 0; i < count; i++)
        {
            Unit.AllUnits[i].transform.DOLocalMove(formation.Poses[i].Position, 1f);
        }
    }

    public void Triangle()
    {
        int count = Unit.AllUnits.Count;
        if (count <= 0)
        {
            return;
        }

        int row = 0;
        int poses = 0;
        while (true)
        {
            poses = poses + row;
            if(count <= poses) { break; }
            row++;
        }

        Formation formation = new Formation();
        Vector3 offset = new Vector3(0, 0, spacing * row / 2f);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                WeightedVacancy vacancy = new WeightedVacancy();
                vacancy.Position = new Vector3( ((i/2f) -j) , 0f, i) * spacing - offset;
                vacancy.Weight = i;

                formation.Poses.Add(vacancy);
            }
        }

        formation.Poses.Sort((x, y) => x.Weight.CompareTo(y.Weight));

        for (int i = 0; i < count; i++)
        {
            Unit.AllUnits[i].transform.DOLocalMove(formation.Poses[i].Position, 1f);
        }

    }

    public void Circle()
    {
        int count = Unit.AllUnits.Count;
        if (count <= 0)
        {
            return;
        }

        int row = 0;
        int poses = 1;
        while (true)
        {
            if (count <= poses) { break; }
            row++;
            poses += Mathf.FloorToInt(2f * Mathf.PI * row);
        }

        Formation formation = new Formation();

        WeightedVacancy vacancy = new WeightedVacancy();
        vacancy.Position = new Vector3(0f, 0f, 0f);
        vacancy.Weight = 0;
        formation.Poses.Add(vacancy);

        for (int i = 1; i <= row; i++)
        {
            float start = Random.Range(0f, 120f);
            for (int j = 0; j < Mathf.FloorToInt(2f * Mathf.PI * i); j++)
            {
                float angle = start + j * 360 / Mathf.FloorToInt(2f * Mathf.PI * i);
                vacancy = new WeightedVacancy();
                vacancy.Position = new Vector3( Mathf.Sin( Mathf.Deg2Rad * angle)*i , 0f, Mathf.Cos(Mathf.Deg2Rad * angle) *i) * spacing;
                vacancy.Weight = i;

                formation.Poses.Add(vacancy);
            }
        }

        formation.Poses.Sort((x, y) => x.Weight.CompareTo(y.Weight));

        for (int i = 0; i < count; i++)
        {
            Unit.AllUnits[i].transform.DOLocalMove(formation.Poses[i].Position, 1f);
        }

    }

}