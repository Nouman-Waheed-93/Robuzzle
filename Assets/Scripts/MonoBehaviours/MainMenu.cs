using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Robuzzle.LevelBuilding;

public class MainMenu : MonoBehaviour
{
    enum LoadType { Game, Edit }
    [SerializeField]
    LoadType loadFor;

    [SerializeField]
    private Button button;
    [SerializeField]
    Transform buttonsParent;

    LevelFileHandler fileHandler;
    // Start is called before the first frame update
    void Start()
    {
        fileHandler = new LevelFileHandler();
        string[] levels = fileHandler.ListLevels();
        for(int i = 0; i < levels.Length; i++)
        {
            Button newBtn = Instantiate<Button>(button, buttonsParent);
            newBtn.GetComponentInChildren<Text>().text = levels[i];
            string str = levels[i];
            newBtn.onClick.AddListener(() => OnLevelBtnClicked(str));
        }
    }

    void OnLevelBtnClicked(string lvl)
    {
        LevelLoader.levelName = lvl;
        if (loadFor == LoadType.Game)
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            LevelLoader loader = FindObjectOfType<EditingLevelLoader>();
            loader.LoadLevel();
        }
    }
    
}
