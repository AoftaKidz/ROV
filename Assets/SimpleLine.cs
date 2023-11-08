using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLine : MonoBehaviour
{
    public LineRenderer line;
    public GameObject tagNumber;
    public int number = 0;
    public float offsetX = -3007f;
    public float offsetY = 0;

    public void Create(int type)
    {
        int c = 0;
        number = type + 1;
        offsetY = 0;
        if (number == 1 || number == 2 || number == 3 || number == 10 || number == 11)
            offsetY = 0.0f;
        if (number == 4 || number == 7 || number == 9 || number == 12 || number == 14)
            offsetY = 0.6f;
        if (number == 5 || number == 6 || number == 8 || number == 13 || number == 15)
            offsetY = -0.6f;

        List<int> points = CreateLineByType(type+1);
        line.positionCount = points.Count + 2;
        int count = points.Count;

        for (int i = 0; i < count; i++)
        {
            Vector3 p = SlotMachineForUI.Instance.spines[points[i]].transform.position;
            p.z = 1;
            p.y += offsetY;

            if (c == 0)
            {
                //First
                Vector3 startP = new Vector3(p.x - 1.9f, p.y, p.z);
                line.SetPosition(c, startP);
                if (number < 10)
                    tagNumber.transform.position = startP;
            }

            c++;
            line.SetPosition(c, p);
    
            if (c == count)
            {
                //Last
                c++;
                Vector3 endP = new Vector3(p.x + 1.9f, p.y, p.z);
                line.SetPosition(c, endP);
                if (number >= 10)
                    tagNumber.transform.position = endP;
            }
        }

        LoadLineNumber();

    }
    List<int> CreateLineByType(int type)
    {
        //Type 1,2,...,15
        switch (type)
        {
            case 1:
                {
                    return new List<int>() { 1, 4, 7, 10, 13 };
                }
            case 2:
                {
                    return new List<int>() { 0, 3, 6, 9, 12 };
                }
            case 3:
                {
                    return new List<int>() { 2, 5, 8, 11, 14 };
                }
            case 4:
                {
                    return new List<int>() { 0, 4, 8, 10, 12 };
                }
            case 5:
                {
                    return new List<int>() { 2, 4, 6, 10, 14 };
                }
            case 6:
                {
                    return new List<int>() { 0, 3, 7, 9, 12 };
                }
            case 7:
                {
                    return new List<int>() { 2, 5, 7, 11, 14 };
                }
            case 8:
                {
                    return new List<int>() { 1, 5, 8, 11, 13 };
                }
            case 9:
                {
                    return new List<int>() { 1, 3, 6, 9, 13 };
                }
            case 10:
                {
                    return new List<int>() { 0, 4, 7, 10, 12 };
                }
            case 11:
                {
                    return new List<int>() { 2, 4, 7, 10, 14 };
                }
            case 12:
                {
                    return new List<int>() { 1, 4, 6, 10, 13 };
                }
            case 13:
                {
                    return new List<int>() { 1, 4, 8, 10, 13 };
                }
            case 14:
                {
                    return new List<int>() { 0, 5, 8, 11, 12 };
                }
            case 15:
                {
                    return new List<int>() { 2, 3, 6, 9, 14 };
                }
            default:
                {
                    return new List<int>() { 0 };
                }
        }
    }
    void LoadLineNumber()
    {
        string filename = "";
        if (number < 10)
            filename = "LineNUM_0" + number;
        else
            filename = "LineNUM_" + number;
        Sprite sp = Resources.Load<Sprite>("SlotMachine/line_number/" + filename);
        tagNumber.GetComponent<SpriteRenderer>().sprite = sp;
    }
}
