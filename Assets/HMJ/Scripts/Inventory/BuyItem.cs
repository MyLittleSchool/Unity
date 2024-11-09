using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BuyItem : MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text itemN;
    public TMP_Text itemPrice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(string _itemName, int _n, int _price)
    {
        itemName.text = _itemName;
        itemN.text = _n.ToString();
        itemPrice.text = _price.ToString();
    }
}
