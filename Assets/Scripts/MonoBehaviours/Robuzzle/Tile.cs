using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class Tile : MonoBehaviour
    {
        #region variables
        [SerializeField]
        Vector3Int position; // position of the tile on the Grid
        [SerializeField]
        Sides enterableSides; // sides of the tile, an agent can step on it from
        [SerializeField]
        Sides lowerEnterableSides; //sides of the tile, an agent can enter from and exit to lower neighboring tiles
        [SerializeField]
        GameObject pathFindingNode; //empty gameObject that would be added in the navmesh to make this tile walkable
        #endregion
        #region properties
        public Vector3Int Position { get => position; set => position = value; }
        public Sides EnterableSides { get => enterableSides; private set => enterableSides = value; }
        public Sides LowerEnterableSides { get => lowerEnterableSides; private set => lowerEnterableSides = value; }
        public GameObject PathFindingNode { get => pathFindingNode; set => pathFindingNode = value; }
        #endregion
        #region Unity Callbacks
        private void OnDestroy()
        {
            DestroyImmediate(PathFindingNode);
        }
        #endregion
    }
}
