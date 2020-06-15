using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class Slider : Mechanical
    {
        #region Variables

        [SerializeField]
        Vector3 MovementAxis;
        
        #endregion
        #region Methods

        public override void Run(int direction, float speed)
        {
            rigidbody.MovePosition(transform.position + MovementAxis * Mathf.Sign(direction) * speed);
        }

        public override void AutomaticMove()
        {
            //if the slider has reached the end of the grid
            //reverse the direction of the movement
        }

        #endregion
    }
}
