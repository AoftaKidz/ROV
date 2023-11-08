using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UI.Dates;
using Newtonsoft.Json;
using System;
public class UIHistoryPopUp : MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] GameObject PreFabItem;
    [SerializeField] GameObject ScrollContent;
    [SerializeField] GameObject datePicker;
    [SerializeField] ScrollRect scrollView;

    [SerializeField] TextMeshProUGUI TextDayNow;
    [SerializeField] TextMeshProUGUI TextTotalList;
    [SerializeField] TextMeshProUGUI TextTotalWallet;
    [SerializeField] TextMeshProUGUI TextTotalBenefit;

    public static UIHistoryPopUp Instance = null;
    bool _isShowDatePicker = false;
    bool _isShowHistoryPopUp = false;
    float scrollTemp;
    int page = 0;
    int size = 15;
    int totalTransaction = 0;
    bool loadApiStatus = false;
    string dateTimeSelect = "";
    public List<HistoryData> ListDatas = new List<HistoryData>();
    public List<GameObject> items = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        //dateTimeSelect = Calendar.Instance.GetDay();//System.DateTime.Now.ToString("dd-MM-yyyy");
       // DatePickerConfig();
    }

    void GetApiHistory()
    {
        //var startDate = dateTimeSelect;
        var startDate = PrevDate(dateTimeSelect, 7);
        StartCoroutine(ServiceManager.Instance.GetHistory(UserProfile.Instance.token, startDate, dateTimeSelect, 1, size, GetHistorySuccess, GetHIstoryFail));
    }

    void LoadMoreHistory()
    {
        loadApiStatus = true;
        page += 1;
        UILoading.Instance.Show();
        GetApiHistory();
    }

    void GetHistorySuccess(string result)
    {
        UILoading.Instance.Hide();
        loadApiStatus = false;
        var t = result.Split("=");
        var historyModel = JsonConvert.DeserializeObject<HistoryModel>(result);
        totalTransaction = historyModel.data.total;
        addDataHistory(historyModel);
    }
    void GetHIstoryFail(string result)
    {
        UILoading.Instance.Hide();
        loadApiStatus = false;
        Debug.Log(result);

    }
    void DestroyObject()
    {
        for (var i = ScrollContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(ScrollContent.transform.GetChild(i).gameObject);
        }
        ListDatas.Clear();
    }
    public void addDataHistory(HistoryModel model)
    {

        int totalBet = 0;
        double totalReward = 0;
        int count = 0;
        foreach (HistoryModelTransaction historyItem in model.data.transactions)
        {
            int betTotal = 0;
            double rewardTotal = 0;
            string dateCreated = "";
            float _bet = 0;
            foreach (HistoryModelCombo comboItem in historyItem.combo)
            {
                dateCreated = comboItem.created;
                betTotal += comboItem.bet;
                totalBet += comboItem.bet;
                rewardTotal += comboItem.reward;
                totalReward += comboItem.totalReward;
                _bet = comboItem.bet;
                Debug.Log("comboItem = " + comboItem.id + "bet = " + betTotal);
            }
            HistoryData dataItem = new HistoryData();
            dataItem.HistoryDataTransactions = historyItem;
            dataItem.historyID = string.Format("{0}", historyItem._id);
            dataItem.historyDateTime = System.DateTime.Parse(dateCreated).ToString("hh:mm:ss dd/MM/yyyy");
            dataItem.historyNo = string.Format("{0}", historyItem._id);
            dataItem.historyWallet = string.Format("{0:#,#.00}", _bet);
            if (rewardTotal > 0)
            {
                dataItem.historyStatus = "won";
                dataItem.historyBenefit = "+" + string.Format("{0:#,#.00}", rewardTotal);
            }
            else
            {
                dataItem.historyStatus = "loss";
                dataItem.historyBenefit = "-" + string.Format("{0:#,#.00}", betTotal);
            }
            /*if (i % 2 == 0) {
                dataItem.historyStatus = "won";
                dataItem.historyWallet = "6.00";
                dataItem.historyBenefit = "+114.00";
            } else {
                dataItem.historyStatus = "loss";
                dataItem.historyWallet = "10.00";
                dataItem.historyBenefit = "-10.00";
            }*/
            ListDatas.Add(dataItem);
        }

        TextTotalWallet.text = "฿" + string.Format("{0:#,#0.00}", totalBet);
        TextTotalBenefit.text = "฿" + string.Format("{0:#,#0.00}", totalReward);
        if (totalTransaction == 0 || totalTransaction == null)
        {
            totalTransaction = 0;
        }

        TextTotalList.text = string.Format("{0:#,#} บันทึก", totalTransaction);
        count = items.Count;
        foreach (var ItemListData in ListDatas)
        {
            count++;
            GameObject obj = Instantiate(PreFabItem, new Vector3(0, 0, 0), Quaternion.identity);
            UIhistoryListItems item = obj.GetComponent<UIhistoryListItems>();
            item.data = ItemListData;
            if (count % 2 == 0)
                item.SetBGColor(true);
            else
                item.SetBGColor(false);
            obj.transform.SetParent(ScrollContent.transform, false);
            //obj.transform.parent = ScrollContent.transform;
            //obj.transform.localScale = Vector3.one;
            //go.transform.localScale = new Vector3 (cardScale, cardScale, cardScale);
            items.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isShowHistoryPopUp)
        {
            float scrollY = scrollView.verticalNormalizedPosition;
            if (scrollY < -0.2 && scrollTemp != scrollY && !loadApiStatus)
            {
                if (page == 0 && 1 * size < totalTransaction)
                {
                    LoadMoreHistory();
                }
                else if (page > 0 && page * size < totalTransaction)
                {
                    LoadMoreHistory();
                }
                scrollTemp = scrollY;
                Debug.Log("temp = " + scrollTemp + "|| Y = " + scrollY);
                //Debug.Log("ScrollY = " + string.Format("{0}", scrollY) + "ScrollTemp = " + string.Format("{0}", scrollTemp));
            }
        }
    }

    public void OnClose()
    {
        Hide();
    }

    public void Show()
    {
        Puzzle.isEnableClick = false;
        _isShowHistoryPopUp = true;
        content.SetActive(true);
        content.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutQuint).OnComplete(() => {
            DestroyObject();
            dateTimeSelect = Calendar.Instance.GetDay();
            GetApiHistory();
            Debug.Log("Show History = " + dateTimeSelect);
        });
    }

    public void Hide()
    {
        SoundManager.Instance.PlaySFX("Close");
        _isShowHistoryPopUp = false;
        Puzzle.isEnableClick = true;
        content.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            content.SetActive(false);

        });
    }
    void DatePickerConfig()
    {
/*        DatePicker picker = datePicker.transform.Find("DatePicker").gameObject.GetComponent<DatePicker>();
        picker.Config.Header.ShowNextAndPreviousMonthButtons = true;
        picker.Config.Header.ShowNextAndPreviousYearButtons = false;
        picker.Config.WeekDays.ShowWeekNumbers = false;
        picker.Config.Misc.ShowDatesInOtherMonths = false;
        picker.DateSelectionMode = DateSelectionMode.SingleDate;
        picker.UpdateDisplay();*/
        //dateTimeSelect = picker.SelectedDate.ToString().Substring(0, 10).Replace('/', '-');
        //Debug.Log("DatePickerConfig : " + picker);
    }

    public void OnOpenDatePicker()
    {
        _isShowDatePicker = !_isShowDatePicker;
        datePicker.SetActive(_isShowDatePicker);
        if(_isShowDatePicker)
            Calendar.Instance.Show(OnCalendarCallback);
        else
            Calendar.Instance.Hide();
    }

    void OnCalendarCallback(string date)
    {
        dateTimeSelect = date;
        OnOpenDatePicker();
        OnSelectedDatePicker();
        Debug.Log(date);
    }

    public void OnSelectedDatePicker()
    {
/*        _isShowDatePicker = !_isShowDatePicker;
        datePicker.SetActive(_isShowDatePicker);*/

        //"6/6/2023 12:00:00 AM"

       /* DatePicker picker = datePicker.transform.Find("DatePicker").gameObject.GetComponent<DatePicker>();
        var _d = picker.SelectedDate.ToString().Split(" ");
        var _dd = _d[0].Split("/");
        dateTimeSelect = _dd[1] + "-" + _dd[0] + "-" + _dd[2];
        string showTextDate = "วันนี้";
        if (dateTimeSelect != System.DateTime.Now.ToString("dd-MM-yyyy"))
        {
            showTextDate = dateTimeSelect;
        }
        TextDayNow.text = showTextDate;
        Debug.Log("OnSelectedDatePicker : " + dateTimeSelect);
        page = 0;*/
        //PrevDate(dateTimeSelect, 1);
        var d1 = Calendar.Instance.GetDay();
        var d2 = DateTime.Now.ToString("dd-MM-yyyy");
        var l1 = d2.Split('-');
        int y = int.Parse(l1[2]) > 2500 ? int.Parse(l1[2])-543 : int.Parse(l1[2]);
        var d3 = l1[0] + "-" + l1[1] + "-" + y;
        if(d1 == d3)
        {
            TextDayNow.text = "วันนี้";

        }
        else
        {
            TextDayNow.text = d1;
        }
        DestroyObject();
        GetApiHistory();

    }
    public string PrevDate(string date, int day)
    {

        var _d = date.Split("-");

        DateTime start = new DateTime(int.Parse(_d[2]), int.Parse(_d[1]), int.Parse(_d[0]));
        DateTime prev = start.AddDays(-day);
        Debug.Log("PrevDate : " + prev);

        var _dd = prev.ToString().Split(" ");
        var _ddd = _dd[0].Split("/");
        int y = int.Parse(_ddd[2]);
        if (y > 2500)
            y -= 543;
        return _ddd[1] + "-" + _ddd[0] + "-" + y;
    }
}
