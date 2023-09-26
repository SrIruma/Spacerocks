using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceRocks : MonoBehaviour
{
    public Image fadeImage;
    public Image blinkingText;
    public AudioSource audioSource;
    public AudioClip pop;
    public AudioClip init;
    public string nextSceneName;
    private bool isBlinking = true;

    private bool fading = true;
    private float fadeSpeed = 1.0f;
    private float textBlinkSpeed = 5.0f;

    private void Start()
    {
        StartCoroutine(FadeIn());
        audioSource.Play();
        StartCoroutine(BlinkText());
    }

    private void Update()
    {
        if (fading && fadeImage.color.a <= 0)
        {
            fading = false;
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(0);
        }
        else
        if (Input.anyKeyDown)
        {
            StartCoroutine(ChangeScene());
        }
    }

    private IEnumerator FadeIn()
    {
        while (fadeImage.color.a > 0)
        {
            Color color = fadeImage.color;
            color.a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = color;
            yield return null;
        }
    }

    private IEnumerator BlinkText()
    {
        while (isBlinking)
        {
            blinkingText.enabled = !blinkingText.enabled;
            if (blinkingText.enabled == true)
            {
                audioSource.PlayOneShot(pop);
            }
            yield return new WaitForSeconds(1f / textBlinkSpeed);
        }
    }

    private IEnumerator ChangeScene()
    {
        isBlinking = false;
        audioSource.PlayOneShot(init);
        blinkingText.enabled = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextSceneName);
    }
}