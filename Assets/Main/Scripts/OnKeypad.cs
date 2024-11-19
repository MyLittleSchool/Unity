using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnKeypad : MonoBehaviour
{
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        if (TouchScreenKeyboard.visible)
        {
            rectTransform.position = new Vector3(rectTransform.position.x, TouchScreenKeyboard.area.size.y, rectTransform.position.z);
        }
        else
        {
            rectTransform.position = new Vector3(rectTransform.position.x, 0, rectTransform.position.z);

        }
    }
}
