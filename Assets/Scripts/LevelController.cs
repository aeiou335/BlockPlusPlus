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
    //public GameObject youlosePanel;
    public GameObject workspace;
    public GameObject playPanel;
    public GameObject completePanel;
    public GameObject losePanel;
    public GameObject helpPanel;
    public GameObject runButton;
    public GameObject stopButton;
    public Text level;
    public Text textCoin;
    public Text textKey;
    public Text textMin;
	public Sprite[] helpImages;
    //public Text textDiamond;

    public bool paused;
    //public static int currentLevel;
    public int maxLevel = 6;
    public int currentLevel;
    public int starsCount = 1;
    public int expectedPuzzlesNumber;
    public string currentLevelPrefix;
	
	public GameObject[] coins;
    public GameObject[] portals, portals1, portals2;
	public GameObject[] diamonds;
    public GameObject[] keys;
    public GameObject[] doors;
    public GameObject[] doorsClosed;
    public GameObject[] blockys;
	public int scoreCoin, keyCount;//, scoreDiamond;
	
	int helpIndex;
	
	//List<string> commands = new List<string>();
	
	void Awake() 
	{ 
		Game.level = this;
	}
	
    public void Start()
    {
        level.text = "Level " + Game.chapterNumber + "-" + Game.levelNumber;
		textMin.text = "" + expectedPuzzlesNumber;
		coins = GameObject.FindGameObjectsWithTag("Coin");
		diamonds = GameObject.FindGameObjectsWithTag("Diamond");
        keys = GameObject.FindGameObjectsWithTag("Key");
        doors = GameObject.FindGameObjectsWithTag("Door");
        doorsClosed = GameObject.FindGameObjectsWithTag("DoorClosed");
        portals = GameObject.FindGameObjectsWithTag("Portal");
        portals1 = GameObject.FindGameObjectsWithTag("Portal1");
        portals2 = GameObject.FindGameObjectsWithTag("Portal2");
		if (keys.Length == 0)
		{
			GameObject.Find("ImageKey").GetComponent<Image>().enabled = false;
			GameObject.Find("TextKey").GetComponent<Text>().enabled = false;
		}
		Reset();
    }
	
	void Reset() 
	{	
		scoreCoin = 0;
        keyCount = 0;
		//scoreDiamond = 0;
        starsCount = 1;
		textCoin.text = "0/" + coins.Length;
        textKey.text = "0";
		if (keys.Length == 0) textKey.text = "-";
		//textDiamond.text = "x 0";
		EnableRunButton();
        workspace.GetComponent<Canvas>().enabled = false;
        playPanel.GetComponent<Canvas>().enabled = true;
        completePanel.GetComponent<Canvas>().enabled = false;
        losePanel.GetComponent<Canvas>().enabled = false;
        helpPanel.GetComponent<Canvas>().enabled = false;
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
        foreach (var portal1 in portals1)
            portal1.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        foreach (var portal2 in portals2)
            portal2.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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
		if (Game.mode == "VIDEO1") 
		{
			workspace.GetComponent<Canvas>().enabled = false;
			playPanel.GetComponent<Canvas>().enabled = false;
			completePanel.GetComponent<Canvas>().enabled = false;
			losePanel.GetComponent<Canvas>().enabled = false;
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
        if (coins.Length == scoreCoin)
        {
            starsCount += 1;
            if (Game.commands.PuzzlesNumber() <= expectedPuzzlesNumber) starsCount += 1;
        }
        
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
        completePanel.GetComponent<Canvas>().enabled = true;
        /*
        if (Game.levelNumber+1 > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", Game.levelNumber+1);
        }
        */   
    }

    public void FailLevel()
    {     
		workspace.GetComponent<Canvas>().enabled = false;
		playPanel.GetComponent<Canvas>().enabled = true;
        losePanel.GetComponent<Canvas>().enabled = true;
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
		//SceneManager.LoadScene("Menu");
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
		textCoin.text = scoreCoin + "/" + coins.Length;
        textKey.text = "" + keyCount;
		//textDiamond.text = "x " + scoreDiamond;
    }

    public void Open(string type)
    {
        if (type == "KEY") keyCount -= 1;
        textKey.text = "" + keyCount;
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
	
	// Help button clicked
    public void OnHelpClicked()
    {
		workspace.GetComponent<Canvas>().enabled = false;
		playPanel.GetComponent<Canvas>().enabled = false;
		helpPanel.GetComponent<Canvas>().enabled = true;
		helpIndex = 1;
		RefreshHelp();
		Game.sound.play("CLICK");
    }
	
	// HelpQuit button clicked
    public void OnHelpQuitClicked()
    {
		workspace.GetComponent<Canvas>().enabled = false;
		playPanel.GetComponent<Canvas>().enabled = true;
		helpPanel.GetComponent<Canvas>().enabled = false;
		Game.sound.play("CLICK");
    }
	
	// HelpBack button clicked
    public void OnHelpBackClicked()
    {
		helpIndex -= 1;
		RefreshHelp();
		Game.sound.play("CLICK");
    }
	
	// HelpNext button clicked
    public void OnHelpNextClicked()
    {
		helpIndex += 1;
		RefreshHelp();
		Game.sound.play("CLICK");
    }
	
	// Refresh Help
    private void RefreshHelp()
    {
		var N = 16;
		var helpMessages = new string[]
		{
			"", // 0
			"Welcome to the tutorial!",
			"This is the game scene.", // 2
			"Hello! Blocky~",
			"Get the diamond.", // 4
			"Collect all coins.",
			"Drag, and zoom.", // 6
			"Switch to the workspace.",
			"This is the workspace.", // 8
			"These are command puzzles.",
			"Connect the puzzles.", // 10
			"Drag the puzzle.",
			"That's it, connected.", // 12
			"Rubbish bin here.",
			"Run your commands.", // 14
			"Target number of puzzles.",
			"Here we go! Block++", // 16
		};
		if (helpIndex < 1) helpIndex = 1;
		if (helpIndex > N) helpIndex = N;
		GameObject.Find("HelpTitle").GetComponent<Text>().text = "Tutorial " + helpIndex + "/" + N;
		GameObject.Find("HelpMessage").GetComponent<Text>().text = helpMessages[helpIndex];
		GameObject.Find("HelpImage").GetComponent<Image>().sprite = helpImages[helpIndex];
		GameObject.Find("HelpBack").GetComponent<Image>().enabled = (helpIndex != 1);
		GameObject.Find("HelpNext").GetComponent<Image>().enabled = (helpIndex != N);
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