using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    
    [SerializeField] Transform fadeTransform;
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeTime = 1;
    [SerializeField] bool fadeFromColorOnStart = false;

    bool fading = false;
    bool doneFadingToColor = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if(fadeFromColorOnStart) {
            FadeFromColor();
        }
    }

    public void FadeToColor() {
        if(fading) {
            return;
        }
        fading = true;
        StartCoroutine(FadeToColorRoutine());
        IEnumerator FadeToColorRoutine() {
            float t = 0;
            while(t < fadeTime) {
                yield return null;
                t += Time.deltaTime;
                fadeTransform.localScale = new Vector3(2000f * t, 1200f * t, 1);
            }

            fadeTransform.localScale = new Vector3(2000, 1200, 1);
            fading = false;
            doneFadingToColor = true;
            yield return null;
        }
    }

    public bool DoneFadingToColor() {
        return doneFadingToColor;
    }

    public void FadeFromColor() {
        if(fading) {
            return;
        }
        fading = true;
        StartCoroutine(FadeFromColorRoutine());
        IEnumerator FadeFromColorRoutine() {
            float t = 0;
            fadeTransform.localScale = new Vector3(2000, 1200, 1);
            while(t < fadeTime) {
                yield return null;
                t += Time.deltaTime;
                fadeTransform.localScale = new Vector3(2000f-(2000f * t), 1200f-(1200f * t), 1);
            }

            fadeTransform.localScale = new Vector3(0, 0, 1);
            fading = false;
            yield return null;
        }
    }
}
