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

    public enum ItemType
    {
        MyClassRoom,
        Common,
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


    public void SetData(string _itemName, int _price, ItemType _itemType, int _count, GameObject gameObject)
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

}