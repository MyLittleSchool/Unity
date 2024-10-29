using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRegisterScrollUI : ScrollUI
{
    public MapContestScrollUI myMapContestScrollUIcp;
    private GameObject registerObject;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        base.Start();
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
}
