using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class Mechanical : MovableTile
    {
        #region Methods

        public override void Move(Vector3Int newPosition)
        {
            base.Move(newPosition);
        }

        private void AutomaticMove()
        {

        }

        #endregion
    }
}