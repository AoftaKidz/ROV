using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIFreeSpinPopupTotal : MonoBehaviour
{
    [SerializeField] GameObject fade;
    [SerializeField] GameObject spine;
    [SerializeField] GameObject imgFreeSpin;
    [SerializeField] GameObject imgTotal;
    [SerializeField] GameObject button;
    public static UIFreeSpinPopupTotal Instance { get; private set; }

    float _score = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        //Show(0);
    }
    public void Show(float score)
    {
        SoundManager.Instance.PlaySFX("AllWin");
        fade.SetActive(true);
        _score = score;
        spine.transform.localScale = Vector3.zero;
        spine.transform.DOScale(new Vector3(1, 1, 1), 0.6f).SetEase(Ease.OutElastic).OnComplete(() =>
        {

        });

        Sequence s = DOTween.Sequence();
        s.SetDelay(0.4f);
        imgFreeSpin.transform.localScale = Vector3.zero;
        s.Append(imgFreeSpin.transform.DOScale(new Vector3(1, 1, 1), 0.6f).SetEase(Ease.OutElastic).OnComplete(() =>{
            imgTotal.transform.DOScale(new Vector3(1, 1, 1), 0.6f).SetEase(Ease.OutElastic).OnComplete(() => {
                button.SetActive(true);
            });
        }));
        s.Play();
    }
    public void Hide()
    {
        SoundManager.Instance.PlaySFX("Close");
        UIGameplay.Instance.NormalMode();
        //transform

        spine.transform.DOScale(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.OutQuint);

        Sequence s = DOTween.Sequence();
        s.SetDelay(0.4f);
        s.Append(imgFreeSpin.transform.DOScale(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.OutQuint).OnComplete(() => {
            imgTotal.transform.DOScale(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.OutQuint).OnComplete(() => { fade.SetActive(false); button.SetActive(false); });
        }));
        s.Play();
    }
}
