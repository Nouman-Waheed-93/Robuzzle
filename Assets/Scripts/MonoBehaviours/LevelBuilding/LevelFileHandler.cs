using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Robuzzle.LevelBuilding
{
    public class LevelFileHandler
    {

        string levelDirectory;

        #region Constructor

        public LevelFileHandler()
        {
            levelDirectory = Application.dataPath + "/Resources/Levels";
            if (!Directory.Exists(levelDirectory))
            {
                Directory.CreateDirectory(levelDirectory);
            }
        }

        #endregion

        #region Public Methods

        public void SaveLevel(IntegerGrid grid, string name)
        {
            string fileData = JsonUtility.ToJson(grid);
            File.WriteAllText(levelDirectory + "/" + name + ".lvl", fileData);
        }

        public string[] ListLevels()
        {
            string[] fileNames = Directory.GetFiles(@levelDirectory, "*.lvl");
            for(int i = 0; i < fileNames.Length; i++)
            {
                Debug.Log("Old name " + fileNames[i]);
                fileNames[i] = fileNames[i].Substring(fileNames[i].LastIndexOf('\\') + 1);
                fileNames[i] = fileNames[i].Substring(0, fileNames[i].Length - 4);
                Debug.Log("New name " + fileNames[i]);
            }
            return fileNames;
        }

        public void DeleteLevel()
        {

        }

        public string LoadLevel(string name)
        {
            string returnString = "";
            string fileName = levelDirectory + "/" + name + ".lvl";
            if (File.Exists(fileName))
                returnString = File.ReadAllText(fileName);
            return returnString;
        }
        #endregion
    }
}
