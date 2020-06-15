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
        #region Unity Callbacks

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        #endregion 
        #region Override Methods
        #endregion
        public override void Attach(RigidbodyTile attachTo)
        {
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = attachTo.rigidbody;
            AddInCompound(attachTo);
        }
        #region Public Methods
        public void SetCompound(TileCompound compound)
        {
            Debug.Log("Rigidbody tile got compound");
            this.compound = compound;
            compound.Add(this);
        }
        #endregion

    }
}