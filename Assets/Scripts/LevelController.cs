using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    public bool gameOver = false;
    public float restartLevelDelay = 2f;
    public Color32 gameOverColor = new Color32(249, 92, 64, 1);
    public GameObject fadeOutPanel;
    public GameObject fadeInPanel;
    public GameObject youWonPanel;
    public GameObject youLosePanel;
    public GameObject CompletePanel;
    public GameObject LosePanel;
    public Text level;
    public Text scoreText;

    public bool paused;
    //public static int currentLevel;
    public int maxLevel = 6;
    public int currentLevel;
    public string currentLevelPrefix;
	
	public GameObject[] coins;
	public int scoreCoin, scoreDiamond;
	
    public void Start()
    {
		Game.level = this;
        fadeInPanel.SetActive(true);
        CompletePanel.GetComponent<Canvas>().enabled = false;
        LosePanel.GetComponent<Canvas>().enabled = false;
        string[] levelName = SceneManager.GetActiveScene().name.Split('_');
        currentLevelPrefix = levelName[0];
        currentLevel = int.Parse(levelName[1]);
        //Debug.Log(currentLevel.Length);
        level.text = "Level " + currentLevel;
        //currentLevel = SceneManager.GetActiveScene().buildIndex;
		
		coins = GameObject.FindGameObjectsWithTag("Coin");
		scoreCoin = 0; scoreDiamond = 0;
		scoreText.text = "Coin:0 Diamond:0";
		//Restart();
    }

    public void Update()
    {
        // Pause

        if (Input.GetKeyDown("p"))
        {
            if (Time.timeScale != 0)
            {
                paused = true;
                Time.timeScale = 0; // pause game
                GetComponent<AudioSource>().Pause(); // pause music
            }
            else
            {
                paused = false;
                Time.timeScale = 1; // unpause
                GetComponent<AudioSource>().Play(); // unpause music
            }
        }
        // Win (tmp)
        if (Input.GetKeyDown("w")) {
            CompleteLevel();
        }
        if (Input.GetKeyDown("l"))
        {
            FailLevel();
        }
    }


    public void CompleteLevel()
    {      
        CompletePanel.GetComponent<Canvas>().enabled = true;
        /*
        if (Game.levelNumber+1 > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", Game.levelNumber+1);
        }
        */   
    }

    public void FailLevel()
    {     
        LosePanel.GetComponent<Canvas>().enabled = true;
    }

    public void Restart()
    {
        //fadeOutPanel.SetActive(true);
        Game.blocky.Reset();
		Game.camera.Reset();
        CompletePanel.GetComponent<Canvas>().enabled = false;
        LosePanel.GetComponent<Canvas>().enabled = false;
		scoreCoin = 0; scoreDiamond = 0;
		scoreText.text = "Coin:0 Diamond:0";
		foreach (var coin in coins)
			coin.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void ReloadScreen()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        if (currentLevel == maxLevel) {
            Invoke("LoadEndScreen", 1);
        }
        else {
            Invoke("LoadNextScreen", 1);
        }        
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    private void LoadEndScreen()
    {
        SceneManager.LoadScene("EndMenu");
    }
    
    private void LoadNextScreen()
    {
        int nextLevelNum = currentLevel + 1; 
        Debug.Log(currentLevelPrefix + '_' + nextLevelNum);
        SceneManager.LoadScene(currentLevelPrefix + '_' + nextLevelNum);
    }
    
    public void Score(string type)
    {
		if (type == "COIN") scoreCoin += 1;
		if (type == "DIAMOND") scoreDiamond += 1;
		scoreText.text = "Coin:" + scoreCoin + " Diamond:" + scoreDiamond;
    }
    
}