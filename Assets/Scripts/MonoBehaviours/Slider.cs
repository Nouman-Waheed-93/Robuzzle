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

        [SerializeField]
        Vector3 minBound;
        [SerializeField]
        Vector3 maxBound;

        int currAutomaticDir = 1; //current automatic movement direction
        #endregion
        #region Properties
        public Vector3 MinBound { get => minBound; set => minBound = value; }
        public Vector3 MaxBound { get => maxBound; set => maxBound = value; }
        #endregion
        #region Methods

        public override void Run(int direction, float speed)
        {
            rigidbody.MovePosition(transform.position + MovementAxis * Mathf.Sign(direction) * speed * Time.fixedDeltaTime);
        }

        public override void AutomaticMove()
        {
            //if the slider has reached the end of the grid
            //reverse the direction of the movement
            if(currAutomaticDir == 1 && transform.position == maxBound)
            {
                currAutomaticDir = -1;
            }
            else if(currAutomaticDir == -1 && transform.position == minBound)
            {
                currAutomaticDir = 1;
            }
            Run(currAutomaticDir, automaticSpeed);
        }

        #endregion
    }
}
