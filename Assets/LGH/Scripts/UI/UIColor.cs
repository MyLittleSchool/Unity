using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIColor : MonoBehaviour
{
    private Image underLine;
    private TMP_Dropdown dropdown;
    private TMP_InputField inputField;
    private Color focuseColor = new Color32 (242, 136, 75, 255);
    private Color noneFouseColor = new Color32 (0, 0, 0, 41);
    void Start()
    {
        underLine = GetComponent<Image>();
        inputField = GetComponentInParent<TMP_InputField>();
        dropdown = GetComponentInParent<TMP_Dropdown>();
    }

    void Update()
    {
        if (inputField != null && inputField.isFocused)
        {
            underLine.color = focuseColor;
        }
        else
        {
            underLine.color = noneFouseColor;
        }

        if (dropdown != null && dropdown.value != 0)
        {
            underLine.color = focuseColor;
        }
        else
        {
            underLine.color = noneFouseColor;
        }
    }
}
