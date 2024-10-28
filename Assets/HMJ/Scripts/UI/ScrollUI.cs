using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollUI : MonoBehaviour
{
    protected List<GameObject> itemlist = new List<GameObject>();
    protected List<Image> imageList = new List<Image>();

    public Transform content;
    public GameObject prefab;

    public Button addItemButton;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(addItemButton)
            addItemButton.onClick.AddListener(AddItem);
        LoadItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void AddItem()
    {
        GameObject item = Instantiate(prefab, content);
        itemlist.Add(item);
        imageList.Add(item.GetComponent<Image>());
    }

    public void LoadItem()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            itemlist.Add(content.GetChild(i).gameObject);
            imageList.Add(content.GetChild(i).gameObject.GetComponent<Image>());
        }
    }
}
