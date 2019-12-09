using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    void Start() {

    }

    void Update() 
    {

    }

    public void ChapterSelectionButtonClicked()
    {   
        SceneManager.LoadScene("ChapterSelection");
    }

}
