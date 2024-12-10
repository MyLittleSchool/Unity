using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonObject : MonoBehaviour
{
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponentInChildren<Button>();
        if(button)
            button.onClick.AddListener(() => SoundManager.instance.ButtonMainEftSound(SoundManager.EButtonEftType.BUTTON_CLICK));
    }
}
