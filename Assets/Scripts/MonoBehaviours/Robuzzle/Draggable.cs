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
        #region Unity Callbacks

        private void Start()
        {
            mechanical = Compound.GetMechanicals()[0];
        }

        private void FixedUpdate()
        {
            if (!dragging)
                return;
            Vector3 direction = targetPosition - transform.position;
          //  mechanical.MovePosition(direction);
            direction.Normalize();
            if (direction.magnitude != 0)
                mechanical.Run(Mathf.RoundToInt((direction.x + direction.y + direction.z)), 1);
            //            rigidbody.AddForce(-transform.forward * 10, ForceMode.Acceleration);
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