using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrawLine : MonoBehaviour
{
    public TextMeshPro txtReward;
    public GameObject tagNumber;
    public int number = 0;
    public float reward = 0;
    LineRenderer _line;
    public void CreateLine(List<int> datas)
    {
        if (datas == null) return;
        if (datas.Count < 3) return;
        _line = GetComponent<LineRenderer>();

        SlotMachine slot = SlotMachine.Instance;
        int c = 0;
        int count = datas.Count;
        _line.positionCount = count + 2;
        float offsetY = 0;
        if (number == 1 || number == 2 || number == 3 || number == 10 || number == 11)
            offsetY = 0.0f;
        if (number == 4 || number == 7 || number == 9 || number == 12 || number == 14)
            offsetY = 0.85f;
        if (number == 5 || number == 6 || number == 8 || number == 13 || number == 15)
            offsetY = -0.85f;
        Vector3 rewardPos = Vector3.zero;

        foreach (int d in datas)
        {
            slot.SetActivePuzzle(d);
            Vector3 p = SlotMachine.activePuzzle.transform.position;
            p.y += offsetY;
            float offsetPos = 3.0f;
            float tagOffsetPos = 1.50f;
            if (c == 0)
            {
                //First
                Vector3 startP = new Vector3(p.x - offsetPos, p.y,p.z);
                _line.SetPosition(c, startP);
                if(number < 10)
                    tagNumber.transform.position = new Vector3(p.x - tagOffsetPos, p.y, p.z);
            } 

            c++;
            _line.SetPosition(c, p);
            if (c == 3)
                rewardPos = p;
            if (c == count)
            {
                //Last
                c++;
                Vector3 endP = new Vector3(p.x + offsetPos, p.y, p.z);
                _line.SetPosition(c, endP);
                if (number >= 10)
                    tagNumber.transform.position = new Vector3(p.x + tagOffsetPos, p.y, p.z); ;
            }
        }

        //Show reward text;
        var format = string.Format("{0:#,#.00}", reward);
        txtReward.text = format;//SpriteNumberManager.ToMeowWhite(format);
        rewardPos.y -= 0.8f;
        txtReward.transform.localPosition = rewardPos;
        ShowReward(false);
        LoadLineNumber();
    }
    void LoadLineNumber()
    {
        string filename = "";
        if(number < 10)
            filename = "LineNUM_0" + number;
        else
            filename = "LineNUM_" + number;
        Sprite sp = Resources.Load<Sprite>("SlotMachine/line_number/" + filename);
        tagNumber.GetComponent<SpriteRenderer>().sprite = sp;
    }
    public void ShowReward(bool isShow)
    {
        txtReward.gameObject.SetActive(isShow);
    }
}
