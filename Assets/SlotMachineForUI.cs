using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SlotMachineForUI : MonoBehaviour
{
    public static SlotMachineForUI Instance = null;
    public GameObject group;
    public List<GameObject> spines;
    public List<GameObject> wildTall;

    public GameObject prefabLine;
    List<GameObject> _lines = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Clear();
        //Drawline(new List<int>());
    }
    public void Show(List<int> data, List<List<int>> matches, List<int>lines)
    {
        Clear();
        group.SetActive(true);
        //Set data
        for(int i = 0; i < data.Count;i++)
        {
            bool isMatch = false;
            {
                //Find match
                foreach(var m in matches)
                {
                    foreach(var d in m)
                    {
                        if (d == data[i])
                        {
                            isMatch = true;
                            break;
                        }
                    }
                    if (isMatch)
                        break;
                }
            }
            if (isMatch)
                spines[i].GetComponent<SkeletonAnimation>().AnimationName = SlotMachine.Instance.GetSpineAnimationMatch(data[i]);
            else
                spines[i].GetComponent<SkeletonAnimation>().AnimationName = SlotMachine.Instance.GetSpineAnimationIdle(data[i]);
        }

        //Wild Tall
        if (data[0] == (int)SlotMachine.SlotMachineID.Puzzle_Wild ||
            data[1] == (int)SlotMachine.SlotMachineID.Puzzle_Wild ||
            data[2] == (int)SlotMachine.SlotMachineID.Puzzle_Wild)
            wildTall[0].SetActive(true);
        if (data[3] == (int)SlotMachine.SlotMachineID.Puzzle_Wild ||
            data[4] == (int)SlotMachine.SlotMachineID.Puzzle_Wild ||
            data[5] == (int)SlotMachine.SlotMachineID.Puzzle_Wild)
            wildTall[1].SetActive(true);
        if (data[6] == (int)SlotMachine.SlotMachineID.Puzzle_Wild ||
            data[7] == (int)SlotMachine.SlotMachineID.Puzzle_Wild ||
            data[8] == (int)SlotMachine.SlotMachineID.Puzzle_Wild)
            wildTall[2].SetActive(true);
        if (data[9] == (int)SlotMachine.SlotMachineID.Puzzle_Wild ||
            data[10] == (int)SlotMachine.SlotMachineID.Puzzle_Wild ||
            data[11] == (int)SlotMachine.SlotMachineID.Puzzle_Wild)
            wildTall[3].SetActive(true);
        if (data[12] == (int)SlotMachine.SlotMachineID.Puzzle_Wild ||
            data[13] == (int)SlotMachine.SlotMachineID.Puzzle_Wild ||
            data[14] == (int)SlotMachine.SlotMachineID.Puzzle_Wild)
            wildTall[4].SetActive(true);

        Drawline(lines);

    }
    void Drawline(List<int> lines)
    {
        //lines = new List<int>() { 1,10,15};

        foreach (int type in lines)
        {
            GameObject prefab = Instantiate(prefabLine, Vector3.zero, Quaternion.identity);
            prefab.GetComponent<SimpleLine>().Create(type);
            _lines.Add(prefab);
        }
    }
    
    public void Hide()
    {
        group.SetActive(false);
        Clear();
    }
    void Clear()
    {
        foreach (var w in wildTall)
        {
            w.SetActive(false);
        }
        foreach (var l in _lines)
        {
            Destroy(l);
        }
        _lines.Clear();
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
}
