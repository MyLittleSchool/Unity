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
        public GameObject tileLine;


        void Start()
        {

        }
        void Update()
        {
            tilePosition = grid.WorldToCell(playerFrontTileTransform.position);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!tilemap.HasTile(tilePosition ))
                {
                    tilemap.SetTile(tilePosition , test);
                    GameObject gold = Instantiate(goldPref, tilemap.transform);
                    gold.transform.position = tilePosition ;
                    print(tilemap.HasTile(tilePosition));
                }
            }
            tileLine.transform.position = tilePosition ;
            if (Input.GetKeyDown(KeyCode.W))
            {
                tilePosition = grid.WorldToCell(playerFrontTileTransform.position);
                //gold.transform.position = tilePosition + new Vector3Int(0, 1, 0);
            }
        }
    }
}