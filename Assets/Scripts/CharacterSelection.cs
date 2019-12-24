using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {
    public GameObject scrollbar;
    public GameObject[] blockys;
    float scrollPos = 0;
    float[] pos;

    void Start() {

    }

    void Update() {
        pos = new float[transform.childCount];
        float distance = 1f/ (pos.Length - 1f);
        //Debug.Log(distance);
        
        //Debug.Log(pos.Length);
        for (int i=0; i<pos.Length; i++) {
            pos[i] = distance * i;
        }
        if (Input.GetMouseButton(0)) {
            scrollPos = scrollbar.GetComponent<Scrollbar>().value;
            //Debug.Log(scrollPos);   
        } else {
            for (int i=0; i<pos.Length; i++) {
                if (scrollPos < pos[i] + (distance/2) && scrollPos > pos[i] - distance / 2) {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i=0; i<pos.Length; i++) {
            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2)) {
                transform.GetChild(i).localScale = Vector3.Lerp (transform.GetChild(i).localScale, new Vector3(250f, 250f, 250f), 10f);
                for (int a=0; a<pos.Length; a++) {
                    if (a != i) {
                        transform.GetChild(a).localScale = Vector3.Lerp(transform.GetChild(a).localScale, new Vector3(175f, 175f, 175f), 10f);
                    }
                }
            }
        }
        /*
        for (int i=0; i<blockys.Length; i++)
        {
            Debug.Log(i);
            Debug.Log(blockys[i].transform.position);
        }
        */
    }

    public void SelectCharacter() 
    {
        for (int i=0; i<blockys.Length; i++)
        {
            if (blockys[i].transform.position.x < 5.0f && blockys[i].transform.position.x > -5.0f)
            {
                Game.characterNumber = i;
                Debug.Log(i);
                break;
            }
        }
        Debug.Log(Game.characterNumber);
		Invoke("_LoadMenu", 0.5f);
		Game.sound.play("CLICK");
    }

    public void BackToMenuButtonClicked()
    {
		Invoke("_LoadMenu", 0.5f);
		Game.sound.play("CLICK");
    }

    void _LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}