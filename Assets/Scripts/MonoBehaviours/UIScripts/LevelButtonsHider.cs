using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Robuzzle.LevelBuilding;

namespace UI
{
    public class LevelButtonsHider : MonoBehaviour
    {
        private void Start()
        {
            LevelLoader.LevelLoaded += InactiveMenu;
        }

        private void InactiveMenu()
        {
            gameObject.SetActive(false);
        }
    }
}