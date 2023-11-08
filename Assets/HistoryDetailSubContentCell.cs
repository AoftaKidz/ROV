using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;
using Unity.Mathematics;

public class  HistoryDetailSubCombo
{
    public List<int> data;
    public List<int> match;
    public int lineID;
    public double reward;
    public double subReward;
}
public class HistoryDetailSubContentCell : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtTitle;
    [SerializeField] TextMeshProUGUI txtSubTitle;
    [SerializeField] TextMeshProUGUI txtReward;
    [SerializeField] TextMeshProUGUI txtSubReward;
    [SerializeField] SkeletonGraphic spine;
    public HistoryDetailSubCombo combo = null;

    // Start is called before the first frame update
    void Start()
    {
        CreateData();
    }
    void CreateData()
    {
        if (combo == null) return;

        {
            int _id = 0;
            for (int i = 0; i < combo.match.Count; i++){
                //if (combo.data[combo.match[i]] != (int)SlotMachine.SlotMachineID.Puzzle_Wild )
                if(CheckWildColumn(combo.match[i],combo.data) == false)
                {
                    _id = combo.data[combo.match[i]];
                    break;
                }
            }
            spine.AnimationState.SetAnimation(0, SlotMachine.Instance.GetSpineAnimationIdle(_id), true);
        }
        {
            int c = combo.match.Count;
            txtTitle.text = c + " สัญลักษณ์เหมือนกัน";
        }
        {
            string c = combo.lineID < 9 ? "0"+(combo.lineID+1) : (combo.lineID + 1) + "";
            txtSubTitle.text = "เส้นที่ชนะ " + c;
        }
        {
            var formatedWallet = combo.reward == 0 ? "0.00" : string.Format("{0:#,#.00}", combo.reward);

            txtReward.text = "฿" + formatedWallet;
        }
        {
            txtSubReward.text = "";
        }
    }
    bool CheckWildColumn(int index,List<int> data)
    {
        int c =Mathf.FloorToInt( index / 3);
        if(c == 0)
        {
            //Column 1
            return data[0] == 11 || data[1] == 11 || data[2] == 11;
        }
        if (c == 0)
        {
            //Column 2
            return data[3] == 11 || data[4] == 11 || data[5] == 11;
        }
        if (c == 0)
        {
            //Column 3
            return data[6] == 11 || data[7] == 11 || data[8] == 11;
        }
        if (c == 0)
        {
            //Column 4
            return data[9] == 11 || data[10] == 11 || data[11] == 11;
        }
        if (c == 0)
        {
            //Column 5
            return data[12] == 11 || data[13] == 11 || data[14] == 11;
        }
        return false;
    }
}
