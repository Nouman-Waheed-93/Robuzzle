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
        #endregion
        #region Unity Callbacks
        private void Start()
        {
            SetUpMotor();
        }
        #endregion
        #region Methods

        public override void Run(int direction, float speed)
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            Vector3 deltaRot = (Vector3)(RobuzzleUtilities.GetSideVector(hingeSide)) * -direction * speed * automaticSpeed;
            Quaternion qRotation = Quaternion.Euler(rotation) * Quaternion.Euler(deltaRot);
            rigidbody.MoveRotation(qRotation);
        }

        public override void AutomaticMove()
        {

        }

        private void SetUpMotor()
        {
            joint = GetComponent<HingeJoint>();
            motor = joint.motor;
            JoinAnchor();
            if (Compound == null || !Compound.isDraggable())
            {
                joint.useMotor = true;
                motor.force = automaticSpeed;
                motor.targetVelocity = automaticSpeed;
                joint.motor = motor;
            }
        }

        private void JoinAnchor()
        {
            Tile otherTile = RobuzzleGrid.singleton.GetTileAtPosition(Position + RobuzzleUtilities.GetSideVector(hingeSide));
            if (otherTile != null)
            {
                Rigidbody otherRB = otherTile.GetComponent<Rigidbody>();
                if (otherRB == null)
                {
                    otherRB = otherTile.gameObject.AddComponent<Rigidbody>();
                }
                otherRB.isKinematic = true;
                GetComponent<HingeJoint>().connectedBody = otherRB;
            }
        }
        #endregion
    }
}