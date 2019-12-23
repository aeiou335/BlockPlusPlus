using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject[] blockys;
    void Start() 
    {
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
    }

    void Update() 
    {

    }

    public void ChapterSelectionButtonClicked()
    {   
        SceneManager.LoadScene("ChapterSelection");
    }

    public void CharacterSelectionButtonClicked()
    {
        SceneManager.LoadScene("CharacterSelection");
    }

}
