using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndMenuController : MonoBehaviour {

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("Level01");
    }

    public void NextButtonClicked() {
        SceneManager.LoadScene("Level0"+Game.levelNumber);
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }
}
