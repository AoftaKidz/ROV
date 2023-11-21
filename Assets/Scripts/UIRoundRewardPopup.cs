using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;

public class UIRoundRewardPopup : MonoBehaviour
{
    public static UIRoundRewardPopup Instance = null;
    [SerializeField] GameObject content;
    [SerializeField] GameObject group;
    [SerializeField] TextMeshProUGUI txtRoundReward;
    [SerializeField] Image imgWin;
    [SerializeField] SkeletonGraphic spine;
    [SerializeField] UICustomFont customFont;

    float _alpha = 1;
    bool _isFadeIn = false;
    bool _isFadeOut = false;
    bool _isWait = false;
    float _time = 0;
    public bool isAppear = false;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Hide();
    }
    private void Update()
    {
        if (_isFadeIn)
        {
            _alpha += Time.deltaTime * 5.0f;
            if(_alpha > 1)
            {
                _alpha = 1;
                _isFadeIn = false;
                _isWait = true;
            }
        }else if (_isFadeOut)
        {
            _alpha -= Time.deltaTime * 5.0f;
            if (_alpha < 0)
            {
                _alpha = 0;
                _isFadeOut = false;
                content.SetActive(false);
            }
        }else if (_isWait)
        {
            _time += Time.deltaTime;
            if(_time > 2.5f)
            {
                _time = 0;
                _isWait = false;
                Hide();
            }

        }
        SetAlpha();
    }
    void SetAlpha()
    {
        return;
        {
            Color c = imgWin.color;
            c.a = _alpha;
            imgWin.color = c;
        }
        {
            Color c = txtRoundReward.color;
            c.a = _alpha;
            txtRoundReward.color = c;
        }
        {
            Color c = spine.color;
            c.a = _alpha;
            spine.color = c;
        }
    }
    void UpdateReward()
    {
        if (SlotMachine.Instance.slotData.reward == 0)
        {
            //var format = "0.00";
            //customFont.SetText(0.00f);
            txtRoundReward.text = SpriteNumberManager.ToYellow((0.00f).ToString());
        }
        else
        {
            var format = string.Format("{0:#,#.00}", SlotMachine.Instance.slotData.reward);
            //customFont.SetText(SlotMachine.Instance.slotData.reward);
            txtRoundReward.text = SpriteNumberManager.ToYellow(format);
        }
    }
    public void Show(float t = 0)
    {
        //return;
        if (SlotMachine.Instance.slotData.reward == 0) return;

        //more delay
        group.transform.DOLocalMoveX(1400, t).SetEase(Ease.OutQuint).OnComplete(() => {

            PuzzleInfo.Instance.Hide();
            SoundManager.Instance.PlaySFX("AllWin");
            isAppear = true;
            UpdateReward();
            _isFadeIn = true;
            _alpha = 0;
            content.SetActive(true);

            float x = 0;

           /* if (SlotMachine.Instance.slotData.reward < 10)
            {
                x = -88;
                txtRoundReward.fontSize = 300;
            }
            else if (SlotMachine.Instance.slotData.reward < 100)
            {
                x = -221;
                txtRoundReward.fontSize = 300;
            }
            else if (SlotMachine.Instance.slotData.reward < 1000)
            {
                x = -328;
                txtRoundReward.fontSize = 300;
            }
            else if (SlotMachine.Instance.slotData.reward < 10000)
            {
                x = -309;
                txtRoundReward.fontSize = 230;

            }
            else if (SlotMachine.Instance.slotData.reward < 100000)
            {
                x = -358;
                txtRoundReward.fontSize = 220;
            }
            else
            {
                x = 0;
            }*/

            group.transform.DOLocalMoveX(x, 0.6f).SetEase(Ease.OutQuint).OnComplete(() => {

            });
        });
    }
    public void Hide()
    {
        _isFadeOut = true;
        isAppear = false;
        group.transform.DOLocalMoveX(1400, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
        });
    }
    public bool Appear()
    {
        return isAppear;
    }
}
