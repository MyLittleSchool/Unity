using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using MJ;

public class MapRegisterScrollUI : ScrollUI
{
    public MapContestScrollUI myMapContestScrollUIcp;
    private GameObject registerObject;

    private Sprite spriteData;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.Start();
        LoadClassImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetColor()
    {
        foreach (var image in imageList)
            image.color = Color.white;
    }

    public void SetRegisterGameObject(GameObject _registerObject)
    {
        registerObject = _registerObject;
    }

    public GameObject GetRegisterGameObject()
    {
        return content.GetChild(0).gameObject;
    }

    public override void AddItem() 
    {
        itemlist.Clear();
        imageList.Clear();
        content.DetachChildren();

        prefab.GetComponent<MapRegisterDataUI>().SetRegisterImageData(spriteData);
        GameObject item = Instantiate(prefab, content);

        itemlist.Add(item);
        // item.GetComponent<Image>().settex
        imageList.Add(item.GetComponent<Image>());
    }

    public void LoadClassImage()
    {
        spriteData = SetPrefabImage();
    }

    public Sprite SetPrefabImage()
    {
        return CaptureManager.GetInstance().CaptureRenderTexture();
    }
}
