using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class RobuzzleFactory : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        Tile fixedTile;
        [SerializeField]
        MovableTile movableTile;
        [SerializeField]
        Motor motor;
        [SerializeField]
        Draggable draggable;

        RobuzzleGrid grid;
        #endregion
        #region Unity Callback
        private void Start()
        {
            grid = (RobuzzleGrid)RobuzzleGrid.singleton;
        }
        #endregion
        #region Public Methods
        public void CreateFixedTile(Vector3Int position)
        {
            if (grid.PositionIsFilled(position))
                return;
            CreateTile(fixedTile, position);
        }

        public void CreateMovableTile(Vector3Int position)
        {
            if (grid.PositionIsFilled(position))
                return;
            MovableTile movableTile = (MovableTile)CreateTile(this.movableTile, position);
            //TODO: JoinToNeighbors
            JoinNeighbors(movableTile);
        }

        public void CreateDraggable(Vector3Int position)
        {
            if (grid.PositionIsFilled(position))
                return;
            MovableTile movableTile = (MovableTile)CreateTile(draggable, position);
            JoinNeighbors(movableTile);
        }

        public void CreateMotor(Vector3Int position)
        {
            if (grid.PositionIsFilled(position))
                return;
            Motor motor = (Motor)CreateTile(this.motor, position);
        }
        #endregion
        #region Private Methods
        private Tile CreateTile(Tile tile, Vector3Int position)
        {
            Tile newTile = Instantiate(tile, position, Quaternion.identity);
            grid.SetTilePosition(newTile, position);
            return newTile;
        }
        
        private void JoinNeighbor(RigidbodyTile rbTile, MovableTile neighbor, bool tile2OnNegativeSide)
        {
            RigidbodyTile neighborRigidbodyTile = GetRigidbodyTile(neighbor);// remains same
            if (neighborRigidbodyTile != null) //reamains same
            {
                RigidbodyTile rbTile1;
                RigidbodyTile rbTile2;
                if (tile2OnNegativeSide)
                {
                    Debug.Log("On Negative Side");
                    rbTile1 = neighborRigidbodyTile;
                    rbTile2 = rbTile;
                }
                else
                {
                    rbTile1 = rbTile;
                    rbTile2 = neighborRigidbodyTile;
                }

                if (rbTile1.Compound == null) //changes
                {
                    if (rbTile2.Compound == null) //changes
                    {
                        rbTile1.SetCompound(new TileCompound());//changes
                    }
                    else // if tile has a compound, assign it to neighbor
                    {
                        rbTile1.SetCompound(rbTile2.Compound);//changes
                    }
                }
                //changes
                rbTile1.Attach(rbTile2);
            }
            else //remains same
            {
                if (rbTile.Compound == null)
                    rbTile.SetCompound(new TileCompound());
                neighbor.Attach(rbTile);
                /*
                 * Simple moving tiles are not connected to their neighbors, 
                 * until a rigidbody is added in their neighborhood. So, they must 
                 * attach all their neighbors recursively when a rigidbody is added in neighborhood
                 * */
                JoinNeighbors(neighbor);
            }
        }

        //TODO: Recheck tile type 
        private void JoinNeighbor(RigidbodyTile tile, MovableTile neighbor, SideName side)
        {
            //the neighbor is on left, down or back side of the tile
            if (side == SideName.left || side == SideName.down || side == SideName.back)
            {
                JoinNeighbor(tile, neighbor, true);
            }
            else// neighbor is on the right, up or front side of the tile
            {
                JoinNeighbor(tile, neighbor, false);
            }
        }
        
        private void TryJoiningNeighbor(MovableTile tile, Tile neighbor, SideName side)
        {
            if (IsMovable(neighbor)) // the neighbor is attachable/movable
            {
                MovableTile neighborMovableTile = (MovableTile)neighbor;
                bool neighborAttachableOnOppositeSide = 
                    RobuzzleUtilities.IsTileAttachableOnOppositeSide(neighborMovableTile, side);

                if (neighborAttachableOnOppositeSide) //the tile and neighbor can join together
                {
                    //if the tile neighbor tile are not already attached
                    if (!AreAttached(tile, neighborMovableTile))
                    {
                        RigidbodyTile RBTile = GetRigidbodyTile(tile);
                        if (RBTile != null)//If neighbor already has a rigidbody, attach both rigidbodies
                        {
                            JoinNeighbor(RBTile, neighborMovableTile, side);
                        }
                        else//join simple movable tile with neighbors -> or can that be done somewhere else 
                        {
                            RigidbodyTile RBNeighbor = GetRigidbodyTile(neighborMovableTile);
                            if (RBNeighbor != null)
                            {
                                JoinNeighbor(RBNeighbor, tile, RobuzzleUtilities.GetOppositeSide(side)); //this tile is on the left of neighbor
                            }
                        }
                    }
                }
            }
        }

        private void JoinNeighbors(MovableTile tile)
        {
            //TODO: Make a mechanism to join a simple movable tile to its neighbors
            //join tile on rightside
            if (tile.AttachableSides.right) //if tile can be attached from right side
            {
                Tile rightSideTile = grid.GetTileAtPosition(tile.Position + Vector3Int.right);
                TryJoiningNeighbor(tile, rightSideTile, SideName.right);
            }   
            if (tile.AttachableSides.left)
            {
                Tile leftSideTile = grid.GetTileAtPosition(tile.Position + Vector3Int.left);
                TryJoiningNeighbor(tile, leftSideTile, SideName.left);
            }
            if (tile.AttachableSides.front)
            {
                Tile frontSideTile = grid.GetTileAtPosition(tile.Position + new Vector3Int(0, 0, 1));
                TryJoiningNeighbor(tile, frontSideTile, SideName.front);
            }
            if (tile.AttachableSides.back)
            {
                Tile backSideTile = grid.GetTileAtPosition(tile.Position + new Vector3Int(0, 0, -1));
                TryJoiningNeighbor(tile, backSideTile, SideName.back);
            }
            if (tile.AttachableSides.up)
            {
                Tile upperTile = grid.GetTileAtPosition(tile.Position + Vector3Int.up);
                TryJoiningNeighbor(tile, upperTile, SideName.up);
            }
            if (tile.AttachableSides.down)
            {
                Tile downSideTile = grid.GetTileAtPosition(tile.Position + Vector3Int.down);
                TryJoiningNeighbor(tile, downSideTile, SideName.down);
            }
        }

        private bool AreAttached(MovableTile tile1, MovableTile tile2)
        {
            return tile1.Compound != null && tile2.Compound != null &&
                            tile1.Compound == tile2.Compound;
        }

        private bool IsMovable(Tile tile)
        {
            return tile != null && // does the tile exist and...
                //is tile movable
                    (tile.GetType() == typeof(MovableTile) || tile.GetType().IsSubclassOf(typeof(MovableTile)));
        }
        
        private RigidbodyTile GetRigidbodyTile(MovableTile tile)
        {
            RigidbodyTile RBTile = null;
            if (tile.GetType() == typeof(RigidbodyTile) || tile.GetType().IsSubclassOf(typeof(RigidbodyTile)))
            {
                RBTile = (RigidbodyTile)tile;
            }
            else if(tile.transform.parent != null)
            {
                RBTile = tile.transform.parent.GetComponent<RigidbodyTile>();
            }
            return RBTile;
        }
        #endregion
    }
}
