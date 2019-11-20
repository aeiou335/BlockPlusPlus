using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {
	void Start () {
    }

    public void PlayButtonClicked(int level)
    {
        SceneManager.LoadScene(level+1);
    }

}