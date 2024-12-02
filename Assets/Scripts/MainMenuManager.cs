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
        // SceneManager.LoadScene("Dungeon");
        SceneManager.LoadScene("Shop");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
