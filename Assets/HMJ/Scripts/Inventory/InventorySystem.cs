using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject parentPanel;
    public List<Item.ItemData> items = new List<Item.ItemData>();

    // Start is called before the first frame update
    void Start()
    {
        InitItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitItem()
    {
        foreach (Item.ItemData item in items)
        {
            GameObject itemGame = Instantiate(itemPrefab, parentPanel.transform);
            itemGame.GetComponent<Item>().SetItemData(item);
        }

    }
}
