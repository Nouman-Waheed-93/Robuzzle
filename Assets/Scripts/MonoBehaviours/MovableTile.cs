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

        protected TileCompound compound;

        #endregion
        #region properties

        public Sides AttachableSides { get => attachableSides; private set => attachableSides = value; }

        public TileCompound Compound { get => compound; }

        #endregion
        #region Events
        public event Action<MovableTile, Vector3Int> PositionChanged;
        #endregion
        #region Public Methods

        public virtual void Attach(RigidbodyTile attachTo)
        {
            transform.parent = attachTo.transform;
            AddInCompound(attachTo);
        }

        public void UpdatePosition()
        {
            //TODO::update the grid
            Vector3Int newPosition = Vector3Int.RoundToInt(transform.position);
            if (newPosition != Position)
            {
                PositionChanged(this, newPosition);
                PathFindingNode.transform.position = Position + Vector3Int.up;
            }
        }

        #endregion
        #region Protected Functions
        protected void AddInCompound(RigidbodyTile attachTo)
        {
            if (attachTo.Compound != null)
            {
                compound = attachTo.Compound;
                compound.Add(this);
            }
        }
        #endregion
    }
}