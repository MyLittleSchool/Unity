using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapRegisterDataUI : MonoBehaviour, IPointerClickHandler
{
    [Serializable]
    public struct MapRegisterData
    {
        public TMP_Text title;
        public TMP_Text Description;
        public RawImage mapImage;
    }

    public MapRegisterScrollUI myMapRegisterScrollUIcp;
    private Image PanelImage;
    public MapRegisterData mapRegisterData;
    // Start is called before the first frame update
    void Start()
    {
        myMapRegisterScrollUIcp = GetComponentInParent<MapRegisterScrollUI>();
        PanelImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            ChangeColor();
        }
    }

    public void ChangeColor()
    {
        myMapRegisterScrollUIcp.SetRegisterGameObject(gameObject);
        myMapRegisterScrollUIcp.ResetColor();
        PanelImage.color = Color.yellow;
    }

    public void SetRegisterData(MapRegisterData _mapRegisterData)
    {
        mapRegisterData = _mapRegisterData;
    }

    public MapRegisterData GetRegisterData()
    {
        return mapRegisterData;
    }
}
