using MJ;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static ROCK;

namespace MJ
{
    public class InputRocks : MonoBehaviour
    {
        private GameObject Player;

        public static int ROCK_ROW = 10;
        public static int ROCK_COLUMN = 10;

        static float GRID_SIZE = 0.5f;

        static Vector3 PIVOT = new Vector3(-GRID_SIZE * ROCK_ROW / 2, -GRID_SIZE * ROCK_COLUMN / 2, 0.0f);

        private ROCK[] rockDatas = new ROCK[ROCK_ROW * ROCK_COLUMN];

        private GameObject rockPrefabObject;

        private ROCK.ROCKCOLOR rockColor = ROCKCOLOR.WHITE;
        // Start is called before the first frame update
        void Start()
        {
            Player = GameObject.Find("Player");
            InitRocks();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Space
            {
                InputRock(CheckRockIdx(), rockColor);
            }
        }

        public void InputRock(int idx, ROCKCOLOR rockColor)
        {
            if (idx < 0 || rockDatas.Length <= idx)
                return;
            rockDatas[idx].SetColor(rockColor);
        }

        public int CheckRockIdx()
        {
            Vector3 PivotPos = transform.position + PIVOT;
            Vector3 PlayerPos = Player.transform.position;

            Vector2 Gap = PlayerPos - PivotPos;

            if (Gap.x >= 0 && Gap.y >= 0)
            {
                return (int)(Gap.y / GRID_SIZE) * ROCK_ROW + (int)(Gap.x / GRID_SIZE);
            }
            else
                return -1;
        }

        public void LoadRockData()
        {
            string data = "Prefab/RockObject";
            rockPrefabObject = Resources.Load<GameObject>(data);
        }

        public void InitRocks()
        {
            LoadRockData();
            for (int i = 0; i < ROCK_ROW * ROCK_COLUMN; i++)
            {
                GameObject rockObject = Instantiate<GameObject>(rockPrefabObject, transform);
                rockObject.transform.position = new Vector3((i % ROCK_COLUMN) * GRID_SIZE + PIVOT.x + GRID_SIZE * 0.5f, (i / ROCK_ROW) * GRID_SIZE + PIVOT.y + GRID_SIZE * 0.5f, 0.0f);

                rockDatas[i] = rockObject.GetComponent<ROCK>();
            }
        }

        public void SetRockColor(ROCKCOLOR _rockColor)
        {
            rockColor = _rockColor;
        }

    }
}