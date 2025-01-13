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
        buttonImage = button.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeButtonClick()
    {
        if (buttonImage.color == selectColor)
            buttonImage.color = nonSelectColor;
        else
            buttonImage.color = selectColor;
    }

    public bool bSelectButton()
    {
        return buttonImage.color == selectColor;
    }
}
