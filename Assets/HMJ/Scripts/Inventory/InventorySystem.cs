using GH;
using MJ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static HttpManager;
using static Item;

public class InventorySystem : MonoBehaviour
{
    // 인벤토리 종류 버튼
    public Button[] inventoryTypeButton;
    public Button installButton;

    /// <summary>
    /// 인벤토리 부모 및 자식 프리펩
    /// </summary>
    public GameObject parentPanel;
    public Item[] itemComponents;
    public GameObject[] childPanel;

    Item choiceItem;

    /// <summary>
    /// 통신 구조체
    /// </summary>
    [Serializable]
    public class ItemData
    {
        public int inventoryId;
        public int itemIdx;
        public string itemName; // 아이템 이름
        public int price; // 가격
        public string itemType; // 아이템 타입
        public int count; // 아이템 개수

        public ItemData(int _inventoryId, int _itemIdx, string _itemName, int _price, string _itemType, int _count)
        {
            inventoryId = _inventoryId;
            itemIdx = _itemIdx;
            itemName = _itemName;
            price = _price;
            itemType = _itemType;
            count = _count;
        }
    }

    public struct QuestItem
    {
        public string ItemName;
        public Sprite textureSprite;

        public QuestItem(string _ItemName, Sprite _textureSprite)
        {
            ItemName = _ItemName;
            textureSprite = _textureSprite;
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

        StartCoroutine(FirstLoadData());
    }

    // 현재 부모에 자식들을 넣어줘야 한다. 이걸 어떻게 넣어줄까?
    // 만약 인벤토리의 내용물이 바뀌었다면?
    // 그때 해당 인벤토리 아이템 정보를 가져온다.
    // 해당 아이템 정보를 생성해서 넣어준다.

    //private List<Item>[] itemList = new List<Item>[2];
    private ItemType curInventoryItemType = ItemType.Common;

    private ItemDatas[] loadItemData = new ItemDatas[2];

    public int Gold = 10000000;

    public void InitItemList()
    {
        SettingInventory(ItemType.MyClassRoom);
        SettingInventory(ItemType.Common);

        ResetButton();
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
            inventoryTypeButton[i].onClick.AddListener(
            ResetChoiceItem);
        }

        installButton.onClick.AddListener(InstallItem);
    }
    private void Start()
    {

    }

    private void Update()
    {
    }

    public ItemType GetCurItemType()
    {
        if (choiceItem)
            return choiceItem.itemType;
        else
            return ItemType.ItemTypeEnd;
    }

    public void UpdateItemData(string itemType, string itemName, int _Count)
    {
        ItemData searchItem;
        ItemType _itemType = ItemType.Common;
        if (itemType == "MyClassRoom")
            _itemType = ItemType.MyClassRoom;

        searchItem = loadItemData[(int)_itemType].response.Find(x => itemName == x.itemName);
        if(searchItem != null)
            searchItem.count += _Count;

        SetItemComponent(_itemType);
    }

