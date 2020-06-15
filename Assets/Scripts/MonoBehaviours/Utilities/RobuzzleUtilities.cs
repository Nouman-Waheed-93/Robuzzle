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
    }
}
