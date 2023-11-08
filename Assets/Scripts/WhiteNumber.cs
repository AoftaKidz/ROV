using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WhiteNumber : MonoBehaviour
{
    [SerializeField] SpriteRenderer num1;
    [SerializeField] SpriteRenderer num2;

    public void SetNumber(int num)
    {
        if (num > 99)
            num = 99;
        else if (num < 0)
            num = 0;

        if (num < 10)
        {
            num1.gameObject.SetActive(true);
            num2.gameObject.SetActive(false);
            num1.sprite = Resources.Load<Sprite>("SlotMachine/Spine/Freespin/NumWhite/" + num);
            //transform.localPosition = new Vector3(0.7f, 0);
        }
        else
        {
            num1.gameObject.SetActive(true);
            num2.gameObject.SetActive(true);

            int a = num / 10;
            int b = num % 10;
            num1.sprite = Resources.Load<Sprite>("SlotMachine/Spine/Freespin/NumWhite/" + a);
            num2.sprite = Resources.Load<Sprite>("SlotMachine/Spine/Freespin/NumWhite/" + b);
            transform.localPosition = Vector3.zero;
        }
    }
    public void JellyEffect()
    {
        transform.localScale = new Vector3(0.25f,0.25f,0.25f);
        transform.DOScale(0.5f, 0.5f).SetEase(Ease.OutElastic);
    }
}
