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

        TileCompound compound;

        #endregion
        #region properties
        public Sides AttachableSides { get => attachableSides; private set => attachableSides = value; }
        #endregion
        #region Methods

        public virtual void Attach(MovableTile attachTo)
        {

        }

        public virtual void Move(Vector3Int newPosition)
        {
            //TODO::update the grid
            Position = newPosition;
        }

        #endregion
    }
}