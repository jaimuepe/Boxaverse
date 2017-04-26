using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadePanelMid : MonoBehaviour
{

    Image img;
    public GameObject message1;
    public GameObject message2;

    public GameObject prevMessage1;
    public GameObject prevMessage2;

    public float delay;
    public float decrement = 0.01f;

    void Start()
    {
        img = GetComponent<Image>();

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {

        if (prevMessage1)
        {
            Image panel = prevMessage1.GetComponent<Image>();
            Text t = prevMessage1.GetComponentInChildren<Text>();

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

        yield return new WaitForSeconds(1.5f);

        if (prevMessage2)
        {
            Image panel = prevMessage2.GetComponent<Image>();
            Text t = prevMessage2.GetComponentInChildren<Text>();

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

        while (img.color.a > 0)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - decrement);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        if (message1)
        {
            Image panel = message1.GetComponent<Image>();
            Text t = message1.GetComponentInChildren<Text>();

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

        if (message2)
        {
            Image panel = message2.GetComponent<Image>();
            Text t = message2.GetComponentInChildren<Text>();

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

        yield return new WaitForSeconds(1f);

        StartCoroutine(FadeOutFrame());
    }

    IEnumerator FadeOutFrame()
    {
        Image p = GameObject.FindGameObjectWithTag("FadeLevelPanel").GetComponent<Image>();

        while (p.color.a < 1)
        {
            p.color = new Color(p.color.r, p.color.g, p.color.b, p.color.a + 0.01f);
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
