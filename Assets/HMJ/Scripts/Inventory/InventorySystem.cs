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
    #region SingleTon
    private static InventorySystem instance;
    public static InventorySystem GetInstance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            go.name = "InventorySystem";
            go.AddComponent<InventorySystem>();
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

    public ItemData choiceItem;
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
            Item itemComponent = itemGame.GetComponent<Item>();
            itemComponent.SetItemData(item, ItemType.InventoryItem);
            itemComponent.UpdateItemData();
            itemComponents.Add(itemComponent);
            itemObjects.Add(itemGame);
        }

    }

    public void UseItem()
    {
        int idx = items.IndexOf(choiceItem);
        --items[idx].n;
        itemComponents[idx].UpdateItemData();
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

    public bool CheckItem()
    {
        return choiceItem.n > 0;
    }

    public void SetChoiceItem(ItemData _itemData)
    {
        choiceItem = _itemData;
        if (!CheckItem())
        {
            // UI ¶ç¿ì±â
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

    private void OnEnable()
    {
        UpdateItemData();
    }
}
