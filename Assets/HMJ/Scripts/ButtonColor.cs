using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : ButtonObject
{
    public Image buttonImage;
    public Color selectColor;
    public Color nonSelectColor;

    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeButtonClick()
    {
        if (buttonImage.color == selectColor)
        {
            buttonImage.color = nonSelectColor;
            Debug.Log("현재 버튼 색상: " + "선택x 색상");
        }
        else
        {
            buttonImage.color = selectColor;
            Debug.Log("현재 버튼 색상: " + "선택o 색상");
        }
    }

    public bool bSelectButton()
    {
        return buttonImage.color == selectColor;
    }
}
