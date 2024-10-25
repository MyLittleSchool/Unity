using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuestbookContent : MonoBehaviour
{
    public GameObject delBtn;
    public Image bg;
    public void EnterPointer()
    {
        delBtn.SetActive(true);
        float h, s, v;
        Color.RGBToHSV(bg.color, out h, out s, out v);
        v -= 0.1f;
        bg.color = Color.HSVToRGB(h, s, v);
    }
    public void ExitPointer()
    {
        delBtn.SetActive(false);
        float h, s, v;
        Color.RGBToHSV(bg.color, out h, out s, out v);
        v += 0.1f;
        bg.color = Color.HSVToRGB(h, s, v);
    }
}
