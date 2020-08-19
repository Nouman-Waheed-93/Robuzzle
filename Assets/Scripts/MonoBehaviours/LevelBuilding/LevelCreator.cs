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
        public GameObject currMenu;
        public Material boundaryMaterial;

        [SerializeField]
        private TileTypes tileTypes;

        [SerializeField]
        private GameObject mainTileMenu;
        ViewHandler viewHandler;
        #endregion
        #region Public Methods

        public void Init(Vector3Int size, string name)
        {
            CreateGrid(size);
            this.name = name;
            CreateVisualBoundaries(size);
            viewHandler = ViewHandler.singleton;
            viewHandler.Init(size);
        }

        public void HideMainMenu()
        {
            mainTileMenu.SetActive(false);
        }

        public void ToMainTileMenu()
        {
            currMenu.SetActive(false);
            mainTileMenu.SetActive(true);
        }

        public void SelectTile(MenuTile tile)
        {
            selectedTile = tile;
        }
        
        public void PlaceTile(Vector3Int position)
        {
            if (grid.CreateTile(selectedTile.tileType, position))
            {
                Tile tile = Instantiate(tileTypes.GetPrefab(selectedTile.tileType), position, Quaternion.identity);
                GameObject tileGO = tile.gameObject;
                MonoBehaviour[] behaviorsOnTile = tileGO.GetComponents<MonoBehaviour>();
                for(int i = 0; i < behaviorsOnTile.Length; i++)
                {
                    Destroy(behaviorsOnTile[i]);
                }
                Joint[] joints = tileGO.GetComponents<Joint>();
                for(int i = 0; i < joints.Length; i++)
                {
                    Destroy(joints[i]);
                }
                Rigidbody rb;
                if(tileGO.TryGetComponent<Rigidbody>(out rb))
                {
                    Destroy(rb);
                }
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
            corners[0] = new Vector3(-1, -0.4f, -1);
            corners[1] = new Vector3(size.x, -0.4f, -1);
            corners[2] = new Vector3(size.x, -0.4f, size.z);
            corners[3] = new Vector3(-1, -0.4f, size.z);
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
