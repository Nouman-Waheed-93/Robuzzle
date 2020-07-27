using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public interface IGrid
    {
        bool PositionIsFilled(Vector3Int position);
        bool PositionIsInsideGrid(Vector3Int position);
    }
}