using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HistoryData
{
    public string historyID;
    public string historyDateTime;
    public string historyNo;
    public string historyWallet;
    public string historyBenefit;
    public string historyStatus;
    public HistoryModelTransaction HistoryDataTransactions;
}

public class UIhistoryListItems : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI TextDateTime;
    [SerializeField] TextMeshProUGUI TextNo;
    [SerializeField] TextMeshProUGUI TextWallet;
    [SerializeField] TextMeshProUGUI TextBenetfit;

    [SerializeField] Color Color1;
    [SerializeField] Color Color2;

    [SerializeField] Color Color3;
    [SerializeField] Color Color4;

    [SerializeField] GameObject BG;

    public HistoryData data;

    // Start is called before the first frame update
    void Start()
    {
        if (data != null)
        {
            TextDateTime.text = data.historyDateTime;
            TextNo.text = data.historyNo;
            TextWallet.text = data.historyWallet;
            TextBenetfit.text = data.historyBenefit;
            if (data.historyStatus == "won")
            {
                TextBenetfit.GetComponent<TextMeshProUGUI>().color = Color3;
            }
            else
            {
                TextBenetfit.GetComponent<TextMeshProUGUI>().color = Color4;
            }
        }
    }
    public void SetBGColor(bool isHilight)
    {
        if (isHilight)
        {
            BG.GetComponent<Image>().color = Color2;
        }
        else
        {
            BG.GetComponent<Image>().color = Color1;
        }
    }

    public void OnClickHistoryDetail()
    {
        //return;
        UIHistoryPopUpDetail.Instance.data = data;
        UIHistoryPopUpDetail.Instance.Show();
        Debug.Log("Click cell history" + data.historyID);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
