using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILoading : MonoBehaviour
{
    [SerializeField] GameObject group;
    [SerializeField] TextMeshProUGUI txtLoading;
    float _time = 0;
    public float delay = 0.3f;
    int count = 1;
    public static UILoading Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        Hide();
    }
    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if(_time > delay)
        {
            _time = 0;
            count++;
            if (count > 3)
                count = 1;
        }

        if (count == 1)
            txtLoading.text = "°”≈—ß‚À≈¥.";
        else if (count == 2)
            txtLoading.text = "°”≈—ß‚À≈¥..";
        else
            txtLoading.text = "°”≈—ß‚À≈¥...";
    }
    public void Show()
    {
        group.SetActive(true);
    }
    public void Hide()
    {
        group.SetActive(false);
    }
}
