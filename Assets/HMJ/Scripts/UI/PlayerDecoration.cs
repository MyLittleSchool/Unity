using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static MJ.DecorationEnum;

namespace MJ
{
    public class PlayerDecoration : MonoBehaviour
    {
        /// <summary>
        /// Player 아바타 꾸밈 정보
        /// </summary>
        private int[] decorationData = new int[3];

        /// <summary>
        /// 파일에서 직접 로드할 이미지 데이터
        /// </summary>
        private Texture2D[,] loadDecorationImage = new Texture2D[4, 3];

        /// <summary>
        /// 꾸미기 데이터와 연결할 이미지
        /// </summary>
        public RawImage[] decorationImage = new RawImage[3];

        private DecorationEnum.DECORATION_DATA curDecorationPanel;

        public DecorationEnum.DECORATION_DATA CurDecorationPanel
        {
            get { return curDecorationPanel; }
            set { curDecorationPanel = value; }
        }

        private void Awake()
        {

        }

        private void Start()
        {
            LoadDecorationData();
        }

        /// <summary>
        /// Player 아바타 꾸밈 정보 Set
        /// </summary>
        /// <param name="_DATA"></param>
        /// <param name="idx"></param>
        public void SetPlayerDecorationData(DECORATION_DATA _DATA)
        {
            if (_DATA == DECORATION_DATA.DECORATION_DATA_END)
                return;

            curDecorationPanel = _DATA;
            for (int i = 0; i < 3; i++)
            {
                decorationImage[i].texture = loadDecorationImage[(int)_DATA, i];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_DATA"></param>
        /// <param name="idx"></param>
        public void SetPlayerSelectDecorationData(DECORATION_DATA _DATA, int idx)
        {
            decorationData[(int)_DATA] = idx;
        }

        /// <summary>
        /// /Image/PlayerDecoration 있는 파일의 Hair, Skin, Face, Cloth의 하위 png 파일을 모두 가져옴.
        /// </summary>
        public void LoadDecorationData()
        {
            string ImagePath = "Assets/Resources/Image/PlayerDecoration/";
            string[] DecorationData = { "Hair", "Skin", "Face", "Cloth" };
            // 폴더명
            for(int i = 0; i < DecorationData.Length; i++)
            {
                FileInfo[] fileInfos = FileManager.Instance.GetFileInfo(ImagePath + DecorationData[i], "png");
                for (int j = 0; j < fileInfos.Length; j++) // 특정 아이템 이미지 데이터 가져오기
                {
                    string data = "Image/PlayerDecoration/" + DecorationData[i] + "/" + Path.GetFileNameWithoutExtension(fileInfos[j].Name);
                    loadDecorationImage[i, j] = Resources.Load<Texture2D>(data);
                }
            }
        }


    }
}
