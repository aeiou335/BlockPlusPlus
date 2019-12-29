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
    public Text textKey;
    //public Text textDiamond;

    public bool paused;
    //public static int currentLevel;
    public int maxLevel = 6;
    public int currentLevel;
    public int starsCount = 1;
    public int expectedPuzzlesNumber;
    public string currentLevelPrefix;
	
	public GameObject[] coins;
    public GameObject[] portals;
	public GameObject[] diamonds;
    public GameObject[] keys;
    public GameObject[] doors;
    public GameObject[] doorsClosed;
    public GameObject[] blockys;
	public int scoreCoin, keyCount;//, scoreDiamond;
	
	//List<string> commands = new List<string>();
	
	void Awake() 
	{ 
		Game.level = this;
	}
	
    public void Start()
    {
        level.text = "Level " + Game.chapterNumber + "-" + Game.levelNumber;
		coins = GameObject.FindGameObjectsWithTag("Coin");
		diamonds = GameObject.FindGameObjectsWithTag("Diamond");
        keys = GameObject.FindGameObjectsWithTag("Key");
        doors = GameObject.FindGameObjectsWithTag("Door");
        doorsClosed = GameObject.FindGameObjectsWithTag("DoorClosed");
        portals = GameObject.FindGameObjectsWithTag("Portal");
		Reset();
    }
	
	void Reset() 
	{	
		scoreCoin = 0;
		//scoreDiamond = 0;
        starsCount = 1;
		textCoin.text = "x 0";
        textKey.text = "x 0";
		//textDiamond.text = "x 0";
		EnableRunButton();
        workspace.GetComponent<Canvas>().enabled = false;
        playPanel.GetComponent<Canvas>().enabled = true;
        CompletePanel.GetComponent<Canvas>().enabled = false;
        LosePanel.GetComponent<Canvas>().enabled = false;
		foreach (var coin in coins)
			coin.transform.localScale = new Vector3(1f, 1f, 1f);
		foreach (var diamond in diamonds)
			diamond.transform.localScale = new Vector3(1f, 1f, 1f);
        foreach (var key in keys)
			key.transform.localScale = new Vector3(2f, 2f, 2f);
        foreach (var door in doors)
            door.SetActive(false);
        foreach (var door in doorsClosed)
            door.SetActive(true);
        foreach (var portal in portals)
            portal.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        GameObject b = GameObject.Find("Blockys");
        blockys = new GameObject[b.transform.childCount];
        
        for (int i=0; i<b.transform.childCount; i++)
        {
            blockys[i] = b.transform.GetChild(i).gameObject;
        }
        foreach (GameObject blocky in blockys)
        {
            blocky.SetActive(false);
        }
        blockys[Game.characterNumber].SetActive(true);
        //Game.blocky = blockys[Game.characterNumber];
		
		// hide all UI buttons/texts for video
		if (Game.forVideo)
		{
			workspace.GetComponent<Canvas>().enabled = false;
			playPanel.GetComponent<Canvas>().enabled = false;
			CompletePanel.GetComponent<Canvas>().enabled = false;
			LosePanel.GetComponent<Canvas>().enabled = false;
			Invoke("_LoadChapter", 8f);
			return;
		}
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
        if (Game.commands.PuzzlesNumber() <= expectedPuzzlesNumber) starsCount += 1;
        if (coins.Length == scoreCoin) starsCount += 1;
        //Debug.Log(starsCount);
        //Debug.Log(GameObject.Find("Coins").transform.childCount);
        //Debug.Log(scoreCoin);
        //Debug.Log(Game.commands.PuzzlesNumber());
        //Debug.Log(expectedPuzzlesNumber);
        var stars = GameObject.Find("Stars");
        var noStars = GameObject.Find("NoStars");
        for (int i=0; i<starsCount; i++)
        {
            stars.transform.GetChild(i).gameObject.SetActive(true);
            noStars.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i=starsCount; i<GameObject.Find("Stars").transform.childCount; i++)
        {
            stars.transform.GetChild(i).gameObject.SetActive(false);
            noStars.transform.GetChild(i).gameObject.SetActive(true);
        }
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
        Game.blocky.Reset();
		Game.camera.Reset();
		Reset();
		Game.sound.play("CLICK");
    }
	
    public void NextLevel()
    {
        if (Game.levelNumber == maxLevel) {
            Invoke("_LoadChapter", 0.5f);
			Game.sound.play("CLICK");
        }
        else {
            Invoke("_LoadNextLevel", 0.5f);
			Game.sound.play("CLICK");
        }        
    }

    public void BackToMenu()
    {
		Invoke("_LoadChapter", 0.5f);
		Game.sound.play("CLICK");

        SceneManager.LoadScene("Menu");
    }
    private void LoadEndScreen()
    {
        SceneManager.LoadScene("EndMenu");
    }
    
    private void LoadNextScreen()
    {
        Game.levelNumber += 1;
        SceneManager.LoadScene("Level" + Game.chapterNumber + '_' + Game.levelNumber);		

    }
    
    public void Score(string type)
    {
		if (type == "COIN") scoreCoin += 1;
        if (type == "KEY") keyCount += 1;
		//if (type == "DIAMOND") scoreDiamond += 1;
		textCoin.text = "x " + scoreCoin;
        textKey.text = "x " + keyCount;
		//textDiamond.text = "x " + scoreDiamond;
    }

    public void Open(string type)
    {
        if (type == "KEY") keyCount -= 1;
        textKey.text = "x " + keyCount;
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
		Game.sound.play("CLICK");
	}
	
	// Stop button clicked
	public void OnStopClicked() 
	{
		Restart();
		Game.sound.play("CLICK");
	}
	
	// Quit button clicked
    public void OnQuitClicked()
    {
		Invoke("_LoadChapter", 0.5f);
		Game.sound.play("CLICK");
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
    
    void _LoadNextLevel()
    {
		Game.levelNumber += 1;
        SceneManager.LoadScene("Level" + Game.chapterNumber + '_' + Game.levelNumber);
    }
	
	void _LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void _LoadChapter()
    {
        SceneManager.LoadScene("Chapter");
    }
	
}