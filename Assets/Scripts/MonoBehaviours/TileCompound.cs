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
            if(!tiles.Contains(tile))
                tiles.Add(tile);
        }

        public void Remove(MovableTile tile)
        {
            if(tiles.Contains(tile))
                tiles.Remove(tile);
        }
        
        public void Add(RigidbodyTile rigidbody)
        {
            if (!rigidbodies.Contains(rigidbody))
            {
                rigidbodies.Add(rigidbody);
                if (rigidbody.GetType() == typeof(Draggable))
                    Add((Draggable)rigidbody);
                else
                    Add((MovableTile)rigidbody);
            }
        }

        public void Remove(RigidbodyTile rigidbody)
        {
            if (rigidbodies.Contains(rigidbody))
            {
                rigidbodies.Remove(rigidbody);
                if(rigidbody.GetType() == typeof(Draggable))
                    Remove((Draggable)rigidbody);
                else
                    Remove((MovableTile)rigidbody);
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
            //dragables would not be added because rigidbodies add them
            //Add all other rigidbodies
            for(int i = 0; i < otherCompound.rigidbodies.Count; i++)
            {
                Add(otherCompound.rigidbodies[i]);
            }

            //Add all other movable tiles
            for(int i = 0; i < otherCompound.tiles.Count; i++)
            {
                Add(otherCompound.tiles[i]);
            }

        }
        #endregion
        #region Private Methods

        private void Add(Draggable draggable)
        {
            Debug.Log("Ading dragable");
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