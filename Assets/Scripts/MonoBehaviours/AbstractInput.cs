using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class AbstractInput : MonoBehaviour
    {
        #region Variables
        ViewHandler viewHandler;
        protected IInputHandler inputHandler;
        protected Vector3 pointerDeltaPos;
        #endregion
        #region Unity Callbacks
        protected void Start()
        {
            viewHandler = ViewHandler.singleton;

            if (Input.touchSupported)
            {
                inputHandler = new TouchInputHandler();
            }
            else
            {
                inputHandler = new MouseInputHandler();
            }
        }
        #endregion

        public bool PanView()
        {
            if (inputHandler.IsDoubleDragging())
            {
                viewHandler.PanView(pointerDeltaPos * Time.deltaTime);
                return true;
            }
            return false;
        }

        public bool RotateView()
        {
            if (inputHandler.IsSingleDragging())
            {
                viewHandler.RotateView(pointerDeltaPos.x);
                return true;
            }
            return false;
        }

        public bool ZoomView()
        {
            if (inputHandler.IsPinchZooming())
            {
                viewHandler.ZoomView(pointerDeltaPos.y + pointerDeltaPos.x * Time.deltaTime);
                return true;
            }
            return false;
        }

        public void GetPointerPosDelta()
        {
            pointerDeltaPos = inputHandler.GetPointerPosDelta();
        }
    }
}
