using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobuzzlePathFinding;

namespace Robuzzle {
    public class RobuzzleGrid : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        Vector3Int size; // three dimensional size of the grid

        Graph graph; //Graph is a mesh used for pathfinding
        Tile[,,] tiles; // The 3d array that represents the 3d grid of tiles in game
        #endregion
        #region PublicMethods
        public void CreateGrid()
        {
            tiles = new Tile[size.x, size.y, size.z];
            graph = new Graph();
        }
        
        public bool PositionIsFilled(Vector3Int position)
        {
            return tiles[position.x, position.y, position.z] != null;
        }

        /*
         * Tile position is set when a tile is created or moved from one position to another position
         */
        public void SetTilePosition(Tile tile, Vector3Int position)
        {
            tiles[position.x, position.y, position.z] = tile;
            //Add the tile position in graph
            graph.AddNode(tile.gameObject, false, false, false);

            if (tile.EnterableSides.left) // an agent can enter the tile from left side
            {
                if (position.x - 1 > -1) //grid has space on the left side of the tile
                {
                    Tile leftNeighbor = tiles[position.x - 1, position.y, position.z];
                    MakePathBetweenTiles(leftNeighbor, tile, SideName.left);
                }
            }
            if (tile.EnterableSides.right)
            {
                if (position.x + 1 < size.x)// grid has space on the right side of the tile
                {
                    Tile rightNeighbor = tiles[position.x + 1, position.y, position.z];
                    MakePathBetweenTiles(rightNeighbor, tile, SideName.right);
                }
            }
            if (tile.EnterableSides.back)
            {
                if(position.z - 1 > -1)// grid has space on the back side of the tile
                {
                    Tile backNeighbor = tiles[position.x, position.y, position.z - 1];
                    MakePathBetweenTiles(backNeighbor, tile, SideName.back);
                }
            }
            if (tile.EnterableSides.front)
            {
                if (position.z + 1 < size.z)// grid has space on the front side of the tile
                {
                    Tile frontNeighbor = tiles[position.x, position.y, position.z + 1];
                    MakePathBetweenTiles(frontNeighbor, tile, SideName.front);
                }
            }

            if (position.y - 1 > -1) // grid has space below the tile
            {
                //we want to remove paths that go to or come from the tile beneath this tile
                Tile tileBeneath = tiles[position.x, position.y - 1, position.z];
                if (tileBeneath != null)
                    graph.RemoveNode(tileBeneath.gameObject);

                if (tile.LowerEnterableSides.left) // an agent can enter the tile from lower left side
                {
                    if (position.x - 1 > -1) //grid has space on the left side of the tile
                    {
                        Tile lowerLeftNeighbor = tiles[position.x - 1, position.y - 1, position.z];
                        MakePathBetweenTiles(lowerLeftNeighbor, tile, SideName.left);
                    }
                }
                if (tile.LowerEnterableSides.right)
                {
                    if (position.x + 1 < size.x)// grid has space on the right side of the tile
                    {
                        Tile lowerRightNeighbor = tiles[position.x + 1, position.y - 1, position.z];
                        MakePathBetweenTiles(lowerRightNeighbor, tile, SideName.right);
                    }
                }
                if (tile.LowerEnterableSides.back)
                {
                    if (position.z - 1 > -1)// grid has space on the back side of the tile
                    {
                        Tile lowerBackNeighbor = tiles[position.x, position.y - 1, position.z - 1];
                        MakePathBetweenTiles(lowerBackNeighbor, tile, SideName.back);
                    }
                }
                if (tile.LowerEnterableSides.front)
                {
                    if (position.z + 1 < size.z)// grid has space on the front side of the tile
                    {
                        Tile lowerFrontNeighbor = tiles[position.x, position.y - 1, position.z + 1];
                        MakePathBetweenTiles(lowerFrontNeighbor, tile, SideName.front);
                    }
                }
            }
        }
        
        // TIle position is removed when a tile is displaced, The new position is given by SetTilePosition method
        public void RemoveTilePosition(Tile tile, Vector3Int position)
        {
            if (tiles[position.x, position.y, position.z] != tile)
                return;
            tiles[position.x, position.y, position.z] = null;
            //Remove the tile position from the graph
            graph.RemoveNode(tile.gameObject);
        }
        #endregion
        #region PrivateMethods
        private void MakePathBetweenTiles(Tile neighbor, Tile tile, SideName side)
        {
            bool isNull = neighbor != null;
            if (neighbor != null) // left neighbor exists
            {
                bool neighborSide = false; //bool to tell if agent can enter the neighbor tile from the tile
                //if neighbor is on left side of this tile, then this tile is on the right side of the neighbor
                //if neighbor is on back side of the tile, then the tile is on the front of the neighbor
                //So, check if an agent can go from this tile to the neighboring tile
                switch (side)
                {
                    case SideName.left:
                        neighborSide = neighbor.EnterableSides.right;
                        break;
                    case SideName.right:
                        neighborSide = neighbor.EnterableSides.left;
                        break;
                    case SideName.back:
                        neighborSide = neighbor.EnterableSides.front;
                        break;
                    case SideName.front:
                        neighborSide = neighbor.EnterableSides.back;
                        break;
                }
                
                if (neighborSide) // an agent can enter the neighbor tile from this tile
                {
                    graph.AddEdge(neighbor.gameObject, tile.gameObject); // Now, agent can go from this tile to neighbor tile
                    graph.AddEdge(tile.gameObject, neighbor.gameObject); //Now, agent can go from the neighbor tile to this tile
                }
            }
        }
        #endregion
        #region DebugMethods
        private void Update()
        {
            graph.debugDraw();
        }
        #endregion
    }
}
