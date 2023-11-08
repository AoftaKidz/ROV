using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RewardAnimate : MonoBehaviour
{
    public TextMeshPro txtReward;
    public TextMeshProUGUI txtUIReward;
    public bool isUI = false;
    public float reward = 0;
    public float target = 0;
    public float step = 0;
    public float animateStep = 30;
    bool _isStart = false;
    float _time = 0;

    // Update is called once per frame
    void Update()
    {
        if (_isStart)
        {
            _time += Time.deltaTime;
            if (_time > 0.03f)
            {
                _time = 0;
                if (step >= 0)
                {
                    reward += step;
                    if (reward > target)
                    {
                        //Finish
                        _isStart = false;
                        reward = target;
                        SoundManager.Instance.StopSFX("CoinCount");

                    }
                    UpdateReward(reward);
                }
                else
                {
                    reward += step;
                    if (reward < target)
                    {
                        //Finish
                        _isStart = false;
                        reward = target;
                        SoundManager.Instance.StopSFX("CoinCount");
                    }
                    UpdateReward(reward);
                }
            }
        }
    }
    public void StartAnimate(float target)
    {
        SoundManager.Instance.PlaySFX("CoinCount");

        reward = 0;
        this.target = target;
        float d = target;
        step = d / animateStep;
        _isStart = true;
    }
    void UpdateReward(float reward)
    {
        if (reward == 0)
        {
            if (isUI)
                txtUIReward.text = SpriteNumberManager.ToMeowWhite("00.00");
            else
                txtReward.text = SpriteNumberManager.ToMeowWhite("00.00");
        }
        else
        {
            var format = string.Format("{0:#,#.00}", reward);
            if (isUI)
                txtUIReward.text = SpriteNumberManager.ToMeowWhite(format);
            else
                txtReward.text = SpriteNumberManager.ToMeowWhite(format);
        }
    }
}
