using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Robuzzle.LevelBuilding
{
    public abstract class LevelLoader : MonoBehaviour
    {
        #region Variables
        public static string levelName = "HamsterWhil";

        public static Action LevelLoaded;

        protected LevelFileHandler fileHandler;
        #endregion
        // Start is called before the first frame update
        public void Init()
        {
            fileHandler = new LevelFileHandler();
        }

        public abstract void LoadLevel();
        
    }
}
