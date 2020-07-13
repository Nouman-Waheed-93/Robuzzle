using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle.LevelBuilding
{
    public class LevelLoader : MonoBehaviour
    {
        #region Variables
        public static string levelName = "chonda";
        RobuzzleFactory factory;
        LevelFileHandler fileHandler;
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            factory = GetComponent<RobuzzleFactory>();
            fileHandler = new LevelFileHandler();
            Invoke("LoadLevel", 0.1f);
        }
        
        public void LoadLevel()
        {
            if (levelName != "")
            {
                string levelString = fileHandler.LoadLevel(levelName);
                IntegerGrid levelData = JsonUtility.FromJson<IntegerGrid>(levelString);
                RobuzzleGrid.singleton.CreateGrid(levelData.size);
                foreach(PositionTile tileData in levelData.tiles)
                {
                    Debug.Log("lag gaye loray");
                    if (tileData.tile == 61)
                    {
                        Debug.Log("han ni ha");
                        factory.CreateMovableTile(tileData.position);
                    }
                }
            }
        }

    }
}
