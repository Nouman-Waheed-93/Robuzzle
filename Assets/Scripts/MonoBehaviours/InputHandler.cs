using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle {
    public interface IInputHandler
    {
        bool IsTapped();
        bool IsSingleDragging();
        bool IsDoubleDragging();
        bool IsPinchZooming();
        Vector3 GetPointerPosDelta();
    }

    public class TouchInputHandler : IInputHandler
    {
        #region Variables
        float touchHoldTimer;
        Vector3 pointerDelta;
        #endregion

        public bool IsTapped()
        {
            if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchHoldTimer = 0;
                return touchHoldTimer < 0.2f;
            }
            return false;
        }

        public bool IsSingleDragging()
        {
            if (Input.touchCount == 1)
            {
                if (touchHoldTimer > 0.2f)
                {
                    pointerDelta = Input.GetTouch(0).deltaPosition;
                    return true;
                }
                else
                {
                    touchHoldTimer += Time.deltaTime;
                }
            }
            return false;
        }

        public bool IsDoubleDragging()
        {
            if(Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if(Vector3.Dot(touch1.deltaPosition, touch2.deltaPosition) > 0.5f
                    && Mathf.Abs(touch1.deltaPosition.sqrMagnitude - touch2.deltaPosition.sqrMagnitude) < 1 )
                {
                    pointerDelta = (touch1.deltaPosition + touch2.deltaPosition) * 0.5f;
                    return true;
                }
            }
            return false;
        }

        public bool IsPinchZooming()
        {
            if(Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if(Vector3.Dot(touch1.deltaPosition, touch2.deltaPosition) < -0.5f)
                {
                    Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                    Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

                    Vector2 prevAverage = touch1PrevPos - touch2PrevPos;
                    Vector2 currentAverage = touch1.position - touch2.position;
                    pointerDelta = currentAverage - prevAverage;
                    return true;
                }
            }
            return false;
        }

        public Vector3 GetPointerPosDelta()
        {
            return pointerDelta;
        }

    }

    public class MouseInputHandler : IInputHandler
    {
        #region Variables
        Vector3 lastMousePosition;
        Vector3 positionDelta;
        #endregion
        
        #region Methods
        public Vector3 GetPointerPosDelta()
        {
            positionDelta = lastMousePosition - Input.mousePosition;
            lastMousePosition = Input.mousePosition;
            return positionDelta;
        }

        public bool IsDoubleDragging()
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                return true;
            }
            return false;
        }

        public bool IsPinchZooming()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return true;
            }
            return false;
        }

        public bool IsSingleDragging()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                return true;
            }
            return false;
        }

        public bool IsTapped()
        {
            return Input.GetMouseButtonDown(0);
        }
        #endregion
    }

}