using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapContestScrollUI : ScrollUI
{
    public MapRegisterScrollUI myMapRegisterScrollUIcp;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void AddItem()
    {
        GameObject registerObject = myMapRegisterScrollUIcp.GetRegisterGameObject();
        if(registerObject)
        {
            GameObject item = Instantiate(prefab, content);
            //item.set
            MJ.MapContestDataUI mapContestDataUI = item.GetComponent<MJ.MapContestDataUI>();
            MapRegisterDataUI mapRegisterDataUI = registerObject.GetComponent<MapRegisterDataUI>();
            mapContestDataUI.SetRegisterData(mapRegisterDataUI.GetRegisterData());
            itemlist.Add(item);
            imageList.Add(item.GetComponent<Image>());
        }


    }

    public void ResetColor()
    {
        foreach (var image in imageList)
            image.color = Color.white;
    }

}
