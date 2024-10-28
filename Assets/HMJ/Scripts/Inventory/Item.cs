using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    [Serializable]
    public struct ItemData
    {
        public Texture image;
        public string itemName;
        public int n;
    }

    private ItemData itemdata;

    /// <summary>
    /// 인스펙터 연결 데이터
    /// </summary>
    public RawImage image;
    public TMP_Text name;
    public TMP_Text N;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemData(ItemData _itemData)
    {
        itemdata = _itemData;
        image.texture = itemdata.image;
        name.text = itemdata.itemName;
    }
}
