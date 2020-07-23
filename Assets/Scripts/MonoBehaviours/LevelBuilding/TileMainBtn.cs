using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle.LevelBuilding
{
    public class TileMainBtn : MonoBehaviour
    {
        private void OnMouseDown()
        {
            LevelCreator.singleton.ToMainTileMenu();
        }
    }
}