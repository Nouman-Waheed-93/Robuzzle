using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Robuzzle.LevelBuilding
{
    public enum TileInteger {
        LRStair = 1, RLStair = 2, BFStair = 3,
        FBStair = 4, InvertedLStair = 5, InverteRStair = 6,
        InvertedBStair = 7, InvertedFStair = 8, LaidLStair = 9,
        LaidRStair = 10, LaidBStair = 11, LaidFStair = 12,
        SandyTile = 13, BrickTile = 14, LRBoard = 15, 
        BFBoard = 16, LaidLRBoard = 17, LaidBFBoard = 18,
        StandingLRBoard = 19, StandingBFBoard = 20, 
        StoneStandingCylinder = 21, StoneLaidLRCylinder = 22,
        StoneLaidBFCylinder = 23, LRSlope = 24, RLSlope =25, 
        BFSlope = 26, FBSlope = 27, InvertedLRSlope = 28,
        InvertedRLSlope = 29, InvertedBFSlope = 30,
        InvertedFBSlope = 31, LaidLRSlope = 32, LaidRLSLope = 33,
        LaidFBSlope = 34, LaidBFSlope = 35, Draggable = 36,
        LRSlider = 37, BFSlider = 38, DUSlider = 39, LRRail = 40,
        BFRail = 41, DURail = 42, LFRailElbow = 43,
        FRRailElbow = 44, RBRailElbow = 45, BLRailElbow = 46, 
        DLRailElbow = 47, DFRailElbow = 48, DRRailElbow = 49,
        DBRailElbow = 50, ULRailElbow = 51, UFRailElbow = 52,
        URRailElbow = 53, UBRailElbow = 54, LMotor = 55,
        RMotor = 56, BMotor = 57, FMotor = 58, DMotor = 59,
        UMotor = 60, WoodenTile = 61, WoodenCylinderStanding = 62,
        WoodenLRCylinder = 63, WoodenBFCylinder = 64, WoodenHalfLRCylinder = 65,
        WoodenHalfRLCylinder = 66, WoodenHalfBFCylinder = 67, WoodenHalfFBCylinder = 68,
        WoodenHalfLRSideCylinder = 69, WoodenHalfRLSideCylinder = 70,
        WoodenHalfBFSideCylinder = 71, WoodenHalfFBSideCylinder = 72,
        WoodenHalfLRStandingCylinder = 73, WoodenHalfRLStandingCylinder = 74,
        WoodenHalfBFStandingCylinder = 75, WoodenHalfFBStandingCylinder = 76,
        Ball = 77, RedRobo = 78, YelloWRobo = 79, Eye = 80, Shocker = 81, Goal = 82,
        StoneTile = 83
    }
    
    [System.Serializable]
    public class PositionTile
    {
        public Vector3Int position;
        public int tile;
    }

    [System.Serializable]
    public class IntegerGrid
    {
        #region Variables
        public List<PositionTile> tiles;
        public Vector3Int size;
        #endregion
    }
    
    public class IntegerGridHandler
    {
        #region Variables
        public IntegerGrid grid {get; private set; }
        #endregion
        #region Public Methods
        public IntegerGridHandler(Vector3Int size)
        {
            grid = new IntegerGrid();
            grid.size = size;
            grid.tiles = new List<PositionTile>();
        }
        
        public bool CreateTile(TileInteger tile, Vector3Int position)
        {
            if (PositionIsInsideGrid(position) && !PositionIsFilled(position))
            {
                PositionTile pTile = new PositionTile();
                pTile.position = position;
                pTile.tile = (int)tile;
                grid.tiles.Add(pTile);
                return true;
            }
            return false;
        }

        public void DeleteTile(Vector3Int position)
        {
            if (PositionIsInsideGrid(position))
            {
                grid.tiles.Remove((PositionTile)grid.tiles.Where(tile => tile.position == position));
            }
        }
        #endregion
        #region Private Methods
        private bool PositionIsInsideGrid(Vector3Int position)
        {
            return position.x >= 0 && position.y >= 0 && position.z >= 0
                && position.x < grid.size.x && position.y < grid.size.y && position.z < grid.size.z;
        }

        private bool PositionIsFilled(Vector3Int position)
        {
            var tileVar = from pTile in grid.tiles where pTile.position.Equals(position) select pTile;
            return tileVar.FirstOrDefault<PositionTile>() != null;
        }
        #endregion

    }
}