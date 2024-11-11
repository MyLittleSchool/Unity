using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static MJ.MapContestDataUI;

namespace MJ
{
    public class MapContestDataUI : MonoBehaviour, IPointerClickHandler
    {
        [Serializable]
        public struct MapContestData
        {
            public TMP_Text title;
            public TMP_Text Description;
            public RawImage mapImage;
            public List<ObjectContestInfo> furnitureList;
        }

        public MapRegisterScrollUI myMapRegisterScrollUIcp;
        private Image PanelImage;
        public MapContestData mapContestData;

        public Button button;
        // Start is called before the first frame update
        void Start()
        {
            PanelImage = GetComponent<Image>();
        }

        public void SettingButton()
        {
            button = GetComponent<Button>();

            //button.onClick.AddListener();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
            }
        }

        public void SetRegisterData(MapRegisterDataUI.MapRegisterData _mapRegisterData)
        {
            mapContestData.title.text = _mapRegisterData.title;
            mapContestData.Description.text = _mapRegisterData.Description;
            if (_mapRegisterData.mapImage)
                mapContestData.mapImage.texture = _mapRegisterData.mapImage.texture;
        }

        public void SetRegisterData(MapRegisterDataUI.MapRegisterData _mapRegisterData, Texture2D _texture2D, List<ObjectContestInfo> _furnitureList)
        {
            mapContestData.title.text = _mapRegisterData.title;
            mapContestData.Description.text = _mapRegisterData.Description;
            mapContestData.mapImage.texture = _texture2D;
            mapContestData.furnitureList = _furnitureList;
        }

    }
}
