using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GH
{

    public class SetTile : MonoBehaviour
    {
        public Tilemap tilemap;
        public Transform playerFrontTileTransform;
        private Vector3Int tilePosition;
        private Grid grid;
        public GameObject setGameObject;
        public TileBase emptyTilebase;
        public GameObject tileLine;

        void Start()
        {
            SuchGrid();

        }

        void Update()
        {
            tilePosition = grid.WorldToCell(playerFrontTileTransform.position);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!tilemap.HasTile(tilePosition))
                {
                    tilemap.SetTile(tilePosition, emptyTilebase);
                    GameObject gold = Instantiate(setGameObject, tilemap.transform);
                    gold.transform.position = tilePosition;
                    print(tilemap.HasTile(tilePosition));
                }
            }
            tileLine.transform.position = tilePosition;
            if (Input.GetKeyDown(KeyCode.W))
            {
                tilePosition = grid.WorldToCell(playerFrontTileTransform.position);
                //gold.transform.position = tilePosition + new Vector3Int(0, 1, 0);
            }
        }

        public void SuchGrid()
        {
            grid = GameObject.Find("Grid").GetComponent<Grid>();
            tilemap = grid.transform.GetChild(0).GetComponent<Tilemap>();
        }
    }

}