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
        
        private void AttachRespectively(RigidbodyTile rbTile, MovableTile neighbor, bool tile2OnNegativeSide)
        {
            RigidbodyTile neighborRigidbodyTile = GetRigidbodyTile(neighbor);// remains same
            if (neighborRigidbodyTile != null) //reamains same
            {
                RigidbodyTile rbTile1;
                RigidbodyTile rbTile2;
                if (tile2OnNegativeSide)
                {
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
            if (side == SideName.left || side == SideName.down || side == SideName.back)
            {
                RigidbodyTile neighborRigidBbodyTile = GetRigidbodyTile(neighbor);// = neighbor.GetComponentInParent<RigidbodyTile>();
                if (neighborRigidBbodyTile != null) //if the neighbor tile has a rigidbody on itself, or has one in parent
                {
                    if (neighbor.Compound == null) //if neighbor has no compound
                    {
                        if (tile.Compound == null) //if tile also has no compound
                        {
                            neighborRigidBbodyTile.SetCompound(new TileCompound());
                        }
                        else // if tile has a compound, assign it to neighbor
                        {
                            neighborRigidBbodyTile.SetCompound(tile.Compound);
                        }
                    }
                    //Now neighbor must have a compound
                    neighborRigidBbodyTile.Attach(tile);
                }
                else //if neighbor is a simple movable tile
                {
                    if (tile.Compound == null)
                        tile.SetCompound(new TileCompound());
                    neighbor.Attach(tile);
                    /*
                     * Simple moving tiles are not connected to their neighbors, 
                     * until a rigidbody is added in their neighborhood. So, they must 
                     * attach all their neighbors recursively when a rigidbody is added in neighborhood
                     * */
                    JoinNeighbors(neighbor);
                }
            }
            else// neighbor is on the right, up or front side of the tile
            {
                RigidbodyTile neighborRigidBbodyTile = GetRigidbodyTile(neighbor); //neighbor.GetComponentInParent<RigidbodyTile>();
                if (neighborRigidBbodyTile != null) //if the neighbor tile has a rigidbody on itself, or has one in parent
                {
                    if (tile.Compound == null) //if neighbor has no compound
                    {
                        if (neighbor.Compound == null) //if tile also has no compound
                        {
                            tile.SetCompound(new TileCompound());// neighborRigidBbodyTile.SetCompound(new TileCompound());
                        }
                        else // if tile has a compound, assign it to neighbor
                        {
                            tile.SetCompound(neighbor.Compound); //neighborRigidBbodyTile.SetCompound(tile.Compound);
                        }
                    }
                    //Now neighbor must have a compound
                    tile.Attach(neighborRigidBbodyTile); // neighborRigidBbodyTile.Attach(tile);
                }
                else //if neighbor is a simple movable tile
                {
                    if (tile.Compound == null)
                        tile.SetCompound(new TileCompound());
                    neighbor.Attach(tile);
                    /*
                     * Simple moving tiles are not connected to their neighbors, 
                     * until a rigidbody is added in their neighborhood. So, they must 
                     * attach all their neighbors recursively when a rigidbody is added in neighborhood
                     * */
                    JoinNeighbors(neighbor);
                }
            }

        }
        
        private void JoinNeighbors(MovableTile tile)
        {
            //TODO: Make a mechanism to join a simple movable tile to its neighbors
            //join tile on rightside
            if (tile.AttachableSides.right) //if tile can be attached from right side
            {
                Debug.Log("Attachable on right");
                Tile rightSideTile = grid.GetTileAtPosition(tile.Position + Vector3Int.right);
                if (IsMovable(rightSideTile)) // the tile on right side is attachable/movable
                {
                    Debug.Log("Right tile is movable");
                    MovableTile rightMovableTile = (MovableTile)rightSideTile;
                    if (rightMovableTile.AttachableSides.left) //if right side tile is attachable on left side
                    {
                        Debug.Log("Right tile is attachable on left");
                        //if the tile and right Movable tile are not already attached
                        if (!AreAttached(tile, rightMovableTile))
                        {
                            Debug.Log("Are not attached");
                            RigidbodyTile RBTile = GetRigidbodyTile(tile);
                            if (RBTile != null)
                            {
                                Debug.Log("Found rigidbody tile on the tile");
                                JoinNeighbor(RBTile, rightMovableTile, SideName.right);//neighbor is on the right of tile
                            }
                            else//join simple movable tile with neighbors -> or can that be done somewhere else 
                            {
                                RigidbodyTile RBNeighbor = GetRigidbodyTile(rightMovableTile);
                                if (RBNeighbor != null)
                                {
                                    Debug.Log("Found rigidbody tile on the neighbor");
                                    JoinNeighbor(RBNeighbor, tile, SideName.left); //this tile is on the left of neighbor
                                }
                            }
                        }
                    }
                }
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
