using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour
{
    public TMP_Text clickN;

    public Item item;

    public BuyItemList buyItemList;

    private bool bClick = false;
    private Button button;
    private Image buttonImage;
    int iClickN = 0;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        button.onClick.AddListener(Click);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        bClick = true;
        buttonImage.color = new Color(242.0f, 106.0f, 27.0f);
        iClickN++;
        buyItemList.AddBuyItem(item.itemType, item.itemName, item.price);
        SettingClickText();
    }

    public void ResetClick()
    {
        bClick = false;
        buttonImage.color = Color.white;
        iClickN = 0;
        SettingClickText();
    }

    public void SettingClickText()
    {
        if (iClickN <= 0)
            clickN.text = "";
        else
            clickN.text = "x" + iClickN.ToString();
    }

}
