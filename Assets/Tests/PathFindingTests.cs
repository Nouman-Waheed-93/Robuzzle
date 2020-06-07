using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RobuzzlePathFinding;
using UnityEngine.TestTools;

namespace Tests
{
    public class PathFindingTests
    {
        Graph graph;
        GameObject gameObject1;
        GameObject gameObject2;

        [SetUp]
        public void SetUp()
        {
            graph = new Graph();
            gameObject1 = new GameObject();
            gameObject2 = new GameObject();
            graph.AddNode(gameObject1);
            graph.AddNode(gameObject2);
            graph.AddEdge(gameObject1, gameObject2);
            graph.AddEdge(gameObject2, gameObject1);
        }

        // A Test behaves as an ordinary method
        [UnityTest]
        public IEnumerator PathFindingEdge1Added()
        {
            yield return null;
            Assert.IsNotNull(graph.FindEdge(gameObject1, gameObject2));
        }

        // A Test behaves as an ordinary method
        [UnityTest]
        public IEnumerator PathFindingEdge2Added()
        {
            yield return null;
            Assert.IsNotNull(graph.FindEdge(gameObject2, gameObject1));
        }

        // A Test behaves as an ordinary method
        [UnityTest]
        public IEnumerator PathFindingEdge1Removed()
        {
            graph.RemoveEdge(gameObject1, gameObject2);
            yield return null;
            Assert.IsNull(graph.FindEdge(gameObject1, gameObject2));
        }

        // A Test behaves as an ordinary method
        [UnityTest]
        public IEnumerator PathFindingEdge2Removed()
        {
            graph.RemoveEdge(gameObject2, gameObject1);
            yield return null;
            Assert.IsNull(graph.FindEdge(gameObject2, gameObject1));
        }

        [UnityTest]
        public IEnumerator Edge1RemovedNAdded()
        {
            graph.RemoveEdge(gameObject1, gameObject2);
            graph.AddEdge(gameObject1, gameObject2);
            yield return null;
            Assert.IsNotNull(graph.FindEdge(gameObject1, gameObject2));
        }

        [UnityTest]
        public IEnumerator CheckEdge1AftrNode1Remove()
        {
            graph.RemoveNode(gameObject1);
            yield return null;
            Assert.IsNull(graph.FindEdge(gameObject1, gameObject2));
        }

        [UnityTest]
        public IEnumerator CheckEdge2AftrNode1Remove()
        {
            graph.RemoveNode(gameObject1);
            yield return null;
            Assert.IsNull(graph.FindEdge(gameObject2, gameObject1));
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(gameObject1);
            GameObject.Destroy(gameObject2);
        }
    }
}
