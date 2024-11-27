using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public Button inventoryButton;

    private void Awake()
    {
        InitButtons();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitButtons()
    {
        inventoryButton.onClick.AddListener(() => InventorySystem.GetInstance().SetChoiceItem(GetComponentInParent<Item>()));
    }
}
