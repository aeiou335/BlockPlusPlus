using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    //public bool gameOver = false;
    //public float restartLevelDelay = 2f;
    //public Color32 gameOverColor = new Color32(249, 92, 64, 1);
    //public GameObject fadeOutPanel;
    //public GameObject fadeInPanel;
    //public GameObject youWonPanel;
    //public GameObject youLosePanel;
    public GameObject workspace;
    public GameObject playPanel;
    public GameObject CompletePanel;
    public GameObject LosePanel;
    public GameObject runButton;
    public GameObject stopButton;
    public Text level;
    public Text textCoin;
    public Text textDiamond;

    public bool paused;
    //public static int currentLevel;
    public int maxLevel = 6;
    public int currentLevel;
    public string currentLevelPrefix;
	
	public GameObject[] coins;
	public GameObject[] diamonds;
	public int scoreCoin, scoreDiamond;
	
	List<string> commands = new List<string>();
	
	void Awake() 
	{ 
		Game.level = this;
	}
	
    public void Start()
    {
        level.text = "Level " + Game.chapterNumber + "-" + Game.levelNumber;
		coins = GameObject.FindGameObjectsWithTag("Coin");
		diamonds = GameObject.FindGameObjectsWithTag("Diamond");
		Reset();
    }
	
	void Reset() 
	{
		scoreCoin = 0;
		scoreDiamond = 0;
		textCoin.text = "x 0";
		textDiamond.text = "x 0";
		EnableRunButton();
        workspace.GetComponent<Canvas>().enabled = false;
        playPanel.GetComponent<Canvas>().enabled = true;
        CompletePanel.GetComponent<Canvas>().enabled = false;
        LosePanel.GetComponent<Canvas>().enabled = false;
		foreach (var coin in coins)
			coin.transform.localScale = new Vector3(1f, 1f, 1f);
		foreach (var diamond in diamonds)
			diamond.transform.localScale = new Vector3(1f, 1f, 1f);
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
		Reset();
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
        //int nextLevelNum = currentLevel + 1; 
        //Debug.Log(currentLevelPrefix + '_' + nextLevelNum);
        //SceneManager.LoadScene(currentLevelPrefix + '_' + nextLevelNum);
        SceneManager.LoadScene("Level" + Game.chapterNumber + '_' + Game.levelNumber);
		Game.levelNumber += 1;
    }
    
    public void Score(string type)
    {
		if (type == "COIN") scoreCoin += 1;
		if (type == "DIAMOND") scoreDiamond += 1;
		textCoin.text = "x " + scoreCoin;
		textDiamond.text = "x " + scoreDiamond;
    }
	
	// Zoom in button clicked
	public void OnZoomInClicked() 
	{
		Game.camera.ZoomIn();
	}
	
	// Zoom out button clicked
	public void OnZoomOutClicked() 
	{
		Game.camera.ZoomOut();
	}
	
	// Switch button clicked
	public void OnSwitchClicked() 
	{
		workspace.GetComponent<Canvas>().enabled ^= true;
		playPanel.GetComponent<Canvas>().enabled ^= true;
	}
	
	// Stop button clicked
	public void OnStopClicked() 
	{
		Restart();
	}
	
	// Quit button clicked
    public void OnQuitClicked()
    {
        SceneManager.LoadScene("Chapter");
    }
	
	// Enable run button
	public void EnableRunButton() 
	{
		runButton.GetComponent<Button>().interactable = true;
		stopButton.GetComponent<Button>().interactable = false;
	}
	
	// Enable stop button
	public void EnableStopButton() 
	{
		runButton.GetComponent<Button>().interactable = false;
		stopButton.GetComponent<Button>().interactable = true;
	}
    
}