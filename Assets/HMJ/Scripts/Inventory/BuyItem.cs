using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BuyItem : MonoBehaviour
{
    public TMP_Text itemName;

    private int n;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(string _itemName, int _n)
    {
        itemName.text = _itemName;
        n = _n;
    }
}
