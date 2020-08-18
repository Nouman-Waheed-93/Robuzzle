using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Robuzzle
{
    public class NavAgent : MonoBehaviour
    {
        #region Variables
        // agent speed
        [SerializeField]
        float speed = 5.0f;
        // Final distance from target
        [SerializeField]
        float accuracy = 1.0f;
        // agent rotation speed
        [SerializeField]
        float rotSpeed = 2.0f;
        // height that agent will maintain from ground
        [SerializeField]
        float standHeight = 0.2f;
        // the bottom to cast raycast for allevation
        [SerializeField]
        Transform bottom;
        //the force to apply to elevate agent4
        [SerializeField]
        Vector3 elevationForce;
        
        private PIDController heightPID;
        private RobuzzleGrid grid;
        private RigidbodyTile tile; //Agent has a movable tile because, it has to block path and also becauseanother agent can move onto it
        private Rigidbody rigidbody;
        int wayPointIndex;
        GameObject currentNode;
        #endregion
        #region Properties
        public int WayPointIndex { get => wayPointIndex; private set => wayPointIndex = value; }
        public GameObject CurrentNode { get => currentNode; private set => currentNode = value; }
        #endregion
        #region UnityCallbacks
        private void Start()
        {
            WayPointIndex = 0;
            heightPID = new PIDController();
            grid = (RobuzzleGrid)RobuzzleGrid.singleton;
            tile = GetComponent<RigidbodyTile>();
            rigidbody = GetComponent<Rigidbody>();
            currentNode = grid.GetNodeOnPosition(Vector3Int.FloorToInt(transform.position - Vector3.up));
            rigidbody.centerOfMass = new Vector3(0, -10, 0);
        }

        private void FixedUpdate()
        {
            MoveAgent();
        }
        #endregion
        #region Public Methods
        public void SetDestination(Vector3Int destination)
        {
            grid.RemoveTile(tile);
            currentNode = grid.GetNodeOnPosition(tile.Position - Vector3Int.up);
            GameObject destinationNode = grid.GetNodeOnPosition(destination);
            WayPointIndex = 0;
            grid.NavMesh.AStar(currentNode, destinationNode);
            grid.SetTilePosition(tile, tile.Position);
        }

        public void MoveAgent()
        {
            // If we've nowhere to go then just return
            if (grid.NavMesh.getPathLength() == 0 || WayPointIndex == grid.NavMesh.getPathLength())
                return;

            //the node we are closest to at this moment
            currentNode = grid.NavMesh.getPathPoint(WayPointIndex);

            //if we are close enough to the current waypoint move to next
            if (Vector3.Distance(
                grid.NavMesh.getPathPoint(WayPointIndex).transform.position,
                transform.position) < accuracy)
            {
                WayPointIndex++;
           //     charController.Move(Vector3.zero, false, false);
            }

            //if we are not at the end of the path
            if (WayPointIndex < grid.NavMesh.getPathLength())
            {
                Transform goal = grid.NavMesh.getPathPoint(WayPointIndex).transform;
                Vector3 direction = goal.position - this.transform.position;

                rigidbody.velocity = direction.normalized;
            }
        }
        #endregion
        #region Private Methods

        private void StandUpRight()
        {
            Ray ray = new Ray(bottom.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, standHeight))
            {
                float forcePercent = heightPID.Seek(standHeight, hit.distance);
                rigidbody.AddForce(elevationForce * forcePercent);
            }
        }

        #endregion
    }
}