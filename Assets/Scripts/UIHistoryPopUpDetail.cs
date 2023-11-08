using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using DG.Tweening;
using DanielLochner.Assets.SimpleScrollSnap;

public class UIHistoryPopUpDetail : MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] GameObject PreFabItem;
    [SerializeField] GameObject ScrollContent;
    [SerializeField] private SimpleScrollSnap scrollSnap;

    public static UIHistoryPopUpDetail Instance = null;
    public HistoryData data = null;
    //public List<HistoryModelCombo> ComboList = new List<HistoryModelCombo>();
    public List<GameObject> ListSliderCombo = new List<GameObject>();
    public static List<HistoryModelCombo> dataCombos = new List<HistoryModelCombo>();
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClose()
    {
        Hide();
    }

    public void updateSliderData()
    {
        foreach (Transform t in scrollSnap.Content)
        {
            Debug.Log(t.gameObject.name);
            if (t.gameObject.GetComponent<UIHistoryDetailContentSliderItem>().index == scrollSnap.CenteredPanel)
            {
                t.gameObject.GetComponent<UIHistoryDetailContentSliderItem>().UpdateSlotTable();
                return;
            }
        }

    }

    void createSlider()
    {
        Debug.Log("createSlider = " + data);
        /*
         foreach (HistoryModelCombo item in data.HistoryDataTransactions.combo)
         {
             ListSliderCombo.Add(item);
         }
        */
        /*
          int indexSlider = 0;
          foreach(HistoryModelCombo sliderItem in data.HistoryDataTransactions.combo)
          {

              GameObject obj = Instantiate(PreFabItem, new Vector3(0, 0, 0), Quaternion.identity);
              obj.GetComponent<UIHistoryDetailContentSliderItem>().data = sliderItem;
              obj.GetComponent<UIHistoryDetailContentSliderItem>().index = indexSlider;
              obj.name = "item-" + indexSlider;
              scrollSnap.Add(obj, indexSlider);
              ListSliderCombo.Add(obj);
              if(indexSlider == 0)
              {
                  obj.GetComponent<UIHistoryDetailContentSliderItem>().UpdateSlotTable();
              }

              indexSlider++;
          }
        */

        int count = dataCombos.Count;
        for (int i = 0; i < count; i++)
        {
            scrollSnap.Remove(0);
            dataCombos.RemoveAt(0);
        }

        int indexSlider = 0;
        foreach (HistoryModelCombo sliderItem in data.HistoryDataTransactions.combo)
        {
            sliderItem.maxCombo = data.HistoryDataTransactions.maxCombo;
            dataCombos.Add(sliderItem);
            GameObject obj = Instantiate(PreFabItem, new Vector3(0, 0, 0), Quaternion.identity);
            obj.GetComponent<UIHistoryDetailContentSliderItem>().index = indexSlider;
            obj.name = "UIHistoryDetailContentSliderItem-" + indexSlider;

            scrollSnap.AddToBack(obj);
            if (indexSlider == 0)
            {
                obj.GetComponent<UIHistoryDetailContentSliderItem>().UpdateSlotTable();
            }
            Destroy(obj);
            indexSlider++;
        }

        scrollSnap.GoToPanel(0);
    }

    void DestroyObject()
    {
        int count = dataCombos.Count;
        for (int i = 0; i < count; i++)
        {
            scrollSnap.Remove(0);
            dataCombos.RemoveAt(0);
        }
    }

    public void Show()
    {
        Puzzle.isEnableClick = false;
        content.SetActive(true);
        content.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutQuint).OnComplete(() => {
            createSlider();
        });
    }

    public void Hide()
    {
        Puzzle.isEnableClick = true;
        content.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            //DestroyObject();
            UIHistoryDetailContentSliderItem.Instance.Hide();
            UIHistoryPopUp.Instance.Show();
            content.SetActive(false);
        });
    }
}
