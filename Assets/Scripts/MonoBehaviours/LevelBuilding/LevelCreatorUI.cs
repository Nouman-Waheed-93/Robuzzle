using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle.LevelBuilding
{
    public class LevelCreatorUI : MonoBehaviour
    {
        #region Variables
        public Camera uiCam;
        public LayerMask uiLayerMask;
        public float menuMaxYPos;
        public float menuMinYPos;
        
        #endregion
        #region Public Methods
        public bool PointerOverUI()
        {
            if(Physics.Raycast(uiCam.ScreenPointToRay(Input.mousePosition), 1000, uiLayerMask))
            {
                return true;
            }
            return false;
        }

        public void SlideTileMenu(float dir)
        {
            Vector3 newPosition = transform.localPosition;
            newPosition.z = Mathf.Clamp(newPosition.z- dir, menuMinYPos, menuMaxYPos); 
            transform.localPosition = newPosition;
        }       
        #endregion
    }
}
