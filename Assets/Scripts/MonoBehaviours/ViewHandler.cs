using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Robuzzle
{
    public class ViewHandler : SingletonMonoBehaviour<ViewHandler>
    {
        #region Variables
        public float minFOV = 30;
        public float maxFOV = 60;
        Camera camera;
        Transform parent;
        Transform myTransform;
        #endregion
        #region Events
        public event Action<Quaternion> ViewRotated;
        #endregion
        #region Unity Callbacks
        private void Start()
        {
            camera = GetComponent<Camera>();
            parent = transform.parent;
            myTransform = transform;
        }
        #endregion
        #region Public Methods
        public void RotateView(float dir)
        {
            //TODO : Create an event and notify here
            parent.Rotate(0, dir, 0);
            ViewRotated.Invoke(parent.rotation);
        }

        public void SetView(Vector3 position)
        {
            parent.position = new Vector3(position.x, parent.position.y, position.z);
        }

        public void PanView(Vector2 dir)
        {
            myTransform.localPosition = new Vector3(myTransform.localPosition.x + dir.x, myTransform.localPosition.y, myTransform.localPosition.z + dir.y);
        }

        public void ZoomView(float delta)
        {
            camera.fieldOfView += delta;
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFOV, maxFOV);
        }
        #endregion
    }
}
