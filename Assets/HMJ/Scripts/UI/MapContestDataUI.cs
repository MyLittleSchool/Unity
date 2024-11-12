using GH;
using Photon.Pun;
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
        public struct MapContestDrawData
        {
            public TMP_Text title;
            public TMP_Text Description;
            public RawImage mapImage;
            public TMP_Text likes;
            public TMP_Text views;
            public List<ObjectContestInfo> furnitureList;
        }

        public MapRegisterScrollUI myMapRegisterScrollUIcp;
        private Image PanelImage;
        public MapContestDrawData mapContestDrawData;

        public Button viewsButton;
        public Button likesButton;


        MapContestData mapContestData;
        // Start is called before the first frame update
        void Start()
        {
            PanelImage = GetComponent<Image>();
            SettingButton();
        }

        public void SettingButton()
        {
            viewsButton.onClick.AddListener(ClickViewButton);
            likesButton.onClick.AddListener(ClickLikeButton);

            viewsButton.onClick.AddListener(SettingUIData);
            likesButton.onClick.AddListener(SettingUIData);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void IntoMapcontestRoom()
        {
            MapContestLoader.GetInstance().loadfurnitureList = mapContestData.furnitureList;
            SceneMgr.instance.MapContestMapIn(DataManager.instance.playerName + "의 " + mapContestData.title);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
            }
        }

        public void SetRegisterData(MapContestData _mapContestData, Texture2D _texture2D)
        {
            mapContestData = _mapContestData;

            mapContestDrawData.title.text = _mapContestData.title;
            mapContestDrawData.Description.text = _mapContestData.description;
            mapContestDrawData.mapImage.texture = _texture2D;
            mapContestDrawData.furnitureList = _mapContestData.furnitureList;

            SettingUIData();
        }

        public void ClickLikeButton()
        {
            mapContestData.likeCount++;
            //post 통신
            MapContestLoader.GetInstance().MapContestEditSave(mapContestData);
        }

        public void ClickViewButton()
        {
            mapContestData.viewCount++;

            //post 통신
            MapContestLoader.GetInstance().MapContestEditSave(mapContestData);
            IntoMapcontestRoom();
        }

        private void OnEnable()
        {
            SettingUIData();
        }

        public void SettingUIData()
        {
            mapContestDrawData.likes.text = mapContestData.likeCount.ToString();
            mapContestDrawData.views.text = mapContestData.viewCount.ToString();
        }
    }
}
