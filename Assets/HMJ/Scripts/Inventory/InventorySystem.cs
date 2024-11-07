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
    #region SingleTone
    private static InventorySystem instence;
    public static InventorySystem GetInstance()
    {
        if (instence == null)
        {
            GameObject go = new GameObject();
            go.name = "InventorySystem";
            go.AddComponent<InventorySystem>();
        }
        return instence;
    }

    private void Awake()
    {
        if (instence == null)
        {
            instence = this;
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
            itemComponents.Add(itemGame.GetComponent<Item>());
            itemGame.GetComponent<Item>().SetItemData(item);
            itemObjects.Add(itemGame);
        }

    }

    public void SetChoiceItem(ItemData _itemData)
    {
        choiceItem = _itemData;
        if(!CheckItem())
        {
            // UI ¶ç¿ì±â
            SceneUIManager.GetInstance().OnMapInventoryErrorPanel();
            return;
        }
        DataManager.instance.setTileObj = choiceItem.prefab;
    }

    public void UseItem()
    {
        int idx = items.IndexOf(choiceItem);
        --items[idx].n;
        itemComponents[idx].UpdateItemData();
    }

    public bool CheckItem()
    {
        return choiceItem.n > 0;
    }
}
