using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class RobuzzleFactory : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        MovableTile agent;
        [SerializeField]
        Tile fixedTile;
        [SerializeField]
        MovableTile movableTile;
        [SerializeField]
        Motor motor;
        [SerializeField]
        Draggable draggable;
        [SerializeField]
        Slider sliderLR; //slider that moves on x axis
        [SerializeField]
        SliderRail railLR;// rail on x axis
        [SerializeField]
        Slider sliderBF; //slider that moves on z axis
        [SerializeField]
        SliderRail railBF; //rail on z axis
        [SerializeField]
        Slider sliderDU; //slider that moves on y axis
        [SerializeField]
        SliderRail railDU; //slider that moves on y axis

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

        public void CreateAgent(Vector3Int position)
        {
            if (grid.PositionIsFilled(position))
                return;
            MovableTile movableTile = (MovableTile)CreateTile(agent, position);
            grid.SubscribeTileMovement(movableTile);
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

            slider.MinBound = slider.Position;
            slider.MaxBound = slider.Position;

            SliderRail rail = CreateRailLR(position);
            //ignore collision between the rail and slider

            Physics.IgnoreCollision(movableTile.GetComponent<Collider>(), rail.GetComponent<Collider>(), true);
        }

        public void CreateSliderUD(Vector3Int position)
        {
            if (grid.PositionIsFilled(position))
                return;

            MovableTile movableTile = (MovableTile)CreateTile(sliderDU, position);
            JoinNeighbors(movableTile);

            grid.SubscribeTileMovement(movableTile);

            Slider slider = (Slider)movableTile;

            slider.MinBound = slider.Position;
            slider.MaxBound = slider.Position;

            SliderRail rail = CreateRailUD(position);
            //ignore collision between the rail and slider

            Physics.IgnoreCollision(movableTile.GetComponent<Collider>(), rail.GetComponent<Collider>(), true);
        }

        public void CreateSliderBF(Vector3Int position)
        {

            if (grid.PositionIsFilled(position))
                return;

            MovableTile movableTile = (MovableTile)CreateTile(sliderBF, position);
            JoinNeighbors(movableTile);

            grid.SubscribeTileMovement(movableTile);

            Slider slider = (Slider)movableTile;

            slider.MinBound = slider.Position;
            slider.MaxBound = slider.Position;

            SliderRail rail = CreateRailBF(position);
            //ignore collision between the rail and slider

            Physics.IgnoreCollision(movableTile.GetComponent<Collider>(), rail.GetComponent<Collider>(), true);
        }
        
        public SliderRail CreateRailLR(Vector3Int position)
        {
            MovableTile movableTile = (MovableTile)CreateTile(railLR, position);
            JoinNeighbors(movableTile);

            grid.SubscribeTileMovement(movableTile);

            SliderRail rail = (SliderRail)movableTile;
            rail.Position = position;
            //find slider on this movement line
            Slider slider = GetSliderOnAxis(rail);
            //set bounds for that slider
            if (slider)
                SetSliderBounds(slider);
            return (SliderRail)movableTile;
        }

        public SliderRail CreateRailBF(Vector3Int position)
        {
            MovableTile movableTile = (MovableTile)CreateTile(railBF, position);
            JoinNeighbors(movableTile);

            grid.SubscribeTileMovement(movableTile);

            SliderRail rail = (SliderRail)movableTile;
            rail.Position = position;
            //find slider on this movement line
            Slider slider = GetSliderOnAxis(rail);
            //set bounds for that slider
            if (slider)
                SetSliderBounds(slider);
            return (SliderRail)movableTile;
        }

        public SliderRail CreateRailUD(Vector3Int position)
        {
            MovableTile movableTile = (MovableTile)CreateTile(railDU, position);
            JoinNeighbors(movableTile);

            grid.SubscribeTileMovement(movableTile);

            SliderRail rail = (SliderRail)movableTile;
            rail.Position = position;
            //find slider on this movement line
            Slider slider = GetSliderOnAxis(rail);
            //set bounds for that slider
            if (slider)
                SetSliderBounds(slider);
            return (SliderRail)movableTile;
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
        
        private void SetSliderBound(Slider slider, Tile tile, SideName side)
        {
            if (tile != null && tile.GetType() == typeof(SliderRail))
            {
                SliderRail rail = (SliderRail)tile;
                if (RobuzzleUtilities.IsTileAttachableOnOppositeSide(rail, side))
                    if (RobuzzleUtilities.IsPositiveSide(side))
                        slider.MaxBound = GetExtremeBoundNIgnoreCollision(slider, rail, side);
                    else
                        slider.MinBound = GetExtremeBoundNIgnoreCollision(slider, rail, side);
            }
        }

        private Vector3Int GetExtremeBoundNIgnoreCollision(Slider slider, SliderRail rail, SideName side)
        {
            Physics.IgnoreCollision(slider.GetComponent<Collider>(), rail.GetComponent<Collider>(), true);

            if(RobuzzleUtilities.IsTileAttachableOnSide(rail, side))
            {
                Tile tile = grid.GetTileAtPosition(rail.Position + RobuzzleUtilities.GetSideVector(side));
                if(tile != null && tile.GetType() == typeof(SliderRail))
                {
                    SliderRail rail2 = (SliderRail)tile;
                    if (RobuzzleUtilities.IsTileAttachableOnOppositeSide(rail2, side))
                        return GetExtremeBoundNIgnoreCollision(slider, rail2, side);
                }
                return rail.Position;
            }
            
            return Vector3Int.zero;
        }
     
        private void SetSliderBounds(Slider slider)
        {
            if (!slider.AttachableSides.right) //if a slider is not attachable on right side it means, it moves on x axis
            {
                Tile leftTile = grid.GetTileAtPosition(slider.Position + Vector3Int.left);
                SetSliderBound(slider, leftTile, SideName.left);

                Tile rightTile = grid.GetTileAtPosition(slider.Position + Vector3Int.right);
                SetSliderBound(slider, rightTile, SideName.right);
            }
            else if (!slider.AttachableSides.front) //if a slider is not attachable on front side it means, it moves on z axis
            {
                Tile frontTile = grid.GetTileAtPosition(slider.Position + new Vector3Int(0, 0, 1));
                SetSliderBound(slider, frontTile, SideName.front);

                Tile backTile = grid.GetTileAtPosition(slider.Position + new Vector3Int(0, 0, -1));
                SetSliderBound(slider, backTile, SideName.back);
            }
            else if (!slider.AttachableSides.up) //if a slider is not attachable on up side it means, it moves on y axis
            {
                Tile upTile = grid.GetTileAtPosition(slider.Position + Vector3Int.up);
                SetSliderBound(slider, upTile, SideName.up);

                Tile downTile = grid.GetTileAtPosition(slider.Position + Vector3Int.down);
                SetSliderBound(slider, downTile, SideName.down);
            }
        }

        private Slider GetSliderOnAxis(SliderRail rail)
        {
            Slider retSlider = null;

            Tile tile = grid.GetTileAtPosition(rail.Position);
            bool tileExists = tile != null;
            Debug.Log("Tile pos " + rail.Position + " is " + tileExists);
            if (tile.GetType() == typeof(Slider))
            {
                return (Slider)tile;
            }

            if (rail.AttachableSides.left) //if the rail is Left Right oriented
            {
                Tile leftTile = grid.GetTileAtPosition(rail.Position + Vector3Int.left);

                retSlider = GetSliderOnTile(leftTile, SideName.left);

                if (retSlider != null)
                    return retSlider;

                Tile rightTile = grid.GetTileAtPosition(rail.Position + Vector3Int.right);

                retSlider = GetSliderOnTile(rightTile, SideName.right);

                return retSlider;
            }
            else if (rail.AttachableSides.back) // if the rail is back front oriented
            {
                Tile backTile = grid.GetTileAtPosition(rail.Position + new Vector3Int(0, 0, -1));

                retSlider = GetSliderOnTile(backTile, SideName.back);

                if (retSlider != null)
                    return retSlider;

                Tile frontTile = grid.GetTileAtPosition(rail.Position + new Vector3Int(0, 0, 1));

                retSlider = GetSliderOnTile(frontTile, SideName.front);

                return retSlider;
            }
            else if (rail.AttachableSides.down) // if the rail is up down oriented
            {
                Tile downTile = grid.GetTileAtPosition(rail.Position + Vector3Int.down);

                retSlider = GetSliderOnTile(downTile, SideName.down);

                if (retSlider != null)
                    return retSlider;

                Tile upTile = grid.GetTileAtPosition(rail.Position + Vector3Int.up);

                retSlider = GetSliderOnTile(upTile, SideName.up);

                return retSlider;
            }
            return null;
        }

        private Slider GetSliderOnTile(Tile tile, SideName side)
        {
            if (tile != null)
            {
                if (tile.GetType() == typeof(Slider))
                {
                    Slider slider = (Slider)tile;
                    //slider moves left/right if left side is not attachable, so it moves on this rail
                    if (!RobuzzleUtilities.IsTileAttachableOnSide(slider, side))
                        return slider;
                }
                else if (tile.GetType() == typeof(SliderRail))
                {
                    SliderRail rail = (SliderRail)tile;
                    if (RobuzzleUtilities.IsTileAttachableOnOppositeSide(rail, side))
                    {
                        return GetSliderOnTile(
                            grid.GetTileAtPosition(tile.Position + RobuzzleUtilities.GetSideVector(side)),
                            side);
                    }
                }
            }
            return null;
        }
        #endregion
    }
}
