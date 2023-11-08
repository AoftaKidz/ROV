using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Calendar : MonoBehaviour
{
    public static Calendar Instance = null;
    public GameObject content;
    public TextMeshProUGUI txtHeader;
    int currentMonth = 9;
    int currentYear = 2023;
    int currentDay = 12;
    public List<Button> numbers;
    public List<GameObject> buttons;
    public delegate void CalendarCallback(string date); // declare delegate type
    CalendarCallback callback = null;

    List<string> alldays = new List<string>() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    List<string> allmonths = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    List<string> allmonthsTH = new List<string>() { "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม" };
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        DateTime date1 = new DateTime(2023, 09, 10, 6, 20, 40);
        DateTime currentDateTime = DateTime.Now;
        currentDay = currentDateTime.Day;
        currentMonth = currentDateTime.Month;
        currentYear = currentDateTime.Year;
        //var date = new DateTime();

        int name = currentDateTime.Year;//DateTimeFormatInfo.CurrentInfo.GetMonthName(1);

        //string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;
        // Debug.Log(DateTime.DaysInMonth(2019, 06));
        //Debug.Log(date1.DayOfWeek);
        //Debug.Log(currentDateTime.ToString("MMM"));
        UpdateCalendar();
        Hide();
    }
    public void PrevMonth()
    {
        currentMonth--;
        if(currentMonth < 1)
        {
            currentMonth = 12;
            currentYear--;
        }
        UpdateCalendar(false);
    }
    public void NextMonth()
    {
        currentMonth++;
        if (currentMonth > 12)
        {
            currentMonth = 1;
            currentYear++;
        }
        UpdateCalendar(false);
    }
    void UpdateCalendar(bool isShowHL = true)
    {
        //DateTime date = new DateTime(currentYear, currentMonth, currentDay, 1, 0, 0);
        DateTime startDate = new DateTime(currentYear, currentMonth, 1, 1, 0, 0);
        DateTime nowDate = DateTime.Now;

        string m = UserProfile.Instance.language == "th" ? allmonthsTH[currentMonth - 1] : allmonths[currentMonth - 1];
        txtHeader.text = m + " " + GetYear(UserProfile.Instance.language);
        string startDayInWeek = startDate.DayOfWeek.ToString();

        int i = 0;
        bool check = false;
        int dayInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
        foreach (Button button in numbers)
        {
            //CLear button
            ColorBlock cb = button.colors;
            {
                Color c = cb.normalColor;
                c.a = 0;
                cb.normalColor = c;
            }
            button.GetComponentInChildren<TextMeshProUGUI>().color = new Color(79f/255f,79/255f,79f/255f);
            button.gameObject.SetActive(false);

            if (check)
            {
                //numbers
                if(i < dayInMonth)
                {
                    i++;
                    button.gameObject.SetActive(true);
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "" + i;

                }
                else
                {
                    //button.GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
            }
            else
            {
                if(startDayInWeek == alldays[i])
                {
                    button.gameObject.SetActive(true);
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "1";
                    check = true;
                    i = 1;
                }
                else
                {
                    //text.text = "";
                    //button.GetComponentInChildren<TextMeshProUGUI>().text = "";
                    i++;
                }
            }

            if(currentDay == i && isShowHL)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                Color c = cb.normalColor;
                c.a = 1;
                cb.normalColor = c;
            }

            button.colors = cb;

        }

    }
    public void SelectedDay(Button btn)
    {
        currentDay = int.Parse(btn.GetComponentInChildren<TextMeshProUGUI>().text);
        Debug.Log(currentDay);
        int i = 0;

        foreach (Button button in numbers)
        {
            //CLear button
            ColorBlock cb = button.colors;
            {
                Color c = cb.normalColor;
                c.a = 0;
                cb.normalColor = c;
            }
            button.GetComponentInChildren<TextMeshProUGUI>().color = new Color(79f / 255f, 79 / 255f, 79f / 255f);

            if (button == btn)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                Color c = cb.normalColor;
                c.a = 1;
                cb.normalColor = c;
            }
            button.colors = cb;
        }

        Debug.Log (GetDay());
        if(callback != null)
        {
            callback(GetDay());
        }
    }
    int GetYear(string lan = "th")
    {
        if(lan == "th")
            return currentYear + 543;
        else
            return currentYear;
    }
    public string GetDay()
    {
        string day = "" + currentDay;
        string month = "" + (currentMonth < 10 ? "0" + currentMonth : currentMonth);
        string year = "" + GetYear("en");

        return day + "-" + month + "-"+ year;
    }
    public void Show(CalendarCallback callback = null)
    {
        UpdateCalendar();
        this.callback = callback;
        content.SetActive(true);
        content.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
        content.transform.DOScale(1, 0.6f).SetEase(Ease.OutElastic).OnComplete(() => {
            //Hide();
        });

    }
    public void Hide()
    {
        //content.transform.localScale = Vector3.zero;
        content.transform.DOScale(0, 0.3f).SetEase(Ease.OutQuint).OnComplete(() =>{ 
            content.SetActive(false);
            //Show();
        });
    }
}
