using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public int currentChapter;

	void Start () 
    {
       
    }

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("ChapterSelection");
    }

    public void ChapterButtonClicked(int chapter)
    {   
        Debug.Log(chapter);
        PlayerPrefs.SetInt("currentChapter", chapter);
        SceneManager.LoadScene("Chapter" + chapter);
    }

    public void LevelButtonClicked(int level)
    {
        currentChapter = PlayerPrefs.GetInt("currentChapter");
        SceneManager.LoadScene("Level" + currentChapter + "_" + level);
    }

    public void BackToMenuButtonClicked()
    {
        Debug.Log("aaa");
        SceneManager.LoadScene("Menu");
    }
    public void BackToChapterSelcetion()
    {
        SceneManager.LoadScene("ChapterSelection");
    }
}
