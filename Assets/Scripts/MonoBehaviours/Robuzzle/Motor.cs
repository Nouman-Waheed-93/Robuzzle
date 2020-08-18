using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    [RequireComponent(typeof(HingeJoint))]
    public class Motor : Mechanical
    {
        #region Variables
        [SerializeField]
        private SideName hingeSide;

        private HingeJoint joint;
        private JointMotor motor;
        private Vector3 hingeVector;
        private Vector3 perpendicularVector; // perpendicular to the hinge Vector
        private PIDController motorPID;
        #endregion
        #region Unity Callbacks

        private void Start()
        {
            base.Start();
            motorPID = new PIDController();
            hingeVector = RobuzzleUtilities.GetSideVector(hingeSide);
            perpendicularVector = RobuzzleUtilities.GetPerpendicularSideVector(hingeSide);
            JoinAnchor();
        }

        #endregion
        #region Methods
        
        public override void MovePosition(Vector3 position, Draggable draggable)
        {
            Vector3 fromPosition = (draggable.transform.position - transform.position).normalized;
            Vector3 toPosition = (position - transform.position).normalized;

            float fromHingeDot = Vector3.Dot(fromPosition, hingeVector); //we calculate dot product between hingeVector and fromPosition and...
            fromPosition = fromPosition - (hingeVector * fromHingeDot); //...subtract it from the from Position, so that fromPosition is a perpendicular rotation to hinge
            float toHingeDot = Vector3.Dot(toPosition, hingeVector); // same thing as above two lines
            toPosition = toPosition - (hingeVector * toHingeDot);

            Debug.DrawRay(transform.position, fromPosition, Color.red);
            Debug.DrawRay(transform.position, toPosition, Color.cyan);

            float fromAngle = Vector3.SignedAngle(fromPosition, perpendicularVector, -hingeVector);
            float toAngle = Vector3.SignedAngle(toPosition, perpendicularVector, -hingeVector);
            
            float Amt = motorPID.SeekAngle(toAngle, fromAngle);
            rigidbody.AddTorque(hingeVector * Amt, ForceMode.VelocityChange);
        }

        public override void MoveToDiscretePosition(Draggable draggable)
        {
            Vector3 draggableDir = (draggable.transform.position - transform.position).normalized;
            float draggableDot = Vector3.Dot(draggableDir, hingeVector);
            draggableDir = draggableDir - (hingeVector * draggableDot);

            Vector3[] Sides = new Vector3[6];
            Sides[0] = Vector3.left;
            Sides[1] = Vector3.right;
            Sides[2] = Vector3.down;
            Sides[3] = Vector3.up;
            Sides[4] = Vector3.back;
            Sides[5] = Vector3.forward;

            float[] Angles = new float[6];
            int smallestAngleIndex = 0;
            for(int i = 0; i < 6; i++)
            {
                Angles[i] = Vector3.Angle(draggableDir, Sides[i]);
                if(i == 1)
                {
                    if (Angles[i] < Angles[i - 1])
                        smallestAngleIndex = i;
                    else
                        smallestAngleIndex = i - 1;
                }
                if(i > 1)
                {
                    if (Angles[i] < Angles[smallestAngleIndex])
                        smallestAngleIndex = i;
                }
            }
            MovePosition(transform.position + Sides[smallestAngleIndex], draggable);
        }

        public override void AutomaticMove()
        {
            rigidbody.maxAngularVelocity = automaticSpeed;
            rigidbody.AddTorque(hingeVector, ForceMode.VelocityChange);
        }
        
        private void JoinAnchor()
        {
            Tile otherTile = RobuzzleGrid.singleton.GetTileAtPosition(Position + Vector3Int.RoundToInt(hingeVector));
            if (otherTile != null)
            {
                Rigidbody otherRB = otherTile.GetComponent<Rigidbody>();
                if (otherRB == null)
                {
                    otherRB = otherTile.gameObject.AddComponent<Rigidbody>();
                    otherRB.isKinematic = true;
                }
                GetComponent<HingeJoint>().connectedBody = otherRB;
            }
        }
        #endregion
    }
}