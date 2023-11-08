using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFreeSpinNumbers : MonoBehaviour
{
    [SerializeField] GameObject []numberA;
    [SerializeField] GameObject []numberB;
    [SerializeField] GameObject group;

    // Start is called before the first frame update
    void Start()
    {
        //SetNumber(8);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetNumber(int num)
    {
        if (num > 99)
            num = 99;
        else if (num < 0)
            num = 0;

        if(num < 10)
        {
            int a = num;
            //Hide all number
            for (int i = 0; i <= 9; i++)
            {
                numberA[i].SetActive(false);
                numberB[i].SetActive(false);
            }

            numberA[a].SetActive(true);
            group.transform.localPosition = new Vector3(104, 0, 0);
        }
        else
        {
            int a = num / 10;
            int b = num % 10;
            Debug.Log(a + ", " + b);

            //Hide all number
            for (int i = 0; i <= 9; i++)
            {
                numberA[i].SetActive(false);
                numberB[i].SetActive(false);
            }

            numberA[a].SetActive(true);
            numberB[b].SetActive(true);
            group.transform.localPosition = new Vector3(0, 0, 0);

        }

    }
    public void Show()
    {
        group.SetActive(true);
    }
    public void Hide()
    {
        //SoundManager.Instance.PlaySFX("Close");

        group.SetActive(false);
    }
}
