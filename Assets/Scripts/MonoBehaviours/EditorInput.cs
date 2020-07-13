using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Robuzzle.LevelBuilding;

namespace Robuzzle
{
    public class EditorInput : MonoBehaviour
    {
        #region Variables
        LevelCreatorUI ui;
        ViewHandler viewHandler;
        LevelCreator levelCreator;

        Vector3 LastMousePosition;
        #endregion

        #region Unity Callbacks
        private void Start()
        {
            ui = FindObjectOfType<LevelCreatorUI>();
            viewHandler = (ViewHandler)ViewHandler.singleton;
            levelCreator = (LevelCreator)LevelCreator.singleton;
        }

        void Update()
        {
            if (PanView()) { }
            else if (RotateView()) { }
            else if (ZoomView()) { }
            else if (ui.PointerOverUI()) { }
            else if (GetClick()) { }
        }
        #endregion
        #region Private Methods
        private bool GetClick()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return false;
            if (Input.GetMouseButtonDown(0))
            {
                ((LevelCreator)LevelCreator.singleton).PlaceTile(RobuzzleUtilities.GetPositionUnderCursor());
                return true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ((LevelCreator)LevelCreator.singleton).DeleteTile(RobuzzleUtilities.GetPositionUnderCursor());
                return true;
            }
            return false;
        }

        //return true if view is panning
        private bool PanView()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                LastMousePosition = Input.mousePosition;
                return true;
            }
            else if (Input.GetKey(KeyCode.LeftAlt))
            {
                viewHandler.PanView(GetMousePositionDelta() * Time.deltaTime);
                return true;
            }
            return false;
        }

        private bool RotateView()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                LastMousePosition = Input.mousePosition;
                return true;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                viewHandler.RotateView(GetMousePositionDelta().x * Time.deltaTime);
                return true;
            }
            return false;
        }

        private bool ZoomView()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                LastMousePosition = Input.mousePosition;
                return true;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                viewHandler.ZoomView(GetMousePositionDelta().y * Time.deltaTime);
                return true;
            }
            return false;
        }

        private Vector3 GetMousePositionDelta()
        {
            Vector3 delta = LastMousePosition - Input.mousePosition;
            LastMousePosition = Input.mousePosition;
            return delta;
        }
        #endregion

    }
}