using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle {
    public interface IInputHandler
    {
        bool BtnDown();
        bool BtnUp();
        bool IsTapped();
        bool IsSingleDragging();
        bool IsDoubleDragging();
        bool IsPinchZooming();
        Vector3 GetPointerPosDelta();
        Vector3 GetPointerPosition();
    }

    public class TouchInputHandler : IInputHandler
    {
        #region Variables
        float touchHoldTimer;
        Vector3 pointerDelta;
        #endregion

        public Vector3 GetPointerPosition()
        {
            Vector3 returnVal = Vector3.zero;
            if(Input.touchCount == 1)
                returnVal = Input.GetTouch(0).position;
            return returnVal;
        }

        public bool BtnDown()
        {
            if (Input.touchCount == 1)
                return Input.GetTouch(0).phase == TouchPhase.Began;
            return false;
        }

        public bool BtnUp()
        {
            if(Input.touchCount == 1)
                return Input.GetTouch(0).phase == TouchPhase.Ended;
            return false;
        }

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
        float clickHold;
        Vector3 lastMousePosition;
        Vector3 positionDelta;
        #endregion
        
        #region Methods

        public Vector3 GetPointerPosition()
        {
            return Input.mousePosition;
        }

        public bool BtnDown()
        {
            return Input.GetMouseButtonDown(0);
        }

        public bool BtnUp()
        {
            return Input.GetMouseButtonUp(0);
        }

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
            if (Input.GetMouseButton(0))
            {
                clickHold += Time.deltaTime;
            }

            if (Input.GetMouseButtonUp(0))
            {
                bool tap = clickHold < 0.2f;
                clickHold = 0;
                return tap;
            }
            return false;
        }
        #endregion
    }

}