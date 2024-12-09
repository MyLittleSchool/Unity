using GH;
using MJ;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;

using static OmokPiece;
using static UnityEngine.Rendering.DebugUI.Table;
using TMPro;

namespace MJ
{
    public class InputRocks : MonoBehaviour
    {
        public GameObject UIPanel;

        private GameObject Player;

        public static int ROCK_ROW = 10;
        public static int ROCK_COLUMN = 10;

        static float GRID_SIZE = 1.0f;

        static Vector3 PIVOT = new Vector3(-GRID_SIZE * ROCK_ROW / 2, -GRID_SIZE * ROCK_COLUMN / 2, 0.0f);

        private OmokPiece[,] rockDatas = new OmokPiece[ROCK_ROW, ROCK_COLUMN];

        private GameObject rockPrefabObject;

        private OmokPiece.ROCKCOLOR rockColor = ROCKCOLOR.WHITE;

        private OmokBoardChecker omocCheck;

        private PhotonView pv;
        // Start is called before the first frame update
        void Start()
        {
            pv = GetComponent<PhotonView>();
            omocCheck = GetComponent<OmokBoardChecker>();
            InitRocks();
        }

        // Update is called once per frame
        void Update()
        {
            if (gameInteractButton.GetInstance().GetButtonDown()) // Space
            {
                int[] Grid = CheckRockIdx();
                SendInputRock(Grid[0], Grid[1], (int)rockColor);
                if (omocCheck.OmocWin(rockDatas, Grid[0], Grid[1]))
                    SendOmocWin(Grid[0], Grid[1]);
            }
        }
        public void SendInputRock(int row, int col, int rockColor)
        {
            pv.RPC("InputRock", RpcTarget.AllBuffered, row, col, rockColor);
        }

        public void SendOmocWin(int row, int col)
        {
            pv.RPC("OmocWinPlayUI", RpcTarget.All, DataManager.instance.playerName, row, col);
        }

        [PunRPC]
        public void OmocWinPlayUI(string playerName, int row, int col)
        {
            omocCheck.OmocWin(rockDatas, row, col);

            FadeOutUI fadeOutUI = UIPanel.GetComponentInChildren<FadeOutUI>();
            fadeOutUI.GetComponentInChildren<TMP_Text>().text = playerName + " 오목 성공!";
            UIPanel.GetComponentInChildren<FadeOutUI>().FadeInOut(0.0f, 3.0f);
        }

        [PunRPC]
        public void InputRock(int row, int col, int rockColor)
        {
            if (row < 0 || col < 0 || row >= ROCK_ROW || col >= ROCK_COLUMN)
                return;
            rockDatas[row, col].SetColor((ROCKCOLOR)rockColor);
        }

        public int[] CheckRockIdx()
        {
            int[] Grid = new int[2] { -1, -1 };
            Vector3 PivotPos = transform.position + PIVOT;
            Vector3 PlayerPos = DataManager.instance.player.transform.position;

            Vector2 Gap = PlayerPos - PivotPos;

            if (Gap.x >= 0 && Gap.y >= 0)
            {
                Grid[0] = (int)(Gap.y / GRID_SIZE);
                Grid[1] = (int)(Gap.x / GRID_SIZE);
            }
            return Grid;
        }

        public void LoadRockData()
        {
            string data = "Prefab/RockObject";
            rockPrefabObject = Resources.Load<GameObject>(data);
        }

        public void InitRocks()
        {
            rockColor = ROCKCOLOR.WHITE;
            LoadRockData();
            for (int i = 0; i < ROCK_ROW; i++)
            {
                for (int j = 0; j < ROCK_COLUMN; j++)
                {
                    GameObject rockObject = Instantiate<GameObject>(rockPrefabObject, transform);
                    rockObject.transform.localPosition = new Vector3(j * GRID_SIZE + PIVOT.x + GRID_SIZE * 0.5f, i * GRID_SIZE + PIVOT.y + GRID_SIZE * 0.5f, 0.0f);

                    rockDatas[i, j] = rockObject.GetComponent<OmokPiece>();

                }
            }
        }

        public void SetRockColor(ROCKCOLOR _rockColor)
        {
            rockColor = _rockColor;
        }

        private IEnumerator ResetRocks(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            for (int i = 0; i < ROCK_ROW; i++)
            {
                for (int j = 0; j < ROCK_COLUMN; j++)
                    rockDatas[i, j].SetColor(ROCKCOLOR.NONE);
            }
            yield return null;
        }
    }
}