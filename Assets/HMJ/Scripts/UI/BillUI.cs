using MJ;
using Ookii.Dialogs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BillUI : MonoBehaviour
{
    public Button billButton;
    public Button buyButton;
    public Button noBuyButton;

    public TMP_Text BillText;
    public GameObject BillPanel;

    public TMP_Text BillPrice;

    private InventorySystem inventorySystem;
    private MapContestLoader mapContestLoader;

    Dictionary<string, int> ItemMap = new Dictionary<string, int>();
    // Start is called before the first frame update
    void Start()
    {
        inventorySystem = InventorySystem.GetInstance();

        mapContestLoader = MapContestLoader.GetInstance();

        SettingButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SettingButton()
    {
        billButton.onClick.AddListener(OnBillPanel);
        buyButton.onClick.AddListener(ClickBuyButton);
        noBuyButton.onClick.AddListener(ClickNoBuyButton);
    }

    public void OnBillPanel()
    {
        BillPanel.SetActive(true);
        SettingBillData();
        BuyFunitureItem();
    }

    public void OffBillPanel()
    {
        BillPanel.SetActive(false);
    }

    public void ClickBuyButton()
    {
        OffBillPanel();
        // 이때 프리셋 적용
        mapContestLoader.MapCopyFurniture();

    }

    public void ClickNoBuyButton()
    {
        OffBillPanel();

        // 이때 프리셋 적용x
    }

    public void calculateItem()
    {
        ItemMap.Clear();
        foreach (ObjectContestInfo objectContestInfo in MapContestLoader.GetInstance().loadfurnitureList)
        {
            Item.ItemData item = inventorySystem.items[objectContestInfo.objId];
            if (ItemMap.ContainsKey(item.itemName))
                ItemMap[item.itemName]++;
            else
                ItemMap.Add(item.itemName, 1);

        }
    }

    public void calculateDeleteItem()
    {
        // 현재 내 맵에서 삭제된 정보 인벤토리에 다시 넣기
        foreach (DeleteItemData deleteItemData in MapContestLoader.GetInstance().deleteItemDataLists.response)
            ++inventorySystem.items[deleteItemData.objectId].n;
    }

    public void SettingBillData()
    {
        calculateItem();

        BillText.text = "";
        foreach (KeyValuePair<string, int> item in ItemMap)
            BillText.text += item.Key + " X " + item.Value + "\n";
    }

    public void BuyFunitureItem()
    {
        // 가구 모두 지우기
        MapContestLoader.GetInstance().MapContestDeleteAllFurniture();

        StartCoroutine(WaitForConditionToBeTrue());

        // 지운 가구 데이터 계산
        calculateDeleteItem();

        BuyItem();
        //

    }

    public bool LoadDeleteItemComplete()
    {
        if (mapContestLoader.deleteItemDataLists.response.Count == 0)
            return false;

        return true;
    }

    IEnumerator WaitForConditionToBeTrue()
    {
        Debug.Log("Waiting for condition...");
        // 조건이 true가 될 때까지 대기
        yield return new WaitUntil(() => LoadDeleteItemComplete());

        Debug.Log("Condition met! Proceeding...");
    }

    public void BuyItem()
    {
        Dictionary<string, int> sellItemMap = new Dictionary<string, int>();
        foreach (KeyValuePair<string, int> item in ItemMap)
        {
            int remainData = inventorySystem.UseItem(item.Value, item.Key);

            if (sellItemMap.ContainsKey(item.Key))
                sellItemMap[item.Key] += -remainData;
            else
                sellItemMap.Add(item.Key, -remainData);
        }

        int prices = 0;
        foreach (KeyValuePair<string, int> item in sellItemMap)
        {
            prices += inventorySystem.GetPrice(item.Value, item.Key);
        }
        BillPrice.text = prices.ToString();
    }


}
