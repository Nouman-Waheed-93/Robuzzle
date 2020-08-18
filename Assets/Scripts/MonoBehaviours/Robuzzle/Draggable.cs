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
            if (Compound.DraggableLastControlled == this)
            {
                if (dragging)
                    mechanical.MovePosition(targetPosition, this);
                else
                    mechanical.MoveToDiscretePosition(this);
            }
        }

        #endregion
        #region Methods

        public void StartDrag()
        {
            Debug.Log("Started drag");
            Compound.DraggableLastControlled = this;
            CompoundDragging(true);
            //    dragging = true;
        }

        public void Move(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }

        public void EndDrag()
        {
            Debug.Log("Ended drag");
            CompoundDragging(false);
//            dragging = false;
        }

        void CompoundDragging(bool dragging)
        {
            List<Draggable> otherDraggables = Compound.GetDraggables();
            for(int i = 0; i < otherDraggables.Count; i++)
            {
                otherDraggables[i].dragging = dragging;
            }
        }

        #endregion
    }
}