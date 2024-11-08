using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    private bool bToggle = false;
    private Button button;
    private Image buttonImage;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        button.onClick.AddListener(Toggle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle()
    {
        bToggle = !bToggle;
        if (bToggle)
            buttonImage.color = Color.red;
        else
            buttonImage.color = Color.white;
    }
}
