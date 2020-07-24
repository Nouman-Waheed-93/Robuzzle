using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class Draggable : RigidbodyTile
    {
        #region Variables
        Mechanical mechanical;
        #endregion
        #region Unity Callbacks

        private void Start()
        {
            mechanical = Compound.GetMechanicals()[0];
        }
        
        #endregion
        #region Methods
        
        public void StartDrag()
        {
            Debug.Log("Started drag");
        }

        public void Move(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            if(direction.magnitude != 0)
              mechanical.Run(Mathf.RoundToInt(direction.x + direction.y + direction.z), 1);
//            rigidbody.AddForce(-transform.forward * 10, ForceMode.Acceleration);
        }

        public void EndDrag()
        {
            Debug.Log("Ended drag");
        }

        #endregion
    }
}