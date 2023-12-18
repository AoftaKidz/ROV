using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class UIKindOfMeowPopup : MonoBehaviour
{
    public static UIKindOfMeowPopup Instance = null;
    [SerializeField] GameObject group;
    [SerializeField] GameObject content;
    [SerializeField] SkeletonGraphic spine;
    [SerializeField] float delay = 5;

    float time = 0;
    bool isStart = false;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        //Show();
    }
    void Update()
    {
        if (!isStart) return;
        time += Time.deltaTime;
        if(time > delay)
        {
            Hide();
            //Show();
          
        }
    }
    public void Show()
    {
        if (!Condition()) return;
 
        isStart = true;
        content.SetActive(true);


        spine.gameObject.SetActive(true);
        SoundManager.Instance.PlaySFX("AllWin");
        spine.AnimationState.SetAnimation(0, "5Kind_Idle", false);
        spine.transform.localScale = Vector3.one / 2f;
        spine.transform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutElastic);
    }
    public void Hide()
    {
        time = 0;
        isStart = false;
        content.SetActive(false);

        if (UIBigWinPopup.Instance.Condition())
            UIBigWinPopup.Instance.Show();
        else
        {
            //Check for Free spin mode
            SlotMachine slot = SlotMachine.Instance;
            if (slot.slotData.isScatterMode == false && slot.slotData.comingFreeSpinCount > 0)
            {
                //Starting scatter mode
                UIFreeSpinPopup.Instance.Show(slot.slotData.scatterCount);
            }
            else if (slot.slotData.isScatterMode)
            {
                //Update round reward
                UIRoundRewardPopup.Instance.Show(0.3f);
            }
            else if (SlotMachine.isAutoMode)
            {
                SlotMachineAutoSpin.Instance.AutoSpin();
            }
        }
    }
    public bool Condition()
    {
        if (SlotMachine.Instance == null) return false;
        if (SlotMachine.Instance.slotData == null) return false;
        if (SlotMachine.Instance.slotData.reward == 0) return false;
        if (SlotMachine.Instance.slotData.winRatio == 0) return false;

        if (SlotMachine.Instance.slotData.isFiveOfKind)
            return true;
        return false;
    }
    public bool Appear()
    {
        return isStart;
    }
    string GetKindName()
    {
        int foundData = -1;
        for(int i = 0; i < 9; i++)
        {
            List<bool> columnCheck = new List<bool> { false, false, false, false, false };
            for (int c = 0; c < 5; c++)
            {
                //Column
                if (CheckWild(c))
                {
                    columnCheck[c] = true;
                }
                else
                {
                    for(int r = 0;r < 3; r++)
                    {
                        if(GetData(c,r) == i)
                            columnCheck[c] = true;
                    }
                }
            }
            if (columnCheck[0] && columnCheck[1] && columnCheck[2] && columnCheck[3] && columnCheck[4])
            {
                foundData = i;
                break;
            }
        }

        if (foundData >= 0)
        {
/*            if (foundData == (int)SlotMachine.SlotMachineID.Puzzle_9)
                return "5_of_Kind_9";
            if (foundData == (int)SlotMachine.SlotMachineID.Puzzle_10)
                return "5_of_Kind_10";
            if (foundData == (int)SlotMachine.SlotMachineID.Puzzle_A)
                return "5_of_Kind_A";
            if (foundData == (int)SlotMachine.SlotMachineID.Puzzle_J)
                return "5_of_Kind_J";
            if (foundData == (int)SlotMachine.SlotMachineID.Puzzle_K)
                return "5_of_Kind_K";
            if (foundData == (int)SlotMachine.SlotMachineID.Puzzle_Cat01)
                return "5_of_Kind_Meow_1";
            if (foundData == (int)SlotMachine.SlotMachineID.Puzzle_Cat02)
                return "5_of_Kind_Meow_2";
            if (foundData == (int)SlotMachine.SlotMachineID.Puzzle_Cat03)
                return "5_of_Kind_Meow_3";
            if (foundData == (int)SlotMachine.SlotMachineID.Puzzle_Cat04)
                return "5_of_Kind_Meow_4";
            if (foundData == (int)SlotMachine.SlotMachineID.Puzzle_Cat05)
                return "5_of_Kind_Meow_5";
            if (foundData == (int)SlotMachine.SlotMachineID.Puzzle_Q)
                return "5_of_Kind_Q";*/
        }

        return "";
    }
    bool CheckWild(int column)
    {
        int c = column * 3;
        return SlotMachine.Instance.slotData.data[c] == 11 || SlotMachine.Instance.slotData.data[c + 1] == 11 || SlotMachine.Instance.slotData.data[c+2] == 11;
    }
    int GetData(int column,int row)
    {
        int c = column * 3 + row;
        return SlotMachine.Instance.slotData.data[c];
    }
}
