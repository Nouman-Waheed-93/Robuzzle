using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Robuzzle.LevelBuilding;

namespace Robuzzle
{
    [CreateAssetMenu(fileName = "TileTypesData")]
    public class TileTypes : ScriptableObject
    {
        public TileData[] tiles;

        public Tile GetPrefab(TileInteger tile)
        {
            for(int i = 0; i < tiles.Length; i++)
            {
                if(tile == tiles[i].tileType)
                    return tiles[i].tilePrefab.GetComponent<Tile>();
            }
            return null;
        }
    }

    [System.Serializable]
    public struct TileData
    {
        public TileInteger tileType;
        public GameObject tilePrefab;
    }
}