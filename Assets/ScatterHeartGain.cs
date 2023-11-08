using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScatterHeartGain : MonoBehaviour
{
    Vector3 target = new Vector3(4.26f, 6.48f, 0);
    // Start is called before the first frame update
    void Start()
    {
        Show();
    }
    public void Show()
    {
        Vector3 p = target - transform.position;
        float deg = Mathf.Atan2(p.y,p.x) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, deg);

        transform.localScale = Vector2.zero;
        //Delay
        transform.DOScale(1, 0.3f).SetEase(Ease.OutElastic).OnComplete(() => {
            transform.DOScale(1, 0f).SetEase(Ease.OutElastic).OnComplete(() => {
                transform.DOMove(target, 0.3f).SetEase(Ease.Linear).OnComplete(() => {
                    SlotMachineScatterMode.Instance.AddScatterCount(1);
                    Destroy(gameObject);
                });
            });
        });
        
    }
}
