using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Robuzzle.LevelBuilding
{
    public class MenuButtons : MonoBehaviour
    {
        #region Variables
        public GameObject mainMenu;
        public GameObject levelCreationMenu;

        public InputField levelNameField;
        public InputField sizeField;

        public Text rangeMessage;
        LevelCreator levelCreator;
        LevelFileHandler fileHandler;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            levelCreator = LevelCreator.singleton;
            fileHandler = new LevelFileHandler();
            levelCreator.RangeExceeded += RangeExceeded;
        }

        #endregion

        #region Public Methods

        public void ToNewLevel()
        {

        }
        
        public void ToEditExistingLevel()
        {

        }

        public void ToMainLevelEditingMenu()
        {

        }

        public void CreateLevel()
        {
            levelCreator.Init(Vector3Int.one * Int32.Parse(sizeField.text), levelNameField.text);
            levelCreationMenu.SetActive(false);
        }
        
        public void SaveLevel()
        {
            fileHandler.SaveLevel(levelCreator.grid.grid, levelCreator.name);
        }

        #endregion

        #region Private Methods

        private void RangeExceeded()
        {
            StartCoroutine(ShowMaxRangeMessage());
        }

        private IEnumerator ShowMaxRangeMessage()
        {
            rangeMessage.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1.5f);
            rangeMessage.gameObject.SetActive(false);
        }

        #endregion
    }
}