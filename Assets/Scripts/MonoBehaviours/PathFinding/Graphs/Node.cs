using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RobuzzlePathFinding
{
    public class Node
    {
        public List<Edge> edgelist = new List<Edge>();
        public Node path = null;
        public GameObject id;
        public float xPos;
        public float yPos;
        public float zPos;
        public float f;
        public float g;
        public float h;
        public Node cameFrom;
        
        //Debug
        public TextMesh fTxt;
        public TextMesh gTxt;
        public TextMesh hTxt;

        //Debug
        public void UpdateText()
        {
            fTxt.text = f.ToString();
            gTxt.text = g.ToString();
            hTxt.text = h.ToString();
        }

        public Node(GameObject i)
        {
            //debug
            GameObject fGO = new GameObject(i.name + " f");
            fGO.transform.position = i.transform.position + new Vector3(0, 0, 0);
            fGO.transform.rotation = Quaternion.Euler(90, 0, 0);
            fTxt = fGO.AddComponent<TextMesh>();
            fTxt.fontSize = 50;
            fTxt.characterSize = 0.05f;
            fTxt.color = Color.red;

            //debug
            GameObject gGO = new GameObject(i.name + " g");
            gGO.transform.position = i.transform.position + new Vector3(-0.3f, 0, 0.3f);
            gGO.transform.rotation = Quaternion.Euler(90, 0, 0);
            gTxt = gGO.AddComponent<TextMesh>();
            gTxt.fontSize = 50;
            gTxt.characterSize = 0.05f;
            gTxt.color = Color.green;

            //debug
            GameObject hGO = new GameObject(i.name + " h");
            hGO.transform.position = i.transform.position + new Vector3(0.3f, 0, 0.3f);
            hGO.transform.rotation = Quaternion.Euler(90, 0, 0);
            hTxt = hGO.AddComponent<TextMesh>();
            hTxt.fontSize = 50;
            hTxt.characterSize = 0.05f;
            hTxt.color = Color.cyan;

            id = i;
            xPos = i.transform.position.x;
            yPos = i.transform.position.y;
            zPos = i.transform.position.z;
            path = null;
        }

        public GameObject getId()
        {
            return id;
        }

    }
}