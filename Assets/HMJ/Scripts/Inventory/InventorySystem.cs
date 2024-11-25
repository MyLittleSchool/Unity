using GH;
using MJ;
using Ookii.Dialogs;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpManager;
using static InventorySystem;
using static Item;

public class InventorySystem : MonoBehaviour
{
    // 인벤토리 종류 버튼
    public Button[] inventoryTypeButton;

    /// <summary>
    /// 인벤토리 부모 및 자식 프리펩
    /// </summary>
    public GameObject parentPanel;
    public Item[] itemComponents;
    public GameObject[] childPanel;
    /// <summary>
    /// 통신 구조체
    /// </summary>
    [Serializable]
    public struct ItemData
    {
        public string itemName; // 아이템 이름
        public int price; // 가격
        public string itemType; // 아이템 타입
        public int count; // 아이템 개수

        public ItemData(string _itemName, int _price, string _itemType, int _count)
        {
            itemName = _itemName;
            price = _price;
            itemType = _itemType;
            count = _count;
        }
    }

    [Serializable]
    public struct ItemDatas
    {
        public List<ItemData> response;
    }

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
        InitItemList();
        GetItemComponents();
        InitInventoryButtons();
    }

    // 현재 부모에 자식들을 넣어줘야 한다. 이걸 어떻게 넣어줄까?
    // 만약 인벤토리의 내용물이 바뀌었다면?
    // 그때 해당 인벤토리 아이템 정보를 가져온다.
    // 해당 아이템 정보를 생성해서 넣어준다.

    private List<Item>[] itemList = new List<Item>[2];
    private ItemType curInventoryItemType = ItemType.Common;

    public void InitItemList()
    {
        for (int i = 0; i < itemList.Count(); i++)
            itemList[i] = new List<Item>();
    }

    public void InitInventoryButtons()
    {
        for (int i = 0; i < inventoryTypeButton.Count(); i++)
        {
            int data = i;
            inventoryTypeButton[i].onClick.AddListener(
                () => ResetButton()
            );
            inventoryTypeButton[i].onClick.AddListener(
            () => SetItemComponent((ItemType)data)
            );
            inventoryTypeButton[i].onClick.AddListener(
            () => SelectButton((ItemType)data)
            );
        }
    }
    private void Start()
    {

    }
    public void FetchItemData()
    {

    }

    public void GetItemData(ItemType _itemType)
    {
        itemList[(int)_itemType].Clear();

        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/inventory/list/" + DataManager.instance.mapId + "/" + _itemType.ToString();
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            Debug.Log("GetItemData: " + downloadHandler.text);


            string wrappedJson = "{\"response\":" + downloadHandler.text + "}";

            print("a : " + wrappedJson);

            ItemDatas itemData = JsonUtility.FromJson<ItemDatas>(wrappedJson);

            foreach (ItemData itemdata in itemData.response)
            {
                GameObject itemPrefab = LoadItemData(_itemType, itemdata.itemName);
                if (itemPrefab)
                    Debug.Log("itemPrefab - Name: " + itemdata.itemName + ", " + itemdata.count);
                else
                    Debug.Log("Null - itemPrefab  - " + itemdata.itemName);
                itemList[(int)_itemType].Add(new Item(itemdata.itemName, itemdata.price, _itemType, itemdata.count, itemPrefab));
            }


        };
        StartCoroutine(HttpManager.GetInstance().Get(info));
    }

    public void SetItemComponent(ItemType _itemType)
    {
        ResetItemComponents();
        for (int i = 0; i < itemList[(int)_itemType].Count(); i++)
        {
            childPanel[i].SetActive(true);
            itemComponents[i].SetData(itemList[(int)_itemType][i].itemName, itemList[(int)_itemType][i].price, itemList[(int)_itemType][i].itemType, itemList[(int)_itemType][i].count, itemList[(int)_itemType][i].prefab);
        }
    }

    public void GetItemComponents()
    {
        itemComponents = parentPanel.GetComponentsInChildren<Item>();
        for (int i = 0; i < parentPanel.transform.childCount; i++)
            childPanel[i] = parentPanel.transform.GetChild(i).gameObject;
    }

    public void ResetItemComponents()
    {
        for (int i = 0; i < childPanel.Count(); i++)
            childPanel[i].SetActive(false);
    }
    public GameObject LoadItemData(ItemType _itemType, string _itemName)
    {
        string ItemPath = "Inventory/" + _itemType.ToString() + "/" + _itemName;

        // 해당 인벤토리 게임 오브젝트
        return FileManager.Instance.LoadGameObjectFromResource(ItemPath);

    }

    private void OnEnable()
    {
        SettingInventory(ItemType.Common);
        SettingInventory(ItemType.MyClassRoom);

        ResetButton();
        SelectButton(ItemType.MyClassRoom);
    }


    IEnumerator WaitForConditionToBeTrue(ItemType _itemType)
    {
        GetItemData(_itemType);
        Debug.Log("ItemType: " + _itemType.ToString());
        Debug.Log("Waiting for condition...");
        // 조건이 true가 될 때까지 대기
        yield return new WaitUntil(() => itemList[(int)_itemType].Count > 0);

        SetItemComponent(_itemType);
        Debug.Log("Condition met! Proceeding...");
    }

    public void SettingInventory(ItemType _itemType)
    {
        StartCoroutine(WaitForConditionToBeTrue(_itemType)); // 아이템 데
    }

    public void ResetButton()
    {
        for (int i = 0; i < inventoryTypeButton.Count(); i++)
        {
            inventoryTypeButton[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void SelectButton(ItemType _itemType)
    {
        inventoryTypeButton[(int)_itemType].GetComponent<Image>().color = new Color32(242, 106, 27, 255);
    }

}
