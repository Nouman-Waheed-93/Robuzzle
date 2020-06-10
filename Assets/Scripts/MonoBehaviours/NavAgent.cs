using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private RobuzzleGrid grid;
        private MovableTile tile; //Agent has a movable tile because, it has to block path and also becauseanother agent can move onto it
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
            grid = (RobuzzleGrid)RobuzzleGrid.singleton;
            tile = GetComponent<MovableTile>();
            currentNode = grid.GetNodeOnPosition(Vector3Int.FloorToInt(transform.position - Vector3.up));
        }

        private void Update()
        {
            if(Input.GetKey(KeyCode.Space))
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
            }

            //if we are not at the end of the path
            if (WayPointIndex < grid.NavMesh.getPathLength())
            {
                Transform goal = grid.NavMesh.getPathPoint(WayPointIndex).transform;
                Vector3 lookAtGoal = new Vector3(goal.position.x,
                                                this.transform.position.y,
                                                goal.position.z);
                Vector3 direction = lookAtGoal - this.transform.position;

                // Rotate towards the heading
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                        Quaternion.LookRotation(direction),
                                                        Time.deltaTime * rotSpeed);

                // Move the agent
                this.transform.Translate((goal.position - transform.position).normalized * speed * Time.deltaTime, Space.World);
                tile.UpdatePosition();
            }
        }
        #endregion
        #region Private Methods

        #endregion
    }
}