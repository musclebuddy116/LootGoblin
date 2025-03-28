using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    [SerializeField] bool closedByDefault = true;

    void Awake()
    {
        int newGame = PlayerPrefs.GetInt(Strings.newGame,0);
        if(newGame == 0 ) {
            PlayerPrefs.SetInt(Strings.newGame, 1);
            closedByDefault = false;
            OpenMenu();
        }
        if(closedByDefault) {
            CloseMenu();
        }
    }
    
    public void OpenMenu() {
        GetComponent<Canvas>().enabled = true;

    }

    public void CloseMenu() {
        GetComponent<Canvas>().enabled = false;
    }
}
