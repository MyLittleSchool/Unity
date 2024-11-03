using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MapRegisterScrollUI : ScrollUI
{
    public MapContestScrollUI myMapContestScrollUIcp;
    private GameObject registerObject;
    private Capture CaptureComponent;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.Start();

        CaptureComponent = GetComponent<Capture>();
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
        return registerObject;
    }

    public override void AddItem() 
    {
        Sprite spriteData = SetPrefabImage();

        prefab.GetComponent<MapRegisterDataUI>().SetRegisterImageData(spriteData);
        GameObject item = Instantiate(prefab, content);

        itemlist.Add(item);
        // item.GetComponent<Image>().settex
        imageList.Add(item.GetComponent<Image>());
    }

    public Sprite SetPrefabImage()
    {
        return CaptureComponent.CaptureRenderTexture();
    }
}
