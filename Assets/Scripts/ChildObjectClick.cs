using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObjectClick : MonoBehaviour
{
    void OnMouseDown()
    {
        PuzzleInfo.Instance.OnClick();
    }
}
