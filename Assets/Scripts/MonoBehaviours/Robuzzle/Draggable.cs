using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class Draggable : RigidbodyTile
    {
        #region Variables
        Mechanical mechanical;
        Vector3 targetPosition;
        bool dragging;
        #endregion
        //debug
        Transform debugTarget;

        #region Unity Callbacks

        private void Start()
        {
            mechanical = Compound.GetMechanicals()[0];
            debugTarget = (new GameObject("Debug" + name)).transform;
        }

        private void FixedUpdate()
        {
            if (!dragging)
                return;

            mechanical.MovePosition(targetPosition, this);
        }

        #endregion
        #region Methods

        public void StartDrag()
        {
            Debug.Log("Started drag");
            dragging = true;
        }

        public void Move(Vector3 targetPosition)
        {
            debugTarget.position = targetPosition;
            this.targetPosition = targetPosition;
        }

        public void EndDrag()
        {
            Debug.Log("Ended drag");
            dragging = false;
        }

        #endregion
    }
}