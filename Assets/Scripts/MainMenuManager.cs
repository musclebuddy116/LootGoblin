using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] ScreenFader mainMenuScreenFader;
    public void StartGame() {
        mainMenuScreenFader.FadeToColor();
        StartCoroutine(DelayLeaveMenuAfterFade());
    }

    IEnumerator DelayLeaveMenuAfterFade() {
        yield return new WaitUntil(()=>mainMenuScreenFader.DoneFadingToColor());
        int newGame = PlayerPrefs.GetInt(Strings.newGame);
        if(newGame < 2) {
            PlayerPrefs.SetInt(Strings.newGame, 2);
            SceneManager.LoadScene(Strings.tutorialScene);
        } else {
            SceneManager.LoadScene(Strings.shopScene);
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
