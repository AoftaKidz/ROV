using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float scaler = Screen.height / 2235.0f;
        CanvasScaler c = GetComponent<CanvasScaler>();
        
        float nativeRatio = 1284.0f / 2235.0f;
        float currRatio = (float)Screen.width / (float)Screen.height;
        if(currRatio < nativeRatio)
        {
            c.matchWidthOrHeight = 0;
        }
        else
        {
            c.matchWidthOrHeight = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
