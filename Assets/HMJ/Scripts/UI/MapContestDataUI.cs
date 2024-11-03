using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        }

        public MapRegisterScrollUI myMapRegisterScrollUIcp;
        private Image PanelImage;
        public MapContestData mapContestData;
        // Start is called before the first frame update
        void Start()
        {
            PanelImage = GetComponent<Image>();
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
            mapContestData.mapImage.texture = _mapRegisterData.mapImage.texture;
        }

    }
}
