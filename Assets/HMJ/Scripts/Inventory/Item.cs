using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Item;

public class Item : MonoBehaviour
{
    public TmpItem tmpItem;
    public ClickButton clickButton;

    public enum ItemType
    {
        MyClassRoom,
        Common,
        All,
        ItemTypeEnd
    }

    /// <summary>
    /// 실제 아이템 데이터
    /// </summary>
    public string itemName;
    public int price;
    public ItemType itemType;
    public int count;

    /// <summary>
    /// 프리펩 데이터
    /// </summary>
    public GameObject prefab;

    public Item(string _itemName, int _price, ItemType _itemType, int _count, GameObject gameObject)
    {
        itemName = _itemName;
        price = _price;
        itemType = _itemType;
        count = _count;

        prefab = gameObject;

        if (tmpItem)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            tmpItem.SetText_Image(_itemName, _count, spriteRenderer.sprite.texture);

        }
           
    }


    public void SetData(string _itemName, int _price, string _itemType, int _count, GameObject gameObject)
    {
        itemName = _itemName;
        price = _price;

        itemType = ItemType.Common;
        if (_itemType == "MyClassRoom")
            itemType = ItemType.MyClassRoom;

        count = _count;

        prefab = gameObject;

        if (tmpItem)
        {
            Debug.Log("아이템 이름: " + _itemName);
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            tmpItem.SetText_Image(_itemName, _count, spriteRenderer.sprite.texture);
        }
    }

}