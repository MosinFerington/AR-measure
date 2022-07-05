using System;
using System.Collections.Generic;
using UnityEngine;

public class Triangulator
{
    private System.Collections.Generic.List<Vector2> m_points = new System.Collections.Generic.List<Vector2>();

    public Triangulator(Vector2[] points)
    {
        this.m_points = new System.Collections.Generic.List<Vector2>(points);
    }

    public int[] Triangulate()
    {
        System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
        int count = this.m_points.Count;
        if (count < 3)
        {
            return list.ToArray();
        }
        int[] array = new int[count];
        if (this.Area() > 0f)
        {
            for (int i = 0; i < count; i++)
            {
                array[i] = i;
            }
        }
        else
        {
            for (int j = 0; j < count; j++)
            {
                array[j] = count - 1 - j;
            }
        }
        int k = count;
        int num = 2 * k;
        int num2 = k - 1;
        while (k > 2)
        {
            if (num-- <= 0)
            {
                return list.ToArray();
            }
            int num3 = num2;
            if (k <= num3)
            {
                num3 = 0;
            }
            num2 = num3 + 1;
            if (k <= num2)
            {
                num2 = 0;
            }
            int num4 = num2 + 1;
            if (k <= num4)
            {
                num4 = 0;
            }
            if (this.Snip(num3, num2, num4, k, array))
            {
                int item = array[num3];
                int item2 = array[num2];
                int item3 = array[num4];
                list.Add(item);
                list.Add(item2);
                list.Add(item3);
                int num5 = num2;
                for (int l = num2 + 1; l < k; l++)
                {
                    array[num5] = array[l];
                    num5++;
                }
                k--;
                num = 2 * k;
            }
        }
        list.Reverse();
        return list.ToArray();
    }

    private float Area()
    {
        int count = this.m_points.Count;
        float num = 0f;
        int index = count - 1;
        int i = 0;
        while (i < count)
        {
            Vector2 vector = this.m_points[index];
            Vector2 vector2 = this.m_points[i];
            num += vector.x * vector2.y - vector2.x * vector.y;
            index = i++;
        }
        return num * 0.5f;
    }

    private bool Snip(int u, int v, int w, int n, int[] V)
    {
        Vector2 vector = this.m_points[V[u]];
        Vector2 vector2 = this.m_points[V[v]];
        Vector2 vector3 = this.m_points[V[w]];
        if (Mathf.Epsilon > (vector2.x - vector.x) * (vector3.y - vector.y) - (vector2.y - vector.y) * (vector3.x - vector.x))
        {
            return false;
        }
        for (int i = 0; i < n; i++)
        {
            if (i != u && i != v && i != w)
            {
                Vector2 p = this.m_points[V[i]];
                if (this.InsideTriangle(vector, vector2, vector3, p))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
        float arg_B6_0 = C.x - B.x;
        float num = C.y - B.y;
        float num2 = A.x - C.x;
        float num3 = A.y - C.y;
        float num4 = B.x - A.x;
        float num5 = B.y - A.y;
        float num6 = P.x - A.x;
        float num7 = P.y - A.y;
        float num8 = P.x - B.x;
        float num9 = P.y - B.y;
        float num10 = P.x - C.x;
        float num11 = P.y - C.y;
        float arg_D8_0 = arg_B6_0 * num9 - num * num8;
        float num12 = num4 * num7 - num5 * num6;
        float num13 = num2 * num11 - num3 * num10;
        return arg_D8_0 >= 0f && num13 >= 0f && num12 >= 0f;
    }
}
