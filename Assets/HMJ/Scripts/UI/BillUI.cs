using MJ;
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

    public TMP_Text BillGold;
    int price = 0;

    Dictionary<string, int> ItemMap = new Dictionary<string, int>();
    // Start is called before the first frame update
    void Start()
    {
        SettingButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SettingButton()
    {
        billButton.onClick.AddListener(OnBillPanel);

        buyButton.onClick.AddListener(BuyFunitureItem);
        buyButton.onClick.AddListener(ClickBuyButton);

        
        noBuyButton.onClick.AddListener(ClickNoBuyButton);
    }

    public void OnBillPanel()
    {
        billButton.gameObject.SetActive(false);
        BillPanel.SetActive(true);
        SettingBillData();

        BillGold.text = InventorySystem.GetInstance().Gold.ToString();
    }

    public void OffBillPanel()
    {
        BillPanel.SetActive(false);
    }

    public void ClickBuyButton()
    {
        OffBillPanel();
        // 이때 프리셋 적용
        MapContestLoader.GetInstance().MapCopyFurniture();

        InventorySystem.GetInstance().Gold -= price;

    }

    public void ClickNoBuyButton()
    {
        OffBillPanel();
        billButton.gameObject.SetActive(true);
        // 이때 프리셋 적용x
    }

    public void calculateItem()
    {
        price = 0;
        ItemMap.Clear();
        foreach (ObjectContestInfo objectContestInfo in MapContestLoader.GetInstance().loadfurnitureList)
        {
            InventorySystem.ItemData item = InventorySystem.GetInstance().GetItemData(objectContestInfo.objId);

            if(item != null)
            {
                if (ItemMap.ContainsKey(item.itemName))
                    ItemMap[item.itemName]++;
                else
                    ItemMap.Add(item.itemName, 1);

                price += item.price;
            }

        }
    }

    public void calculateDeleteItem()
    {
        if (MapContestLoader.GetInstance().deleteItemDataLists == null)
            return;
        // 현재 내 맵에서 삭제된 정보 인벤토리에 다시 넣기
        foreach (DeleteItemData deleteItemData in MapContestLoader.GetInstance().deleteItemDataLists.response)
        {
            InventorySystem.GetInstance().PatchItemData(deleteItemData.objectId, 1);
        }
    }

    public void SettingBillData()
    {
        calculateItem();

        BillText.text = "";
        foreach (KeyValuePair<string, int> item in ItemMap)
            BillText.text += item.Key + " X " + item.Value + "\n";

        BillPrice.text = price.ToString();
    }

    public void BuyFunitureItem()
    {
        // 가구 모두 지우기
        MapContestLoader.GetInstance().MapContestDeleteAllFurniture();

        StartCoroutine(WaitForConditionToBeTrue());

        // 지운 가구 데이터 계산
        calculateDeleteItem();


    }

    public bool LoadDeleteItemComplete()
    {
        if (MapContestLoader.GetInstance().deleteItemDataLists == null || MapContestLoader.GetInstance().deleteItemDataLists.response.Count == 0)
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



}
