using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public abstract class Mechanical : RigidbodyTile
    {
        #region variables

        [SerializeField]
        protected float automaticSpeed = 0;

        #endregion
        #region Unity Callbacks

        private void FixedUpdate()
        {
            if (Compound == null || !Compound.isDraggable())
            {
                AutomaticMove();
            }
        }

        #endregion
        #region AbstractMethods

        /*
         * Run the machine, negative direction value would run the machine in reverse
         * and positive direction value would run the machine in forward.
         */
        public abstract void Run(int direction, float speed);


        /*
         * Run the machine to move it in the position
         * */
        public abstract void MovePosition(Vector3 position, Draggable draggable);

        /*
         * Run the machine automatically with no player input involved. 
         * This will happen when no draggable is attached to the machine 
         */
        public abstract void AutomaticMove();

        #endregion
    }
}