    public void PatchItemData(ItemType _itemType, string _itemName, int _Count)
    {
        // /list/" + DataManager.instance.mapId + "/" + itemData.itemType;
        ItemData itemData = loadItemData[(int)_itemType].response.Find(x => _itemName == x.itemName);
        itemData.count += _Count;

        Debug.Log("Patch - 아이템 이름: " + itemData.itemName + " 인덱스: " + itemData.itemIdx + " 아이템 타입: " + itemData.itemType + " 인벤토리 id: " + itemData.inventoryId);
        //UpdateItemData(itemData.itemType, _itemName, _Count);

        if (null == itemData)
            return;

        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/inventory";
        info.body = JsonUtility.ToJson(itemData);
        info.contentType = "application/json";
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            SetItemComponent(_itemType);
            print(downloadHandler.text);
        };
        StartCoroutine(HttpManager.GetInstance().Patch(info));
    }

    public void PatchItemData(string _itemName, int _Count)
    {
        ItemData itemData = null;
        // /list/" + DataManager.instance.mapId + "/" + itemData.itemType;
        for (int i = 0; i < 2; i++)
        {
            itemData = loadItemData[i].response.Find(x => _itemName == x.itemName);
            if (null != itemData)
                break;
        }

        ItemType _itemType = ItemType.Common;
        if (itemData.itemType == "MyClassRoom")
            _itemType = ItemType.MyClassRoom;
        itemData.count += _Count;

        if (null == itemData)
            return;

        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/inventory";
        info.body = JsonUtility.ToJson(itemData);
        info.contentType = "application/json";
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            SetItemComponent(_itemType);
            print(downloadHandler.text);
        };
        StartCoroutine(HttpManager.GetInstance().Patch(info));
    }

    public void PatchItemData(int idx, int _Count)
    {
        ItemData itemData = InventorySystem.GetInstance().GetItemData(idx);

        itemData.count += _Count;

        if (null == itemData)
            return;

        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/inventory";
        info.body = JsonUtility.ToJson(itemData);
        info.contentType = "application/json";
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            print(downloadHandler.text);
        };
        StartCoroutine(HttpManager.GetInstance().Patch(info));
    }

    public void GetItemData(ItemType _itemType)
    {
        // itemList[(int)_itemType].Clear();

        ItemDatas itemData;
        HttpInfo info = new HttpInfo();
        info.url = HttpManager.GetInstance().SERVER_ADRESS + "/inventory/list/" + DataManager.instance.mapId + "/" + _itemType.ToString();
        info.onComplete = (DownloadHandler downloadHandler) =>
        {
            Debug.Log("GetItemData: " + downloadHandler.text);


            string wrappedJson = "{\"response\":" + downloadHandler.text + "}";

            print("a : " + wrappedJson);

            itemData = JsonUtility.FromJson<ItemDatas>(wrappedJson);
            loadItemData[(int)_itemType] = itemData;

            foreach (ItemData itemdata in loadItemData[(int)_itemType].response)
            {
                GameObject itemPrefab = LoadItemData(_itemType, itemdata.itemName);
                if (itemPrefab)
                    Debug.Log("itemPrefab - Name: " + itemdata.itemName + ", " + itemdata.count);
                else
                    Debug.Log("Null - itemPrefab  - " + itemdata.itemName);
            }


        };
        StartCoroutine(HttpManager.GetInstance().Get(info));
    }


    public void SetItemComponent(ItemType _itemType)
    {
        ResetItemComponents();
        if (loadItemData[(int)_itemType].response == null || loadItemData[(int)_itemType].response.Count <= 0 || childPanel.Count() <= 0)
            return;
        for (int i = 0; i < loadItemData[(int)_itemType].response.Count(); i++)
        {
            if (childPanel[i] == null)
                continue;
                Debug.Log("현재 i번째 null: " + i);
            childPanel[i].SetActive(true);
            GameObject itemPrefab = LoadItemData(_itemType, loadItemData[(int)_itemType].response[i].itemName);
            itemComponents[i].SetData(loadItemData[(int)_itemType].response[i].itemName, loadItemData[(int)_itemType].response[i].price, loadItemData[(int)_itemType].response[i].itemType.ToString(), loadItemData[(int)_itemType].response[i].count, itemPrefab);
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
        {
            if (childPanel[i])
                childPanel[i].SetActive(false);
        }
    }
    public GameObject LoadItemData(ItemType _itemType, string _itemName)
    {
        string ItemPath = "Inventory/" + _itemType.ToString() + "/" + _itemName;

        // 해당 인벤토리 게임 오브젝트
        return FileManager.Instance.LoadGameObjectFromResource(ItemPath);

    }

    private void OnEnable()
    {
        //SettingInventory(ItemType.MyClassRoom);
        //SettingInventory(ItemType.Common);

        SetItemComponent(ItemType.Common);
        //ResetButton();
    }


    IEnumerator WaitForConditionToBeTrue(ItemType _itemType)
    {
        GetItemData(_itemType);
        Debug.Log("ItemType: " + _itemType.ToString());
        Debug.Log("Waiting for condition...");
        // 조건이 true가 될 때까지 대기
        yield return new WaitUntil(() => loadItemData[(int)_itemType].response != null && loadItemData[(int)_itemType].response.Count() > 0);

        SetItemComponent(_itemType);
        Debug.Log("Condition met! Proceeding...");
    }

    IEnumerator FirstLoadData()
    {
        InitItemList();
        GetItemComponents();
        InitInventoryButtons();
        // 조건이 true가 될 때까지 대기
        yield return new WaitUntil(() => (loadItemData[0].response != null && loadItemData[0].response.Count() > 0) && (loadItemData[1].response != null && loadItemData[1].response.Count() > 0));

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
        curInventoryItemType = _itemType;
    }

    public bool CheckItem()
    {
        if (choiceItem && choiceItem.count > 0)
            return true;
        return false;
    }

    public QuestItem GetQuestItemIndex(int idx) // 인덱스
    {
        ItemData itemdata;
        ItemType itemType = ItemType.MyClassRoom;

        // 각 카테고리별 카운트 계산
        if (idx < loadItemData[(int)ItemType.MyClassRoom].response.Count()) //  MyClassRoom 타입 인덱스
            itemdata = loadItemData[(int)ItemType.MyClassRoom].response[idx];
        else
            itemdata = loadItemData[(int)ItemType.Common].response[idx - loadItemData[(int)ItemType.MyClassRoom].response.Count];

        if (itemdata.itemType == "Common")
            itemType = ItemType.Common;

        SpriteRenderer spriteRenderer = GetItemIndex(idx).GetComponent<SpriteRenderer>();
        return new QuestItem(itemdata.itemName, spriteRenderer.sprite);
        // 이름, 이미지
    }

    //public void AddItemData(ItemType _itemType, string _itemName, int _itemCount)
    //{
    //    // 해당하는 아이템 정보가 있으면
    //    Item itemData = itemList[(int)_itemType].Find(x => _itemName == x.itemName);
    //    if (itemData)
    //    {
    //        int idx = itemList[(int)_itemType].IndexOf(itemData);
    //        itemData.count += _itemCount;
    //    }
    //}

    //public Item GetItemData(ItemType _itemType, string _itemName)
    //{
    //    return itemList[(int)_itemType].Find(x => _itemName == x.itemName);
    //}

    public void SetChoiceItem(Item itemCom)
    {
        choiceItem = itemCom;
        DataManager.instance.setTileObj = choiceItem.prefab;
        for (int i = 0; i < 2; i++)
        {
            ItemData findItemData = loadItemData[i].response.Find(x => choiceItem.prefab.name == x.itemName);
            if (null != findItemData)
                DataManager.instance.setTileObjId = findItemData.itemIdx;
        }

    }


    public void InstallItem()
    {
        SetTile.instance.OnTile();
    }

    public void ResetChoiceItem()
    {
        choiceItem = null;
    }

    public GameObject GetItemIndex(int idx) // 인덱스
    {
        ItemData itemdata;
        ItemType itemType = ItemType.MyClassRoom;

        // 각 카테고리별 카운트 계산
        if (idx < loadItemData[(int)ItemType.MyClassRoom].response.Count()) //  MyClassRoom 타입 인덱스
            itemdata = loadItemData[(int)ItemType.MyClassRoom].response[idx];
        else
            itemdata = loadItemData[(int)ItemType.Common].response[idx - loadItemData[(int)ItemType.MyClassRoom].response.Count];

        if (itemdata.itemType == "Common")
            itemType = ItemType.Common;

        return LoadItemData(itemType, itemdata.itemName);

        // 이름, 이미지
    }

    public ItemData GetItemData(int idx) // 인덱스
    {
        // 각 카테고리별 카운트 계산
        if (idx < loadItemData[(int)ItemType.MyClassRoom].response.Count()) //  MyClassRoom 타입 인덱스
            return loadItemData[(int)ItemType.MyClassRoom].response[idx];
        else
            return loadItemData[(int)ItemType.Common].response[idx - loadItemData[(int)ItemType.MyClassRoom].response.Count];
    }
}
