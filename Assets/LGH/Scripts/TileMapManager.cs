using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GH
{
    public class TileMapManager : MonoBehaviour
    {
        public Tilemap tilemap;
        public Transform playerFrontTileTransform;
        private Vector3Int tilePosition;
        public Grid grid;
        public GameObject goldPref;
        public TileBase test;
        void Start()
        {
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                tilePosition = grid.WorldToCell(playerFrontTileTransform.position);
                if (!tilemap.HasTile(tilePosition + new Vector3Int(0, 1, 0)))
                {
                    tilemap.SetTile(tilePosition + new Vector3Int(0, 1, 0), test);
                    GameObject gold = Instantiate(goldPref, tilemap.transform);
                    gold.transform.position = tilePosition + new Vector3Int(0, 1, 0);
                    print(tilemap.HasTile(tilePosition + new Vector3Int(0, 1, 0)));

                }

            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                tilePosition = grid.WorldToCell(playerFrontTileTransform.position);
                //gold.transform.position = tilePosition + new Vector3Int(0, 1, 0);
            }
        }
    }
}