using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    [RequireComponent (typeof(Rigidbody))]
    public class RigidbodyTile : MovableTile
    {
        #region Variables
        protected Rigidbody rigidbody;
        #endregion
        #region Properties
        public Rigidbody GetRigidbody {
            get
            {
                if (rigidbody == null)
                    rigidbody = GetComponent<Rigidbody>();
                return rigidbody;
            }
        }
        #endregion
        #region Unity Callbacks

        protected void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        #endregion 
        #region Override Methods

        public override void Attach(RigidbodyTile attachTo)
        {
            Debug.Log("Joint creating");
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = attachTo.GetRigidbody;
            Debug.Log("joint has a rigidbody " + attachTo.rigidbody != null);
            AddInCompound(attachTo.Compound);
        }

        #endregion
    }
}