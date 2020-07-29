using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class Slider : Mechanical
    {
        #region Variables

        [SerializeField]
        float EndStayTime = 1; //the time for which the slider will stop before changing direction when it reaches its end

        [SerializeField]
        Vector3 MovementAxis;

        [SerializeField]
        Vector3 minBound;
        [SerializeField]
        Vector3 maxBound;

        float endTimeCounter;
        int currAutomaticDir = 1; //current automatic movement direction
        #endregion
        #region Properties
        public Vector3 MinBound { get => minBound; set => minBound = value; }
        public Vector3 MaxBound { get => maxBound; set => maxBound = value; }
        #endregion
        #region Methods

        public override void Run(int direction, float speed)
        {
            rigidbody.AddRelativeForce(MovementAxis * Mathf.Sign(direction) * speed, ForceMode.VelocityChange);
        }

        public override void MovePosition(Vector3 position, Draggable draggable)
        {
            Vector3 toPosition = position - transform.position;
            Debug.DrawRay(transform.position, toPosition, Color.red);
            rigidbody.AddRelativeForce(toPosition, ForceMode.VelocityChange);
        }

        public override void AutomaticMove()
        {
            if (maxBound == minBound)
                return;
            //if the slider has reached the end of the grid
            //reverse the direction of the movement

            Debug.Log(Vector3.Dot(transform.position, MovementAxis) + " the fuckin dot " + Vector3.Dot(MaxBound, MovementAxis) + " the feckd dot " + currAutomaticDir);

            float sliderPosition = Vector3.Dot(transform.position, MovementAxis);
            float maxPosition = Vector3.Dot(MaxBound, MovementAxis);
            float maxDifference = Mathf.Abs(sliderPosition - maxPosition);

            float minPosition = Vector3.Dot(MinBound, MovementAxis);
            float minDifference = Mathf.Abs(sliderPosition - minPosition);

            if (currAutomaticDir == 1 && maxDifference < 0.02f)
            {
                endTimeCounter += Time.deltaTime;
                if (endTimeCounter >= EndStayTime)
                {
                    endTimeCounter = 0;
                    currAutomaticDir = -1;
                }
            }
            else if(currAutomaticDir == -1 && minDifference < 0.02f)
            {
                endTimeCounter += Time.deltaTime;
                if (endTimeCounter >= EndStayTime)
                {
                    endTimeCounter = 0;
                    currAutomaticDir = 1;
                }
            }
            Run(currAutomaticDir, automaticSpeed);
        }

        #endregion
    }
}
