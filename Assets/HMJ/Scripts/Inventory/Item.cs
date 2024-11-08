using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Item;

public class Item : MonoBehaviour
{
    public enum ItemType
    { 
        InventoryItem,
        ShopItem,
        ItemType_End
    }

    private ItemType itemType;
    /// <summary>
    /// 인스펙터 연결 데이터
    /// </summary>
    public RawImage image;
    public TMP_Text name;
    public TMP_Text N;
    public Button button;

    [Serializable]
    public class ItemData
    {
        public Texture image;
        public string itemName;
        public int n;
        public GameObject prefab;

        public int N
        {
            get { return n; }
            set
            {
                n = value;
            }
        }
    }

    private ItemData itemdata;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetItemData(ItemData _itemData, ItemType _itemType)
    {
        itemdata = _itemData;
        image.texture = itemdata.image;
        name.text = itemdata.itemName;

        itemType = _itemType;
        ItemTypeSetting();
    }

    public void ItemTypeSetting()
    {
        switch (itemType)
        {
            case ItemType.InventoryItem:
                button.onClick.AddListener(() => InventorySystem.GetInstance().SetChoiceItem(itemdata));
                break;
            case ItemType.ShopItem:
                button.onClick.AddListener(() => SellSystem.GetInstance().SetChoiceItem(itemdata));
                // Sell 시스템 아이템 데이터 업데이트
                button.onClick.AddListener(SellSystem.GetInstance().UpdateChoiceItemData);
                break;
        }

    }

    public void UpdateItemData()
    {
        image.texture = itemdata.image;
        name.text = itemdata.itemName;
        N.text = itemdata.N.ToString();
    }
    public void SetItemPrefab()
    {
        InventorySystem.GetInstance().SetChoiceItem(itemdata);
        SellSystem.GetInstance().SetChoiceItem(itemdata);
    }
}
