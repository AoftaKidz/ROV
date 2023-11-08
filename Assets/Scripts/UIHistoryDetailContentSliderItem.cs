using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class UIHistoryDetailContentSliderItem : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI TextWalletBetResult;
    [SerializeField] TextMeshProUGUI TextWalletBetResultSize;
    [SerializeField] TextMeshProUGUI TextDateTime;

    [SerializeField] TextMeshProUGUI TextNoData;
    [SerializeField] TextMeshProUGUI TextWalletBetData;
    [SerializeField] TextMeshProUGUI TextWalletBetBenefitData;
    [SerializeField] TextMeshProUGUI TextWalletBetResultData;
    [SerializeField] HistoryDetailSubContent subContent;
    [SerializeField] GameObject subContentEmpty;

    public static UIHistoryDetailContentSliderItem Instance = null;
    public HistoryModelCombo data = null;
    public HistoryModelCombo testDataSlider = null;
    public int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(data != null)
        {
            CreateSlotTableGrid();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        CreateSlotTableGrid();
    }

    public void UpdateSlotTable()
    {
        data = UIHistoryPopUpDetail.dataCombos[index];
        Debug.Log("combo id : " + data.id);
        List<int> datas = null;
        List<List<int>> matches = null;
        List<int> line = null;
        if (data.data != null)
        {
            datas = data.data;
        }
        if (data.matches != null)
        {
            matches = data.matches;
        }
        if (data.lines != null)
        {
            line = data.lines;
        }
        SlotMachineForUI.Instance.Show(datas, matches, line);
    }

    public void Hide()
    {
        SlotMachineForUI.Instance.Hide();
    }

    public void CreateSlotTableGrid()
    {
        data = UIHistoryPopUpDetail.dataCombos[index];
        
        List<int> datas = null;
        List<List<int>> matches = null;
        List<int> line = null;
        if (data.data != null)
        {
         datas = data.data;
        }
        if(data.matches != null)
        {
         matches = data.matches;
        }
        if (data.lines != null)
        {
            line = data.lines;
            if (data.lines.Count > 0)
            {
                subContentEmpty.SetActive(false);
                subContent.gameObject.SetActive(true);
                subContent.combo = data;
            }
            else
            {
                subContentEmpty.SetActive(true);
                subContent.gameObject.SetActive(false);
            }

        }
        else
        {
            subContentEmpty.SetActive(true);
            subContent.gameObject.SetActive(false);
        }

        Debug.Log("index : " + index);
        var betMultiplies = UIBetPopup.Instance.GetBetMultiplies(data.bet);
        if (betMultiplies[0] < 1)
            TextWalletBetResult.text = string.Format("{0:#,0.00}", betMultiplies[0]);
        else
            TextWalletBetResult.text = string.Format("{0:#,#.00}", betMultiplies[0]);
        TextWalletBetResultSize.text = string.Format("{0}", betMultiplies[1]);
        /*TextWalletBetResult.text = string.Format("{0:#,#.00}", data.bet);
        TextWalletBetResultSize.text = string.Format("{0}", data.maxCombo);*/
        TextDateTime.text = System.DateTime.Parse(data.created).ToString("hh:mm:ss dd/MM/yyyy");
        TextNoData.text = string.Format("{0}", data.id);
        TextWalletBetData.text = string.Format("{0:#,#.00}", data.bet);
        TextWalletBetBenefitData.text = data.reward == 0 ? "0.00" : string.Format("{0:#,#.00}", data.reward);
        TextWalletBetResultData.text = data.totalReward == 0 ? "0.00" : string.Format("{0:#,#.00}", data.totalReward);

        /*
            List<int> datas = new List<int>() { 0, 0, 1, 1, 2, 2, 5, 5, 6, 5, 9, 8, 7, 8, 7 };
            List<List<int>> matches = new List<List<int>>() { new List<int>() { 0, 2, 5, 6, 8, 9 } };
            List<int> line = new List<int>() { 0, 1, 2, 5, 6 };
        */

        SlotMachineForUI.Instance.Show(datas, matches, line);


    }
}
