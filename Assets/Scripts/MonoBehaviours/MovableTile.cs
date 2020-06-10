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

        #endregion
        #region Events
        public event Action<MovableTile, Vector3Int> PositionChanged;
        #endregion
        #region Methods

        public virtual void Attach(TileCompound attachTo)
        {
            transform.parent = attachTo.transform;
            compound = attachTo;
        }

        public void UpdatePosition()
        {
            //TODO::update the grid
            Vector3Int newPosition = Vector3Int.RoundToInt(transform.position);
            PositionChanged(this, newPosition);
            PathFindingNode.transform.position = Position + Vector3Int.up;
        }

        #endregion
    }
}