using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Robuzzle.LevelBuilding;

namespace Robuzzle
{
    public class EditorInput : AbstractInput
    {
        #region Variables
        LevelCreatorUI ui;
        LevelCreator levelCreator;
        bool clickStartedOnPanel;
        #endregion

        #region Unity Callbacks
        private void Start()
        {
            base.Start();
            ui = FindObjectOfType<LevelCreatorUI>();
            levelCreator = (LevelCreator)LevelCreator.singleton;
        }

        void Update()
        {
            GetPointerPosDelta();
            if (PanView()) { }
            else if (RotateView()) { }
            else if (ZoomView()) { }
            else if (ui.PointerOverUI()) { SlideTilePanel(); }
            else if (GetClick()) { }
            if (DragPanel()) { }
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

        private bool DragPanel()
        {
            if (clickStartedOnPanel && Input.GetMouseButton(0))
            {
                ui.SlideTileMenu(pointerDeltaPos.y * Time.deltaTime);
                return true;
            }
            else if(clickStartedOnPanel && Input.GetMouseButtonUp(0))
            {
                clickStartedOnPanel = false;
            }
            return false;
        }

        private bool SlideTilePanel()
        {
            if (Input.GetMouseButtonDown(0))
            {
                clickStartedOnPanel = true;
                return true;
            }
            return false;
        }
       #endregion

    }
}