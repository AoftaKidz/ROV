using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FadeManager : MonoBehaviour
{
    public static event Action OnFadeInComplete;
    public static event Action OnFadeOutComplete;

    public static FadeManager Instance = null;
    bool isFadeIn = false;
    bool isFadeOut = false;
    float _alpha = 0;
    Image _image;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _image = GetComponentInChildren<Image>();
        StartFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeIn)
        {
            _alpha -= Time.deltaTime;
            if(_alpha < 0)
            {
                _alpha = 0;
                isFadeIn = false;
                OnFadeInComplete?.Invoke();
                UpdateAlpha();
                _image.gameObject.SetActive(false);
            }
            else
            {
                UpdateAlpha();
            }
        }
        else if (isFadeOut)
        {
            _alpha += Time.deltaTime;
            if (_alpha > 1)
            {
                _alpha = 1;
                isFadeOut = false;
                OnFadeOutComplete?.Invoke();
                UpdateAlpha();
                //_image.gameObject.SetActive(false);
            }
            else
            {
                UpdateAlpha();
            }
        }
    }
    void UpdateAlpha()
    {
        Color c = _image.color;
        c.a = _alpha;
        _image.color = c;
    }
    public void StartFadeIn()
    {
        //1 -> 0
        isFadeIn = true;
        isFadeOut = false;
        _alpha = 1;
        _image.gameObject.SetActive(true);
        UpdateAlpha();
    }
    public void StartFadeOut()
    {
        //0 -> 1
        isFadeIn = false;
        isFadeOut = true;
        _alpha = 0;
        _image.gameObject.SetActive(true);
        UpdateAlpha();
    }
}
