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
        public string title;
        public string Description;
        public RawImage mapImage;
        public string url;
        public int likes;
        public int views;
    }

    public MapRegisterScrollUI myMapRegisterScrollUIcp;
    private Image PanelImage;
    public MapRegisterData mapRegisterData;

    /// <summary>
    /// 현재 제목 및 설명
    /// </summary>
    public TMP_InputField TitleText;
    public TMP_InputField DescriptionText;
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

    //public void SetRegisterData(MapRegisterData _mapRegisterData)
    //{
    //    mapRegisterData = _mapRegisterData;
    //}

    public MapRegisterData GetRegisterData()
    {
        return mapRegisterData;
    }

    public void SetRegisterImageData(Sprite _spriteImage)
    {
        mapRegisterData.mapImage.texture = _spriteImage.texture;
    }

    public void SetTitleText()
    {
        mapRegisterData.title = TitleText.text;
    }

    public void SetDescriptionText()
    {
        mapRegisterData.Description = DescriptionText.text;
    }
}
