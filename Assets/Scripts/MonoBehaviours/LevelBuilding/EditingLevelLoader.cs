using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle.LevelBuilding
{
    public class EditingLevelLoader : LevelLoader
    {
        LevelCreator creator;

        private void Start()
        {
            creator = GetComponent<LevelCreator>();
            Init();
        }
        public override void LoadLevel()
        {
            if (levelName != "")
            {
                string levelString = fileHandler.LoadLevel(levelName);
                IntegerGrid levelData = JsonUtility.FromJson<IntegerGrid>(levelString);
                creator.Init(levelData.size, levelName);
                foreach (PositionTile tileData in levelData.tiles)
                {
                    creator.PlaceTile((TileInteger)tileData.tile, tileData.position);
                }
                if (LevelLoaded != null)
                {
                    LevelLoaded();
                }
            }
        }
    }
}