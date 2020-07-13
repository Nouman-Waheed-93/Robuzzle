using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Robuzzle.LevelBuilding
{
    public class LevelCreator : SingletonMonoBehaviour<LevelCreator>
    {
        #region Variables
        public Action RangeExceeded;
        public IntegerGridHandler grid { get; private set; }
        public string name { get; private set; }
        public MenuTile selectedTile { get; private set; }
        public Material boundaryMaterial;

        ViewHandler viewHandler;
        #endregion
        #region Public Methods

        public void Init(Vector3Int size, string name)
        {
            CreateGrid(size);
            this.name = name;
            CreateVisualBoundaries(size);
            viewHandler = ViewHandler.singleton;
            viewHandler.SetView((Vector3)size * 0.5f);
        }

        public void SelectTile(MenuTile tile)
        {
            selectedTile = tile;
        }
        
        public void PlaceTile(Vector3Int position)
        {
            if (grid.CreateTile(selectedTile.tileType, position))
            {
                MenuTile tile = Instantiate(selectedTile, position, Quaternion.identity);
                tile.gameObject.layer = 0;
                tile.transform.localScale = Vector3.one;
                Destroy(tile);
            }
            else
                RangeExceeded.Invoke();
        }

        public void DeleteTile(Vector3Int position)
        {
            grid.DeleteTile(position);
        }
        #endregion

        #region Private Methods

        private void CreateGrid(Vector3Int size)
        {
            grid = new IntegerGridHandler(size);
        }
        
        private void CreateVisualBoundaries(Vector3Int size)
        {
            GameObject boundaryLine = new GameObject("Boundary Line");
            boundaryLine.transform.rotation = Quaternion.Euler(90, 0, 0);
            LineRenderer line = boundaryLine.AddComponent<LineRenderer>();
            line.alignment = LineAlignment.TransformZ;
            Vector3[] corners = new Vector3[4];
            corners[0] = new Vector3(-1, 0, -1);
            corners[1] = new Vector3(size.x, 0, -1);
            corners[2] = new Vector3(size.x, 0, size.z);
            corners[3] = new Vector3(-1, 0, size.z);
            line.loop = true;
            line.material = boundaryMaterial;
            line.startWidth = 0.2f;
            line.endWidth = 0.2f;
            line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            line.positionCount = 4;
            line.SetPositions(corners);
        }
        #endregion
    }
}
