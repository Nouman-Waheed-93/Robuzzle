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
            base.Start();
            JoinAnchor();
        }

        #endregion
        #region Methods

        public override void Run(int direction, float speed)
        {

        }

        public override void MovePosition(Vector3 position, Draggable draggable)
        {
            Vector3 fromPosition = (draggable.transform.position - transform.position).normalized;
            Vector3 toPosition = (position - transform.position).normalized;
            Vector3 rotationVector = Vector3.Cross(fromPosition, toPosition).normalized;

            rigidbody.AddTorque(rotationVector, ForceMode.VelocityChange);
        }
        
        public override void AutomaticMove()
        {
            rigidbody.maxAngularVelocity = automaticSpeed;
            rigidbody.AddTorque(RobuzzleUtilities.GetSideVector(hingeSide), ForceMode.VelocityChange);
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