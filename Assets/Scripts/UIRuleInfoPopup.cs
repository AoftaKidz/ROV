using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIRuleInfoPopup : MonoBehaviour
{
    [SerializeField] GameObject content;
    [SerializeField] GameObject scrollContent;
    public static UIRuleInfoPopup Instance = null;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        //StartCoroutine(UpdateRect());
        Hide();
    }
    public void OnClose()
    {
        Hide();
    }
    public IEnumerator UpdateRect()
    {
        scrollContent.GetComponent<VerticalLayoutGroup>().enabled = false;
        yield return new WaitForSeconds(0.1F);
        scrollContent.GetComponent<VerticalLayoutGroup>().enabled = true;
    }

    void RefreshScroll()
    {
        /*scrollContent.GetComponent<VerticalLayoutGroup>().enabled = false;
        scrollContent.GetComponent<VerticalLayoutGroup>().enabled = true;*/
        //Canvas.ForceUpdateCanvases();
        //StartCoroutine(UpdateRect());
    }
    public void Show()
    {
        RefreshScroll();
        Puzzle.isEnableClick = false;
        content.SetActive(true);
        content.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutQuint);
    }
    public void Hide()
    {
        //SoundManager.Instance.PlaySFX("Close");

        Puzzle.isEnableClick = true;
        content.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            scrollContent.GetComponent<VerticalLayoutGroup>().spacing = 0;
            content.SetActive(false);

        });
    }
}
