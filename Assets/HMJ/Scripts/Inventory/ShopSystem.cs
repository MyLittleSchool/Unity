using GH;
using JetBrains.Annotations;
using MJ;
using Ookii.Dialogs;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject parentPanel; // ItemPrefab을 넣을 스크롤뷰의 content
    private List<GameObject> itemObjects = new List<GameObject>(); // 현재 창의 모든 오브젝트

    public List<Item.ItemData> items = new List<Item.ItemData>();
    public List<Item> itemComponents = new List<Item>();

    public List<ItemData> choiceItem;

    public GameObject sellPrefab;
    public GameObject sellParentPanel;

    public Button sellButton;
    public Button cancleButton;

    public GameObject sellPopupPanel;
    private FadeOutUI sellFadeOutUI;

    public TMP_Text allPrice;

    // Start is called before the first frame update
    void Start()
    {
        InitItem();
        InitButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitButton()
    {
        sellButton.onClick.AddListener(SellItems);
        cancleButton.onClick.AddListener(CancleItems);
        sellFadeOutUI = sellPopupPanel.GetComponentInChildren<FadeOutUI>();
    }

    public void SellItems()
    {
        SettingInventoryItem();
        choiceItem.Clear();
        UpdateChoiceItemData();
        OnSellUI();
        ResetShopItemNs();
    }

    public void CancleItems()
    {
        choiceItem.Clear();
        UpdateChoiceItemData();
        ResetShopItemNs();
    }

    public void OnSellUI()
    {
        sellFadeOutUI.FadeInOut(0.1f, 1.0f);
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
        int idx = ContainItem(_itemData.itemName);
        if (idx < 0)
        {
            ItemData choiceItemdata = new ItemData(_itemData);
            choiceItemdata.n = 1;
            choiceItem.Add(choiceItemdata);
        }
        else
        {
            choiceItem[idx].n++;
            choiceItem[idx].price += _itemData.price;
        }
        UpdateCalculatePrice();
    }

    public int ContainItem(string ItemName)
    {
        for(int i = 0; i < choiceItem.Count; i++)
        {
            if (choiceItem[i].itemName == ItemName)
                return i;
        }

        return -1;
    }

    public void UpdateChoiceItemData()
    {
        ResetChoiceItemData();
        foreach (ItemData itemData in choiceItem)
        {
            GameObject itemGame = Instantiate(sellPrefab, sellParentPanel.transform);
            itemGame.GetComponent<BuyItem>().SetData(itemData.itemName, itemData.n, itemData.price);
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

    public void ResetShopItemNs()
    {
        // 버튼 클릭시 클릭 수 리셋 - 아이템 구매 수
        foreach(GameObject item in itemObjects)
            item.GetComponentInChildren<ClickButton>().ResetClick();

        // 가격 계산 업데이트 - 0
        UpdateCalculatePrice();
    }

    public int CalculatePrice()
    {
        int AllPrice = 0;
        foreach (ItemData itemData in choiceItem)
            AllPrice += itemData.price;
        return AllPrice;
    }

    public void UpdateCalculatePrice()
    {
        allPrice.text = CalculatePrice().ToString();
    }

    // 구매시 인벤토리 아이템을 셋팅해준다.
    public void SettingInventoryItem()
    {
        foreach (ItemData itemData in choiceItem)
            InventorySystem.GetInstance().AddItem(itemData.n, itemData.itemName);
    }
}
