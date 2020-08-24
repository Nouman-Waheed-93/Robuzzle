using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using RobuzzlePathFinding;

namespace Robuzzle
{
    public class NavAgent : MonoBehaviour
    {
        #region Variables
        // agent speed
        [SerializeField]
        float speed = 5.0f;
        //the max drag to control speed
        [SerializeField]
        float maxDrag = 10;
        //the min drag to control speed
        [SerializeField]
        float minDrag = 1;
        //force to apply to stick to the tile, agent is standing on
        [SerializeField]
        float standingFirmness = 2;
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

        private List<Node> pathList = new List<Node>();
        private PIDController heightPID;
        private RobuzzleGrid grid;
        private RigidbodyTile tile; //Agent has a movable tile because, it has to block path and also becauseanother agent can move onto it
        private Rigidbody rigidbody;
        private float distanceToBottom; //The distance between center and the bottom
        private bool isOnGround;
        private Transform goal;
        private GameObject currNode;
        int wayPointIndex;
        GameObject nextNode;
        #endregion
        #region Properties
        public int WayPointIndex { get => wayPointIndex; private set => wayPointIndex = value; }
        public GameObject NextNode { get => nextNode; private set => nextNode = value; }
        #endregion
        #region UnityCallbacks
        private void Start()
        {
            WayPointIndex = 0;
            distanceToBottom = Vector3.Distance(transform.position, bottom.position);
            heightPID = new PIDController();
            grid = (RobuzzleGrid)RobuzzleGrid.singleton;
            grid.NavMesh.onNodeRemoved += NodeRemoved;
            tile = GetComponent<RigidbodyTile>();
            rigidbody = GetComponent<Rigidbody>();
            nextNode = grid.GetNodeOnPosition(Vector3Int.FloorToInt(transform.position - Vector3.up));
            currNode = NextNode;
            if (grid.NavMesh.AStar(currNode, nextNode))
                pathList = grid.NavMesh.reconstructPath(currNode, nextNode);
        }

        private void FixedUpdate()
        {
            StandUpRight();
            if(isOnGround)
            Debug.Log("On Ground" );
            if (isOnGround)
                MoveAgent();
            else
                rigidbody.drag = minDrag;
        }
        #endregion
        #region Public Methods
        public bool SetDestination(Vector3Int destination)
        {
            grid.RemoveTile(tile);
            nextNode = grid.GetNodeOnPosition(tile.Position - Vector3Int.up);
            GameObject destinationNode = grid.GetNodeOnPosition(destination);
            WayPointIndex = 0;
            bool retVal = grid.NavMesh.AStar(nextNode, destinationNode);
            if (retVal)
                pathList = grid.NavMesh.reconstructPath(nextNode, destinationNode);
            grid.SetTilePosition(tile, tile.Position);
            return retVal;
        }

        public void MoveAgent()
        {
            currNode = grid.GetNodeOnPosition(Vector3Int.FloorToInt(tile.Position - Vector3.up));
            if (currNode)
            {
                //if we are not at the end of the path
                if (WayPointIndex < getPathLength())
                {
                    //the node we are closest to at this moment
                    nextNode = getPathPoint(WayPointIndex);
                    goal = getPathPoint(WayPointIndex).transform;
                    //if we are close enough to the current waypoint move to next
                    if (Vector3.Distance(
                        getPathPoint(WayPointIndex).transform.position,
                        transform.position) < accuracy)
                    {
                        WayPointIndex++;
                    }
                }
                else
                   goal = currNode.transform;
                 

                Vector3 direction = goal.position - this.transform.position;
                Debug.DrawRay(transform.position, direction.normalized, Color.cyan);
                rigidbody.AddForce(direction.normalized * speed, ForceMode.VelocityChange);
            }
            else
                goal = null;
            LimitVelocity();
        }
        #endregion
        #region Private Methods

        private void NodeRemoved(Node node)
        {
            if (pathList.Contains(node) && node.getId() != nextNode)
            {
                int index = pathList.IndexOf(node);
                Debug.Log(index + " onwards");
                pathList.RemoveRange(index, pathList.Count - index);
            }
        }

        private int getPathLength()
        {
            return pathList.Count;
        }

        private GameObject getPathPoint(int index)
        {
            return pathList[index].id;
        }

        private void printPath()
        {
            foreach (Node n in pathList)
            {
                Debug.Log(n.id.name);
            }
        }

        private void LimitVelocity()
        {
            rigidbody.drag = Mathf.Lerp(minDrag, maxDrag, Mathf.Clamp01(rigidbody.velocity.sqrMagnitude / speed));
        }

        private void StandUpRight()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            float targetHeight = standHeight;
            if (goal != null)
            {
                float difference = goal.position.y - transform.position.y;
                if(difference > 0)
                    targetHeight += difference;
            }
            isOnGround = Physics.Raycast(ray, out hit, targetHeight + distanceToBottom);
            Debug.DrawRay(transform.position, Vector3.down * (targetHeight + distanceToBottom), Color.green);
            if (isOnGround)
            {
                float forcePercent = heightPID.Seek(targetHeight, hit.distance - distanceToBottom);
                rigidbody.AddForce(elevationForce * forcePercent);
            }
        }

        #endregion
    }
}