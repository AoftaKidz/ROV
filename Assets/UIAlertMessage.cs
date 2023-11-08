using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIAlertMessage : MonoBehaviour
{
    public static UIAlertMessage Instance = null;
    [SerializeField] TextMeshProUGUI txtMessage;
    [SerializeField] TextMeshProUGUI txtMessage2;

    [SerializeField] GameObject content;
    [SerializeField] GameObject fade;
    [SerializeField] GameObject btnOK;
    public delegate void UIAlertMessageCallback(); // declare delegate type
    UIAlertMessageCallback callback = null;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show(string message, UIAlertMessageCallback callback = null)
    {
        btnOK.SetActive(true);
        this.callback = callback;
        txtMessage2.gameObject.SetActive(false);
        txtMessage.gameObject.SetActive(true);
        txtMessage.text = message;
        fade.SetActive(true);
        content.SetActive(true);
        content.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutQuint);
    }
    public void Show(string message,bool isShowButton, UIAlertMessageCallback callback = null)
    {
        if (isShowButton)
            btnOK.SetActive(true);
        else
            btnOK.SetActive(false);

        this.callback = callback;
        txtMessage2.gameObject.SetActive(true);
        txtMessage.gameObject.SetActive(false);
        txtMessage2.text = message;
        fade.SetActive(true);
        content.SetActive(true);
        content.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutQuint);
    }
    public void Hide()
    {
        content.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            content.SetActive(false);
            fade.SetActive(false);
            this.callback = null;
        });
    }
    public void OnClick_OK()
    {
        if (callback != null)
            callback();
        Hide();
    }
}
