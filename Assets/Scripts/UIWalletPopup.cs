using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIWalletPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtWallet;
    [SerializeField] GameObject group;
    [SerializeField] GameObject fade;
    [SerializeField] GameObject content;
    public static UIWalletPopup Instance = null;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        CallUpdateWallet();
    }
    private void OnEnable()
    {
        UserProfile.OnUpdateUserProfile += UpdateWallet;
    }
    private void OnDisable()
    {
        UserProfile.OnUpdateUserProfile -= UpdateWallet;
    }
    void CallUpdateWallet()
    {
        UserProfile.Instance.CallUpdateUserProfile();
    }
    public void UpdateWallet(double wallet)
    {
        var formatedWallet = string.Format("{0:#,#.00}", wallet);
        txtWallet.text = formatedWallet;
    }
    public void OnClose()
    {
        Hide();
    }
    public void Show()
    {
        content.SetActive(true);
        Puzzle.isEnableClick = false;
        fade.SetActive(true);
        content.transform.DOLocalMoveY(1117, 0.5f).SetEase(Ease.OutQuint);
        CallUpdateWallet();
    }
    public void Hide()
    {
        SoundManager.Instance.PlaySFX("Close");

        Puzzle.isEnableClick = true;
        fade.SetActive(false);
        content.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            content.SetActive(false);
        });
    }
}
