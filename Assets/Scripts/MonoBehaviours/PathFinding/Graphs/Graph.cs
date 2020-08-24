using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace RobuzzlePathFinding
{
    public class Graph
    {
        public Action<Node> onNodeRemoved;

        List<Edge> edges = new List<Edge>();
        List<Node> nodes = new List<Node>();

        public Graph() { }

        public void AddNode(GameObject id, bool removeRenderer = true, bool removeCollider = true, bool removeId = true)
        {
            Node node = new Node(id);
            nodes.Add(node);

            //remove colliders and mesh renderer
            if (removeCollider)
                GameObject.Destroy(id.GetComponent<Collider>());
            if (removeRenderer)
                GameObject.Destroy(id.GetComponent<Renderer>());
            if (removeId)
            {
                TextMesh[] textms = id.GetComponentsInChildren<TextMesh>() as TextMesh[];

                foreach (TextMesh tm in textms)
                    GameObject.Destroy(tm.gameObject);
            }
        }

        public void RemoveNode(GameObject id)
        {
            //find the node to delete
            Node nodeToDelete = findNode(id);
            //remove all edges that start or end at this node
            Edge[] edgesToDelete = edges.Where(edge => edge.startNode == nodeToDelete || edge.endNode == nodeToDelete).ToArray();
            for(int i = 0; i < edgesToDelete.Length; i++)
            {
                //Remove the edge from the list of outgoing edges the neighbor has to this node
                if (edgesToDelete[i].startNode != nodeToDelete)
                    edgesToDelete[i].startNode.edgelist.Remove(edgesToDelete[i]);

                edges.Remove(edgesToDelete[i]);
            }

            //delete the node
            nodes.Remove(nodeToDelete);

            if (onNodeRemoved != null)
                onNodeRemoved(nodeToDelete);
        }

        public void AddEdge(GameObject fromNode, GameObject toNode)
        {
            Node from = findNode(fromNode);
            Node to = findNode(toNode);

            if (from != null && to != null)
            {
                Edge e = new Edge(from, to);
                edges.Add(e);
                from.edgelist.Add(e);
            }
        }

        public void RemoveEdge(GameObject fromNode, GameObject toNode)
        {
            Node from = findNode(fromNode);
            Node to = findNode(toNode);
            edges.Remove(FindEdge(fromNode, toNode));
        }

        public Edge FindEdge(GameObject fromNode, GameObject toNode)
        {
            Node fromN = findNode(fromNode);
            Node toN = findNode(toNode);
            var result = from edge in edges where edge.startNode == fromN && edge.endNode == toN select edge;
            return result.FirstOrDefault();
        }

        Node findNode(GameObject id)
        {
            foreach (Node n in nodes)
            {
                if (n.getId() == id)
                    return n;
            }
            return null;
        }
        
        public bool AStar(GameObject startId, GameObject endId)
        {
            Node start = findNode(startId);
            Node end = findNode(endId);

            if (start == null || end == null)
            {
                return false;
            }
            
            List<Node> open = new List<Node>();
            List<Node> closed = new List<Node>();
            float tentative_g_score = 0;
            bool tentative_is_better;

            start.g = 0;
            start.h = distance(start, end);
            start.f = start.h;
            open.Add(start);

            while (open.Count > 0)
            {
                int i = lowestF(open);
                Node thisnode = open[i];
                if (thisnode.id == endId)  //path found
                {
                    return true;
                }

                open.RemoveAt(i);
                closed.Add(thisnode);

                Node neighbour;
                foreach (Edge e in thisnode.edgelist)
                {
                    neighbour = e.endNode;
                 //   neighbour.g = thisnode.g + distance(thisnode, neighbour);
                    if (closed.IndexOf(neighbour) > -1)
                        continue;
                    
                    tentative_g_score = thisnode.g + distance(thisnode, neighbour);
                    
                    if (open.IndexOf(neighbour) == -1)
                    {
                        open.Add(neighbour);
                        tentative_is_better = true;
                    }
                    else if (tentative_g_score < neighbour.g)
                    {
                        tentative_is_better = true;
                    }
                    else
                        tentative_is_better = false;

                    if (tentative_is_better)
                    {
                        neighbour.cameFrom = thisnode;
                        neighbour.g = tentative_g_score;
                        //neighbour.h = distance(thisnode,neighbour);
                        neighbour.h = distance(neighbour, end);
                        neighbour.f = neighbour.g + neighbour.h;
                    }
                }

            }

            return false;
        }

        public List<Node> reconstructPath(GameObject startNode, GameObject endNode)
        {
            Node startId = findNode(startNode);
            Node endId = findNode(endNode);

            return reconstructPath(startId, endId);
        }

        public List<Node> reconstructPath(Node startId, Node endId)
        {
            List<Node> pathList = new List<Node>();
            pathList.Add(endId);

            var p = endId.cameFrom;
            while (p != startId && p != null)
            {
                pathList.Insert(0, p);
                p = p.cameFrom;
            }
            pathList.Insert(0, startId);
            return pathList;
        }

        float distance(Node a, Node b)
        {
            float dx = Mathf.Abs(a.xPos - b.xPos);
            //float dy = a.yPos - b.yPos;
            float dy = 0;
            float dz = Mathf.Abs(a.zPos - b.zPos);
            float dist = dx + dy + dz;
            return (dist);
        }

        int lowestF(List<Node> l)
        {
            float lowestf = 0;
            int count = 0;
            int iteratorCount = 0;

            for (int i = 0; i < l.Count; i++)
            {
                if (i == 0)
                {
                    lowestf = l[i].f;
                    iteratorCount = count;
                }
                else if (l[i].f <= lowestf)
                {
                    lowestf = l[i].f;
                    iteratorCount = count;
                }
                count++;
            }
            return iteratorCount;
        }

        public void debugDraw()
        {
            //draw edges
            for (int i = 0; i < edges.Count; i++)
            {
                Debug.DrawLine(edges[i].startNode.id.transform.position, edges[i].endNode.id.transform.position, Color.red);

            }
            //draw directions
            for (int i = 0; i < edges.Count; i++)
            {
                Vector3 to = (edges[i].startNode.id.transform.position - edges[i].endNode.id.transform.position) * 0.05f;
                Debug.DrawRay(edges[i].endNode.id.transform.position, to, Color.blue);
            }
        }
    }
}