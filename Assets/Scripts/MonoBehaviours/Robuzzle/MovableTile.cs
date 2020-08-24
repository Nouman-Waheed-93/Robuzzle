using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class MovableTile : Tile
    {
        #region variables

        [SerializeField]
        Sides attachableSides; // the sides that can be attached to another movable object, so that both act like one object

        private TileCompound compound;

        #endregion
        #region properties

        public Sides AttachableSides { get => attachableSides; private set => attachableSides = value; }

        public TileCompound Compound { get => compound; }

        #endregion
        #region Events
        public event Action<MovableTile, Vector3Int> PositionChanged;
        #endregion
        #region Unity Callbacks

        private void Update()
        {
            UpdatePosition();
        }

        #endregion
        #region Public Methods

        public virtual void Attach(RigidbodyTile attachTo)
        {
            transform.parent = attachTo.transform;
            AddInCompound(attachTo.Compound);
        }
        
        public void AddInCompound(TileCompound attachTo)
        {
            compound = attachTo;
            compound.Add(this);
        }
        #endregion
        #region Private Methods

        private void UpdatePosition()
        {
            Vector3Int newPosition = Vector3Int.RoundToInt(transform.position);
            if (newPosition != Position)
            {
                PositionChanged(this, newPosition);
            }
            if (PathFindingNode) //Rails Do not have pathfinding nodes, but their position has to be updated
                PathFindingNode.transform.position = transform.position + Vector3.up;
        }

        #endregion
    }
}