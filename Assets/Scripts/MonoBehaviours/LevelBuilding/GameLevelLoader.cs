using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle.LevelBuilding
{
    public class GameLevelLoader : LevelLoader
    {
        RobuzzleFactory factory;
        ViewHandler viewHandler;

        private void Start()
        {
            Init();
            viewHandler = ViewHandler.singleton;
            factory = GetComponent<RobuzzleFactory>();
            Invoke("LoadLevel", 0.1f);
        }

        public override void LoadLevel()
        {
            if (levelName != "")
            {
                string levelString = fileHandler.LoadLevel(levelName);
                IntegerGrid levelData = JsonUtility.FromJson<IntegerGrid>(levelString);
                RobuzzleGrid.singleton.CreateGrid(levelData.size);
                viewHandler.Init(levelData.size);
                foreach (PositionTile tileData in levelData.tiles)
                {
                    switch ((TileInteger)tileData.tile)
                    {
                        case TileInteger.RedRobo:
                            factory.CreateAgent(tileData.position);
                            break;
                        case TileInteger.YelloWRobo:
                            factory.CreateAgent(tileData.position);
                            break;
                        #region Simple Tiles
                        case TileInteger.WoodenTile:
                            factory.CreateMovableTile(tileData.position);
                            break;
                        case TileInteger.StoneTile:
                            factory.CreateFixedTile(tileData.position);
                            break;
                        case TileInteger.Draggable:
                            factory.CreateDraggable(tileData.position);
                            break;
                        case TileInteger.Goal:
                            factory.CreateGoalTile(tileData.position);
                            break;
                        #endregion
                        #region Rails
                        case TileInteger.BFRail:
                            factory.CreateRailBF(tileData.position);
                            break;
                        case TileInteger.DURail:
                            factory.CreateRailUD(tileData.position);
                            break;
                        case TileInteger.LRRail:
                            factory.CreateRailLR(tileData.position);
                            break;
                        #endregion
                        #region Sliders
                        case TileInteger.BFSlider:
                            factory.CreateSliderBF(tileData.position);
                            break;
                        case TileInteger.DUSlider:
                            factory.CreateSliderUD(tileData.position);
                            break;
                        case TileInteger.LRSlider:
                            factory.CreateSliderLR(tileData.position);
                            break;
                        #endregion
                        #region Stairs
                        case TileInteger.BFStair:
                            factory.CreateStairBF(tileData.position);
                            break;
                        case TileInteger.LRStair:
                            factory.CreateStairLR(tileData.position);
                            break;
                        case TileInteger.FBStair:
                            factory.CreateStairFB(tileData.position);
                            break;
                        case TileInteger.RLStair:
                            factory.CreateStairRL(tileData.position);
                            break;
                        #endregion
                        #region Motors
                        case TileInteger.LMotor:
                            factory.CreateLMotor(tileData.position);
                            break;
                        case TileInteger.RMotor:
                            factory.CreateRMotor(tileData.position);
                            break;
                        case TileInteger.BMotor:
                            factory.CreateBMotor(tileData.position);
                            break;
                        case TileInteger.FMotor:
                            factory.CreateFMotor(tileData.position);
                            break;
                        case TileInteger.DMotor:
                            factory.CreateDMotor(tileData.position);
                            break;
                        case TileInteger.UMotor:
                            factory.CreateUMotor(tileData.position);
                            break;
                            #endregion
                    }
                }
                
                if(LevelLoaded != null)
                {
                    LevelLoaded();
                }
            }
        }
    }
}