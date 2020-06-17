using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class TileCompound
    {
        #region variables

        List<MovableTile> tiles = new List<MovableTile>();
        List<Draggable> draggables = new List<Draggable>();
        List<RigidbodyTile> rigidbodies = new List<RigidbodyTile>();

        #endregion
        #region Methods
        public void Add(MovableTile tile)
        {
            if (!tiles.Contains(tile))
            {
                tiles.Add(tile);
                if (tile.GetType() == typeof(RigidbodyTile) || 
                    tile.GetType().IsSubclassOf(typeof(RigidbodyTile)))
                    Add((RigidbodyTile)tile);
            }
        }

        public void Remove(MovableTile tile)
        {
            if (tiles.Contains(tile))
            {
                tiles.Remove(tile);
                if (tile.GetType() == typeof(RigidbodyTile) ||
                    tile.GetType().IsSubclassOf(typeof(RigidbodyTile)))
                    Remove((RigidbodyTile)tile);
            }
        }
        
        private void Add(RigidbodyTile rigidbody)
        {
            if (!rigidbodies.Contains(rigidbody))
            {
                rigidbodies.Add(rigidbody);
                if (rigidbody.GetType() == typeof(Draggable))
                    Add((Draggable)rigidbody);
            }
        }

        private void Remove(RigidbodyTile rigidbody)
        {
            if (rigidbodies.Contains(rigidbody))
            {
                rigidbodies.Remove(rigidbody);
                if(rigidbody.GetType() == typeof(Draggable))
                    Remove((Draggable)rigidbody);
            }
        }

        public bool isDraggable()
        {
            return draggables.Count > 0;
        }
        
        public int GetTotalSize()
        {
            return tiles.Count + draggables.Count + rigidbodies.Count;
        }

        public void Integrate(TileCompound otherCompound)
        {
            for (int i = 0; i < otherCompound.tiles.Count; i++)
            {
            //    Debug.Log("Integrating " + otherCompound.tiles[i] tile from other compound");
                otherCompound.tiles[i].AddInCompound(this);
            }

        }
        #endregion
        #region Private Methods

        private void Add(Draggable draggable)
        {
//            Debug.Log("Ading dragable");
            if (!draggables.Contains(draggable))
            {
                draggables.Add(draggable);
                Add((MovableTile)draggable);
            }
        }

        private void Remove(Draggable draggable)
        {
            if (draggables.Contains(draggable))
            {
                draggables.Remove(draggable);
                Remove((MovableTile)draggable);
            }
        }

        #endregion
    }
}