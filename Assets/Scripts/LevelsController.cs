using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsController : MonoBehaviour {
    public Button[] lvlButtons;
    public Text chapterText;
    void Start()
    {
        Debug.Log("aaa");
        Debug.Log(Game.chapterNumber);
        chapterText.text = "Chapter " + Game.chapterNumber;
        int levelAt = PlayerPrefs.GetInt("levelAt", 2); /* < Change this int value to whatever your
                                                             level selection build index is on your
                                                             build settings */
        PlayerPrefs.SetInt("levelAt", 2); 
        /*
        for (int i = 0; i < lvlButtons.Length; i++)
        {
            if (i + 2 > levelAt)
                lvlButtons[i].interactable = false;
        }
        */
        
    }
    public void PlayButtonClicked(int level)
    {
		Game.levelNumber = level;
        SceneManager.LoadScene("Level" + Game.chapterNumber + "_" + Game.levelNumber);
    }
    
    public void BackButtonClicked()
    {
        SceneManager.LoadScene("ChapterSelection");
    }
}