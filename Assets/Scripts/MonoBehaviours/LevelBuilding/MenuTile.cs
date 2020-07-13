using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle.LevelBuilding
{
    public class MenuTile : MonoBehaviour
    {
        #region Variables
        public Vector3 bigSize = new Vector3(1.5f, 1.5f, 1.5f);
        public Vector3 normalSize = new Vector3(1, 1, 1);
        public TileInteger tileType;

        private LevelCreator levelCreator;
        private ViewHandler viewHandler;
        #endregion
        #region Unity Callbacks
        private void Start()
        {
            levelCreator = LevelCreator.singleton;
            viewHandler = ViewHandler.singleton;
            viewHandler.ViewRotated += Rotate;
        }

        void OnMouseDown()
        {
            SelectTile();
        }
        #endregion
        #region Private Methods
        private void SelectTile()
        {
            if (levelCreator.selectedTile != null)
                levelCreator.selectedTile.ToNormalSize();
            levelCreator.SelectTile(this);
            levelCreator.selectedTile.ToBigSize();
        }

        private void ToNormalSize()
        {
            transform.localScale = normalSize;
        }

        private void ToBigSize()
        {
            transform.localScale = bigSize;
        }

        private void Rotate(Quaternion rotation)
        {
            transform.localRotation = rotation;
        }
        #endregion
    }
}
