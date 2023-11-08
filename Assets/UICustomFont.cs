using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICustomFont : MonoBehaviour
{
    List<GameObject> numbers = new List<GameObject>();
    float numberWidth = 0;

    // Start is called before the first frame update
    void Start()
    {
        //SetText(27890.12f);
    }
    public void SetText(float value)
    {
        //Clear
        numberWidth = 0;
        foreach (var item in numbers)
        {
            Destroy(item.gameObject);
        }
        numbers.Clear();
        var _s = string.Format("{0:#,#.00}", value);
        //string _s = value.ToString();
        //Debug.Log(_s);
        // Sprite sp = Resources.Load<Sprite>("SlotMachine/Spine/Freespin/NumWhite/" + filename);
        float _space = -18;
        foreach (var c in _s)
        {
            GameObject _n;
            if (c == '.')
                _n = CreateNumber("Dot");
            else if (c == ',')
                _n = CreateNumber("Comma");
            else if (c == '-')
                continue;
            else if(c == '+')
                continue;
            else
                _n = CreateNumber(c.ToString());
            if (_n == null)
                continue;
            numbers.Add(_n);
            _n.transform.SetParent(transform, false);
            Vector3 p = _n.transform.localPosition;
            p.x = numberWidth;
            _n.transform.localPosition = p;
            numberWidth += _n.GetComponent<Image>().sprite.rect.width + _space;
            if (c == '.')
            {
                numberWidth -= 56;
                p.x = numberWidth;
                p.y = -51;
                _n.transform.localPosition = p;
                numberWidth += _n.GetComponent<Image>().sprite.rect.width;

            }
            else if (c == ',')
            {
                numberWidth -= 56;
                p.x = numberWidth;
                p.y = -66;
                _n.transform.localPosition = p;
                numberWidth += _n.GetComponent<Image>().sprite.rect.width;
            }
        }
        
    }
    GameObject CreateNumber(string number)
    {
        //number 0..9 ',' '.'
        GameObject _n = new GameObject();
        Image _m = _n.AddComponent<Image>();
        _m.sprite = Resources.Load<Sprite>("SlotMachine/Spine/Freespin/NumWhite/" + number);
        _m.color = new Color(236f/255f,167f/255f,59f/255f);
        _n.name = number;
        _m.SetNativeSize();
        /*float aspectRatio = _m.sprite.rect.width / _m.sprite.rect.height;
        var fitter = _m.GetComponent<AspectRatioFitter>();
        fitter.aspectRatio = aspectRatio;*/
        
        return _n;
    }
}
