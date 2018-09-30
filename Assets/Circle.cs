using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle
{
    private static List<int> Temp = new List<int>();

    public static float OuterToRadius(int side)
    {
        if (side <= 1) { return 0; }
        return 1f / (2f * (Mathf.Sin(Mathf.PI / side)));
    }

    public static List<int> PosesToFomation(int count)
    {
        if (count <= 6)
        {
            return new List<int> { count };
        }

        if (count <= 10)
        {
            return new List<int> { count - 1, 1 };
        }

        List<int> result = new List<int>();

        for (int i = 1; i < count; i++)
        {
            int total = 0;
            result = OuterToFomation(i);
            foreach (var item in result)
            {
                total += item;
            }

            if (total >= count)
            {
                return result;
            }
        }

        Debug.Log("error " + count);
        return new List<int> { 0 };
    }

    private static int OuterToPose(int side)
    {
        if (side < 1)
        {
            return 0;
        }

        Temp.Add(side);
        int result = side;
        float radius = OuterToRadius(side);
        int polygon = side;

        for (int i = polygon; i >= 1; i--)
        {
            if (OuterToRadius(i) <= radius - 1f)
            {
                radius = OuterToRadius(i);
                result += OuterToPose(i);
                break;
            }
        }
        return result;
    }

    private static List<int> OuterToFomation(int count)
    {
        Temp = new List<int>();
        OuterToPose(count);
        List<int> result = Temp;
        return result;
    }
}
