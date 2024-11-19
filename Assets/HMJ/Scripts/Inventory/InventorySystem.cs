using GH;
using MJ;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class InventorySystem : MonoBehaviour
{
    private static InventorySystem instance;
    public static InventorySystem GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        InitItem();
    }

    public GameObject itemPrefab;
    public GameObject parentPanel;
    private List<GameObject> itemObjects = new List<GameObject>();

    public List<Item.ItemData> items = new List<Item.ItemData>();
    public List<Item> itemComponents = new List<Item>();

    public ItemData choiceItem;

    [Serializable]
    public struct ItemInfoData
    {
        public int id;
        public string itemName;
        public int count;
        public int price;
        public int userId;
        public string itemType;
    }

    [Serializable]
    public class ItemInfoDataList
    {
        public List<ItemInfoData> response;
    }


    // Start is called before the first frame update
    void Start()
    {
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
            Item itemComponent = itemGame.GetComponent<Item>();
            itemComponent.SetItemData(item, ItemType.InventoryItem);
            itemComponent.UpdateItemData();
            itemComponents.Add(itemComponent);
            itemObjects.Add(itemGame);
        }

    }

    public bool UseItem()                                                                                                                                                                                                                                   
    {
        int idx = items.IndexOf(choiceItem);
        if (items[idx].n <= 0)
            return false;
        else
            --items[idx].n;
        itemComponents[idx].UpdateItemData();

        return true;
    }

    // 모자란 개수 반환
    public int UseItem(int n, string ItemName)
    {
        int remainData = 0;
        int idx = ContainItem(ItemName);
        if (idx >= 0)
        {
            remainData = items[idx].n - n;
            if (items[idx].n - n < 0)
                items[idx].n = 0;
            else
               items[idx].n -= n;
            itemComponents[idx].UpdateItemData();

            if (remainData < 0)
                return remainData;
            else
                return 0;
        }
        return 0;
    }

    public void AddItem(int n, string ItemName)
    {
        int idx = ContainItem(ItemName);
        if (idx >= 0)
            itemComponents[idx].AddItemN(n);
    }

    public int ContainItem(string ItemName)
    {
        for (int i = 0; i < itemComponents.Count; i++)
        {
            if (itemComponents[i].GetItemData().itemName == ItemName)
                return i;
        }

        return -1;
    }

    public int GetPrice(int n, string ItemName)
    {
        int idx = ContainItem(ItemName);
        return itemComponents[idx].GetItemData().price * n;
    }

    public bool CheckItem()
    {
        return choiceItem.n > 0;
    }

    public void SetChoiceItem(ItemData _itemData)
    {
        choiceItem = _itemData;
        if (!CheckItem())
        {
            // UI 띄우기
            SceneUIManager.GetInstance().OnMapInventoryErrorPanel();
            return;
        }
        DataManager.instance.setTileObj = _itemData.prefab;
        DataManager.instance.setTileObjId = GetItemIndex(_itemData);
    }

    public int GetItemIndex(Item.ItemData _itemData)
    {
        return items.IndexOf(_itemData);
    }

    public void UpdateItemData()
    {
        foreach (Item itemComponent in itemComponents)
            itemComponent.UpdateItemData();
    }

    public void RegisterItem()
    {

    }

    private void OnEnable()
    {
        UpdateItemData();
    }
}
