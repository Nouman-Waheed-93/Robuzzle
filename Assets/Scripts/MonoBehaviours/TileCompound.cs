using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class TileCompound : MonoBehaviour
    {
        #region variables

        List<MovableTile> tiles = new List<MovableTile>();
        List<Draggable> draggables = new List<Draggable>();
        
        #endregion
        #region Methods

        public void Add(MovableTile tile)
        {
            if(!tiles.Contains(tile))
                tiles.Add(tile);
        }

        public void Remove(MovableTile tile)
        {
            if(tiles.Contains(tile))
                tiles.Remove(tile);
        }

        public bool isDraggable()
        {
            return draggables.Count > 0;
        }

        #endregion
    }
}