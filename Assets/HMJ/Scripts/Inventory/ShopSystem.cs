using GH;
using MJ;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class SellSystem : MonoBehaviour
{
    #region SingleTon
    private static SellSystem instance;
    public static SellSystem GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            go.name = "InventorySystem";
            go.AddComponent<SellSystem>();
        }
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public GameObject itemPrefab;
    public GameObject parentPanel;
    private List<GameObject> itemObjects = new List<GameObject>();

    public List<Item.ItemData> items = new List<Item.ItemData>();
    public List<Item> itemComponents = new List<Item>();

    public List<ItemData> choiceItem;

    public GameObject sellPrefab;
    public GameObject sellParentPanel;

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
            itemComponents.Add(itemGame.GetComponent<Item>());
            itemGame.GetComponent<Item>().SetItemData(item, ItemType.ShopItem);
            itemObjects.Add(itemGame);
        }

    }

    public void SetChoiceItem(ItemData _itemData)
    {
        int idx = choiceItem.IndexOf(_itemData);
        if (idx < 0)
            choiceItem.Add(_itemData);
        else
            choiceItem.RemoveAt(idx);

    }
    public void UpdateChoiceItemData()
    {
        ResetChoiceItemData();
        foreach (ItemData itemData in choiceItem)
        {
            GameObject itemGame = Instantiate(sellPrefab, sellParentPanel.transform);
            itemGame.GetComponent<BuyItem>().SetData(itemData.itemName, 0);
        }

    }

    public void ResetChoiceItemData()
    {
        foreach (Transform child in sellParentPanel.transform)
            Destroy(child.gameObject);
    }

    public int GetItemIndex(Item.ItemData _itemData)
    {
        return items.IndexOf(_itemData);
    }
}
