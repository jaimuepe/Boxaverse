using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuListener : MonoBehaviour
{
    public bool titleHasFadedIn;
    public bool teamHasFadedIn;

    public Image imgGameName;
    public Image imgTeamName;

    private void Start()
    {
        coroutineFadeGameName = StartCoroutine(FadeGameName());
        coroutineFadeTeamName = StartCoroutine(FadeTeamName());
    }

    Coroutine coroutineFadeGameName;
    Coroutine coroutineFadeTeamName;

    IEnumerator FadeGameName()
    {

        yield return new WaitForSeconds(2f);

        float elapsedTime = 0.0f;
        float totalTime = 2.0f;

        float i = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float newA = Mathf.Lerp(i, 1f, elapsedTime / totalTime);
            imgGameName.color = new Color(imgGameName.color.r, imgGameName.color.g, imgGameName.color.b, newA);
            yield return null;
        }

        titleHasFadedIn = true;
    }

    IEnumerator FadeTeamName()
    {

        yield return new WaitForSeconds(4f);

        float elapsedTime = 0.0f;
        float totalTime = 2.0f;

        float i = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float newA = Mathf.Lerp(i, 1f, elapsedTime / totalTime);
            imgTeamName.color = new Color(imgTeamName.color.r, imgTeamName.color.g, imgTeamName.color.b, newA);
            yield return null;
        }

        teamHasFadedIn = true;
    }

    public void ClickEvt()
    {
        if (!titleHasFadedIn && !teamHasFadedIn)
        {
            StopCoroutine(coroutineFadeGameName);
            StopCoroutine(coroutineFadeTeamName);

            imgGameName.color = Color.white;
            imgTeamName.color = Color.white;

            titleHasFadedIn = true;
            teamHasFadedIn = true;
        }
        else
        {
            StartCoroutine(FadeOutFrame());
        }
    }

    IEnumerator FadeOutFrame()
    {
        Image p = GameObject.FindGameObjectWithTag("FadeLevelPanel").GetComponent<Image>();

        while (p.color.a < 1)
        {
            p.color = new Color(p.color.r, p.color.g, p.color.b, p.color.a + 0.01f);
            yield return null;
        }

        StartCoroutine(FadeOutMusic());
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator FadeOutMusic()
    {
        float elapsedTime = 0.0f;
        float totalTime = 1.0f;

        AudioSource aSource = GetComponent<AudioSource>();
        float initialVolume = aSource.volume;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            aSource.volume = Mathf.Lerp(initialVolume, 0f, elapsedTime / totalTime);
            yield return null;
        }
    }
}
