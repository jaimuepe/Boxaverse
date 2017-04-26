using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class Clickable : MonoBehaviour
{
    public abstract bool OnClick(Action action);

    public GameObject msg1;
    public GameObject msg2;

    SpriteOutline so;
    GameManager gm;

    private void OnMouseEnter()
    {
        if (!gm)
        {
            gm = FindObjectOfType<GameManager>();
        }

        if (gm.IsSceneBlocked())
        {
            return;
        }

        if (!so)
        {
            so = GetComponent<SpriteOutline>();
        }
        MagnifyingGlass magnifyingGlass = FindObjectOfType<MagnifyingGlass>();
        if (so && (!magnifyingGlass || magnifyingGlass && !magnifyingGlass.IsActive()))
        {
            so.EnableOutline();
        }
    }

    private void OnMouseExit()
    {
        if (!so)
        {
            so = GetComponent<SpriteOutline>();
        }
        if (so && !Input.GetKey(KeyCode.LeftAlt))
        {
            so.DisableOutline();
        }
    }

    public void ShowNightMessages()
    {

    }

    public GameObject nightOverlay;

    public void NightMode()
    {
        StartCoroutine(LerpNightMode());
    }

    private IEnumerator LerpNightMode()
    {
        float elapsedTime = 0.0f;
        float totalTime = 2.0f;

        Image i = nightOverlay.GetComponent<Image>();
        Color32 startColor = i.color;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            i.color = Color.Lerp(startColor, new Color32(104, 93, 186, 152), elapsedTime / totalTime);
            yield return null;
        }
    }

    public virtual void ActionsDuringFadeOut()
    {
    }

    public float fadeInDelay = 0.1f;

    public IEnumerator FadeOut(bool fadeIn)
    {
        Image panel = GameObject.FindGameObjectWithTag("FadePanel").GetComponent<Image>();

        while (panel.color.a < 1)
        {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a + 0.01f);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        ActionsDuringFadeOut();
        yield return new WaitForSeconds(fadeInDelay);

        if (fadeIn)
        {
            StartCoroutine(FadeIn());
        }
    }

    public IEnumerator FadeIn()
    {
        Image panel = GameObject.FindGameObjectWithTag("FadePanel").GetComponent<Image>();

        while (panel.color.a > 0)
        {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - 0.01f);
            yield return null;
        }
    }

    public void GoToNextLevel()
    {
        StartCoroutine(End());
    }

    IEnumerator End()
    {

        NightMode();
        yield return new WaitForSeconds(5f);
        StartCoroutine(ShowMsg1());
    }

    IEnumerator ShowMsg1()
    {

        if (msg1)
        {
            Image panel = msg1.GetComponent<Image>();
            Text t = msg1.GetComponentInChildren<Text>();

            if (t.text != "")
            {

                while (panel.color.a < 1)
                {
                    panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a + 0.01f);
                    t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a + 0.01f);
                    yield return null;
                }

                yield return new WaitForSeconds(2f);

                while (panel.color.a > 0)
                {
                    panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - 0.01f);
                    t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - 0.01f);
                    yield return null;
                }
            }
        }

        if (msg2)
        {

            Image panel = msg2.GetComponent<Image>();
            Text t = msg2.GetComponentInChildren<Text>();

            if (t.text != "")
            {

                while (panel.color.a < 1)
                {
                    panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a + 0.01f);
                    t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a + 0.01f);
                    yield return null;
                }

                yield return new WaitForSeconds(2f);

                while (panel.color.a > 0)
                {
                    panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - 0.01f);
                    t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - 0.01f);
                    yield return null;
                }
            }
        }

        Image p = GameObject.FindGameObjectWithTag("FadeLevelPanel").GetComponent<Image>();

        while (p.color.a < 1)
        {
            p.color = new Color(p.color.r, p.color.g, p.color.b, p.color.a + 0.01f);
            yield return null;
        }

        StartCoroutine(FadeOutMusic());
        yield return new WaitForSeconds(1f);

        AudioSource.PlayClipAtPoint(goodNightClip, Camera.main.transform.position);
        yield return new WaitForSeconds(5.1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator FadeOutMusic()
    {
        float elapsedTime = 0.0f;
        float totalTime = 1.0f;

        AudioSource aSource = FindObjectOfType<GameManager>().GetComponent<AudioSource>();
        float initialVolume = aSource.volume;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            aSource.volume = Mathf.Lerp(initialVolume, 0f, elapsedTime / totalTime);
            yield return null;
        }
    }

    public AudioClip goodNightClip;
}
