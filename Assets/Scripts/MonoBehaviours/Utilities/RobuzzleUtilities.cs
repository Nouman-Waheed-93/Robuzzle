using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class RobuzzleUtilities : MonoBehaviour
    {
        public static SideName GetOppositeSide(SideName side)
        {
            switch (side)
            {
                case SideName.right:
                    return SideName.left;
                case SideName.left:
                    return SideName.right;
                case SideName.front:
                    return SideName.back;
                case SideName.back:
                    return SideName.front;
                case SideName.up:
                    return SideName.down;
                case SideName.down:
                    return SideName.up;
          }
            //Code will never get here.. So, to fulfil the requirement of compoiler
            return SideName.right;
        }
        
        public static bool IsTileAttachableOnOppositeSide(MovableTile tile, SideName side)
        {
            switch (side)
            {
                case SideName.right:
                    return tile.AttachableSides.left;
                case SideName.left:
                    return tile.AttachableSides.right;
                case SideName.front:
                    return tile.AttachableSides.back;
                case SideName.back:
                    return tile.AttachableSides.front;
                case SideName.up:
                    return tile.AttachableSides.down;
                case SideName.down:
                    return tile.AttachableSides.up;
            }
            return false;
        }

        public static bool IsTileAttachableOnSide(MovableTile tile, SideName side)
        {
            switch (side)
            {
                case SideName.right:
                    return tile.AttachableSides.right;
                case SideName.left:
                    return tile.AttachableSides.left;
                case SideName.front:
                    return tile.AttachableSides.front;
                case SideName.back:
                    return tile.AttachableSides.back;
                case SideName.up:
                    return tile.AttachableSides.up;
                case SideName.down:
                    return tile.AttachableSides.down;
            }
            return false;
        }

        public static Vector3Int GetSideVector(SideName side)
        {
            switch (side)
            {
                case SideName.right:
                    return Vector3Int.right;
                case SideName.left:
                    return Vector3Int.left;
                case SideName.front:
                    return new Vector3Int(0, 0, 1);
                case SideName.back:
                    return new Vector3Int(0, 0, -1);
                case SideName.up:
                    return Vector3Int.up;
                case SideName.down:
                    return Vector3Int.down;
            }
            return Vector3Int.zero;
        }

        // Any perpendicular axis to the Vector
        public static Vector3Int GetPerpendicularSideVector(SideName side)
        {
            switch (side)
            {
                case SideName.right:
                    return Vector3Int.up;
                case SideName.left:
                    return Vector3Int.down;
                case SideName.front:
                    return Vector3Int.right;
                case SideName.back:
                    return Vector3Int.left;
                case SideName.up:
                    return Vector3Int.right;
                case SideName.down:
                    return Vector3Int.left;
            }
            return Vector3Int.zero;
        }

        //if a side is right, up, or forward
        public static bool IsPositiveSide(SideName side)
        {
            return side == SideName.right || side == SideName.up || side == SideName.front;
        }


        public static Vector3Int GetTilePositionUnderCursor(Camera cam, IGrid grid)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                return Vector3Int.RoundToInt(hit.collider.transform.position);
            }
            return -Vector3Int.one;
        }


        public static GameObject GetGameObjectUnderCursor(Camera cam)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                return hit.collider.gameObject;
            }
            return null;
        }

        public static Vector3Int GetEmptyPositionUnderCursor(Camera cam, IGrid grid)
        {
            float checkIncrement = 0.25f;
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                float reach = hit.distance - checkIncrement;
                while(reach >= checkIncrement)
                {
                    Vector3Int checkPosition = Vector3Int.RoundToInt(ray.GetPoint(reach));
                    if (grid.PositionIsInsideGrid(checkPosition) && !grid.PositionIsFilled(checkPosition))
                    {
                        return checkPosition;
                    }

                    reach -= checkIncrement;
                }
            }
            return -Vector3Int.one;
        }
        
    }
}
