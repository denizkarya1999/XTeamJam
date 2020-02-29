using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuObject;
    public GameObject LevelSelect;

    private void Start()
    {
        MainMenuObject.SetActive(true);
        LevelSelect.SetActive(false);
    }

    public void NewGameClick() {
        SceneManager.LoadScene("SampleScene");
    }

    public void LevelSelectClick()
    {
        MainMenuObject.SetActive(false);
        LevelSelect.SetActive(true);
    }

    public void SelectLevelByName(string levelName) {
        //SceneManager.LoadScene("levelName");
        SceneManager.LoadScene(levelName);
    }
}
