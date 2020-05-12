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
        #endregion
        #region properties
        public Vector3Int Position { get => position; set => position = value; }
        public Sides EnterableSides { get => enterableSides; private set => enterableSides = value; }
        #endregion
    }
}
