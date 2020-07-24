using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Robuzzle
{
    public class GameInput : AbstractInput
    {
        #region Variables
        [SerializeField]
        LayerMask rayCastLayer;

        NavAgent player;
        Draggable selectedDraggable;
        Camera cam;
        #endregion

        #region Unity Callbacks

        private void Start()
        {
            base.Start();
            Invoke("Init", 0.12f);
        }

        void Init()
        {
            cam = Camera.main;
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO)
            {
                player = playerGO.GetComponent<NavAgent>();
            }
        }

        void Update()
        {
            GetPointerPosDelta();
            if (inputHandler.BtnDown())
            {
                Ray ray = cam.ScreenPointToRay(inputHandler.GetPointerPosition());
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, rayCastLayer))
                {
                    hit.collider.TryGetComponent(out selectedDraggable);
                    if (selectedDraggable)
                        selectedDraggable.StartDrag();
                }
            }
            if (inputHandler.BtnUp())
            {
                if (selectedDraggable)
                {
                    selectedDraggable.EndDrag();
                    selectedDraggable = null;
                }
            }
            if (inputHandler.IsTapped())
            {
                Ray ray = cam.ScreenPointToRay(inputHandler.GetPointerPosition());
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, rayCastLayer))
                {
                    Tile targetTile;
                    hit.collider.TryGetComponent<Tile>(out targetTile);
                    if(targetTile != null)
                    {
                        player.SetDestination(targetTile.Position);
                    }
                }
            }
            if (inputHandler.IsSingleDragging() && selectedDraggable)
            {
                Ray ray = cam.ScreenPointToRay(inputHandler.GetPointerPosition());
                Vector3 targetLocation = ray.GetPoint(Vector3.Distance(cam.transform.position, selectedDraggable.transform.position));
                selectedDraggable.Move(targetLocation);
            }
            else if (PanView()) { }
            else if (RotateView()) { }
            else if (ZoomView()) { }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}