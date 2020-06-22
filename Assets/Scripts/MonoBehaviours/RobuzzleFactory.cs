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
        [SerializeField]
        Slider sliderLR;
        [SerializeField]
        SliderRail railLR;

        int tileNumber;
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
            grid.SubscribeTileMovement(movableTile);
            //TODO: JoinToNeighbors
            JoinNeighbors(movableTile);
        }

        public void CreateDraggable(Vector3Int position)
        {
            if (grid.PositionIsFilled(position))
                return;
            MovableTile movableTile = (MovableTile)CreateTile(draggable, position);
            grid.SubscribeTileMovement(movableTile);
            JoinNeighbors(movableTile);
        }

        public void CreateMotor(Vector3Int position)
        {
            if (grid.PositionIsFilled(position))
                return;
            Motor motor = (Motor)CreateTile(this.motor, position);
        }
       
        public void CreateSliderLR(Vector3Int position)
        {
            if (grid.PositionIsFilled(position))
                return;

            MovableTile movableTile = (MovableTile)CreateTile(sliderLR, position);
            JoinNeighbors(movableTile);
            
            grid.SubscribeTileMovement(movableTile);
            
            Slider slider = (Slider)movableTile;

            SliderRail rail = CreateRailLR(position);
            //ignore collision between the rail and slider

            Physics.IgnoreCollision(movableTile.GetComponent<Collider>(), rail.GetComponent<Collider>(), true);

            slider.MinBound = slider.Position;
            slider.MaxBound = slider.Position;
            
            Tile leftTile = grid.GetTileAtPosition(slider.Position + Vector3Int.left);
            if (leftTile != null && leftTile.GetType() == typeof(SliderRail))
            {
                SliderRail leftRail = (SliderRail)leftTile;
                if (leftRail.AttachableSides.right)
                    slider.MinBound = GetExtremeBoundNIgnoreCollision(slider, leftRail, SideName.left);
            }

            //Create a method to end redundancy
            Tile rightTile = grid.GetTileAtPosition(slider.Position + Vector3Int.right);
            if (rightTile != null && rightTile.GetType() == typeof(SliderRail))
            {
                SliderRail rightRail = (SliderRail)rightTile;
                if (rightRail.AttachableSides.left)
                    slider.MaxBound = GetExtremeBoundNIgnoreCollision(slider, rightRail, SideName.right);
            }
        }

        public void CreateSliderUD(Vector3Int position)
        {

        }

        public void CreateSliderBF(Vector3Int position)
        {

        }
        
        public SliderRail CreateRailLR(Vector3Int position)
        {
            MovableTile movableTile = (MovableTile)CreateTile(railLR, position);
            JoinNeighbors(movableTile);

            grid.SubscribeTileMovement(movableTile);

            return (SliderRail)movableTile;
        }

        public void CreateRailBF(Vector3Int position)
        {

        }

        public void CreateRailUD(Vector3Int position)
        {

        }
        #endregion
        #region Private Methods
        private Tile CreateTile(Tile tile, Vector3Int position)
        {
            Tile newTile = Instantiate(tile, position, Quaternion.identity);
            tileNumber++;
            newTile.gameObject.name = tile.name + tileNumber;
            grid.SetTilePosition(newTile, position);
            return newTile;
        }
        
        private void JoinNeighbor(RigidbodyTile rbTile, MovableTile neighbor, bool tile2OnNegativeSide)
        {
            RigidbodyTile neighborRigidbodyTile = GetRigidbodyTile(neighbor); 
            if (neighborRigidbodyTile != null) //if the neighbor has rigidbody, or a parent has rigidbody
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

                /* Before attaching tile1 with tile2 we want to make sure that tile2 has a compound.
                 * Because, tile1 is added into tile2's cmompound
                 */

                if (rbTile2.Compound == null) //tile2 has no compound
                {
                    if (rbTile1.Compound == null) //tile1 has no compound
                    {
                        rbTile2.AddInCompound(new TileCompound());
                    }
                    else // if tile1 has a compound, assign it to tile2
                    {
                        rbTile2.AddInCompound(rbTile1.Compound);
                    }
                }
                else if(rbTile1.Compound != null) // if both tiles have compound we will merge the smaller one into bigger one
                {
                    if (rbTile2.Compound.GetTotalSize() > rbTile1.Compound.GetTotalSize())
                        rbTile2.Compound.Integrate(rbTile1.Compound);
                    else
                        rbTile1.Compound.Integrate(rbTile2.Compound);
                }
                //now rbTile2 must have a compound. Tile1 is added into tile2's compound
                rbTile1.Attach(rbTile2);
            }
            else // if the neighbor has no rigidbody onitself and on its parent
            {
                if (rbTile.Compound == null)
                    rbTile.AddInCompound(new TileCompound());
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
            bool retVal = tile1.Compound != null && tile2.Compound != null &&
                            tile1.Compound == tile2.Compound;
            Debug.Log(tile1.gameObject.name + " and " + tile2.gameObject.name + " Are attached " + retVal);
            return retVal;
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
        
        private Vector3Int GetExtremeBoundNIgnoreCollision(Slider slider, SliderRail rail, SideName side)
        {
            Physics.IgnoreCollision(slider.GetComponent<Collider>(), rail.GetComponent<Collider>(), true);

            switch (side)
            {
                case SideName.left:
                    if (rail.AttachableSides.left)
                    {
                        Tile leftTile = grid.GetTileAtPosition(rail.Position + Vector3Int.left);
                        if(leftTile != null && leftTile.GetType() == typeof(SliderRail))
                        {
                            SliderRail leftRail = (SliderRail)leftTile;
                            if (leftRail.AttachableSides.right)
                                return GetExtremeBoundNIgnoreCollision(slider, leftRail, SideName.left);
                        }
                        return rail.Position;
                    }
                    break;
                case SideName.right:
                    if (rail.AttachableSides.right)
                    {
                        Tile rightTile = grid.GetTileAtPosition(rail.Position + Vector3Int.right);
                        if (rightTile != null && rightTile.GetType() == typeof(SliderRail))
                        {
                            SliderRail rightRail = (SliderRail)rightTile;
                            if (rightRail.AttachableSides.left)
                                return GetExtremeBoundNIgnoreCollision(slider, rightRail, SideName.right);
                        }
                        return rail.Position;
                    }
                    break;
            }
            return Vector3Int.zero;
        }
        #endregion
    }
}
