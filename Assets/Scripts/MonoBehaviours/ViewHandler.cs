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
        public float centeringSpeed = 10;
        Camera camera;
        Transform parent;
        Transform myTransform;
        Vector3 boundary;
        Vector3 parentCenterPosition;
        bool movingToCenter;
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
            parent.Rotate(0, dir, 0);
            if(ViewRotated != null)
                ViewRotated.Invoke(parent.rotation);
            if (!movingToCenter)
            {
                StartCoroutine("MoveToCenter");
            }
        }

        IEnumerator MoveToCenter()
        {
            movingToCenter = true;
            while(Vector3.Distance(parent.position, parentCenterPosition)> 0.1f)
            {
                parent.position = Vector3.Lerp(parent.position, parentCenterPosition, centeringSpeed * Time.deltaTime);
                yield return null;
            }
            movingToCenter = false;
        }

        public void Init(Vector3 levelSize)
        {
            boundary = levelSize + (Vector3.one * 3);
            Vector3 midPoint = levelSize * 0.5f;
            parentCenterPosition = new Vector3(midPoint.x, midPoint.y * 3, midPoint.z);
            parent.position = parentCenterPosition;
        }

        public void PanView(Vector2 dir)
        {
            Vector3 newPos = parent.position + parent.right * dir.x + parent.forward * dir.y;

            newPos.x = Mathf.Clamp(newPos.x, -boundary.x, boundary.x);
            newPos.y = Mathf.Clamp(newPos.y, -boundary.y, boundary.y);
            newPos.z = Mathf.Clamp(newPos.z, -boundary.z, boundary.z);

            parent.position = newPos;
        }

        public void ZoomView(float delta)
        {
            camera.fieldOfView += delta;
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFOV, maxFOV);
        }
        #endregion
    }
}
