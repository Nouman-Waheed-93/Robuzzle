using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public abstract class Mechanical : MovableTile
    {
        #region variables

        [SerializeField]
        protected float automaticSpeed = 0;

        protected Rigidbody rb;

        #endregion
        #region AbstractMethods

        /*
         * Run the machine, negative direction value would run the machine in reverse
         * and positive direction value would run the machine in forward.
         */
        public abstract void Run(int direction, float speed);


        /*
         * Run the machine automatically with no player input involved. 
         * This will happen when no draggable is attached to the machine 
         */
        public abstract void AutomaticMove();

        #endregion
        #region ConcreteMethods

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if(compound == null || !compound.isDraggable())
            {
                AutomaticMove();
            }
        }
        
        #endregion
    }
}