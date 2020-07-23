using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class GameInput : AbstractInput
    {
        #region Variables
        NavAgent player;
        Draggable selectedDraggable;
        #endregion

        #region Unity Callbacks

        private void Start()
        {
            base.Start();
        }

        void Update()
        {
            GetPointerPosDelta();
            if (PanView()) { }
            else if (RotateView()) { }
            else if (ZoomView()) { }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}