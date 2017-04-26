using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameRocketPanel : Clickable
{

    public GameObject blockingLayer;

    public Image item1;
    public Image item2;
    public Image item3;

    public Sprite spr11;
    public Sprite spr12;
    public Sprite spr13;

    public Sprite spr21;
    public Sprite spr22;
    public Sprite spr23;

    public Sprite spr31;
    public Sprite spr32;
    public Sprite spr33;

    private int[] correctSequence = new int[] { 0, 2, 2 };
    private int[] playerSequence;

    private void Start()
    {
        StartGame();
    }

    public int currentPhase = 0;

    public void StartGame()
    {
        blockingLayer.SetActive(true);
        gameObject.SetActive(true);
        playerSequence = new int[3];
        currentPhase = 0;
        LoadNextPhase();
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }
    }

    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;

    public void Choose(int choice)
    {
        playerSequence[currentPhase] = choice;

        AudioClip clip;

        if (currentPhase == 0)
        {
            clip = clip1;
        }
        else if (currentPhase == 1)
        {
            clip = clip2;
        }
        else
        {
            clip = clip3;
        }

        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);

        currentPhase++;

        if (currentPhase == 3)
        {
            CheckAnswer();
        }
        else
        {
            LoadNextPhase();
        }
    }

    private void CheckAnswer()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }

        StartCoroutine(HideBlockingLayer());
    }

    private bool CorrectAnswer()
    {
        bool rightAnswer = true;

        for (int i = 0; i < 3; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                rightAnswer = false;
                break;
            }
        }

        return rightAnswer;
    }

    private Color blockingLayerColor;

    IEnumerator HideBlockingLayer()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }

        Image panel = blockingLayer.GetComponent<Image>();
        blockingLayerColor = panel.color;

        while (panel.color.a > 0)
        {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - 0.01f);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        if (CorrectAnswer())
        {
            fadeInDelay = 1f;
            StartCoroutine(FadeOut(true));
        }
        else
        {
            rocket.GetComponent<Rocket>().EnableFires();
            yield return new WaitForSeconds(2f);
            fadeInDelay = 3f;
            StartCoroutine(FadeOut(true));
        }
    }

    public Sprite rocketFull;
    public GameObject rocket;

    public AudioClip audioClip;
    public AudioClip clipOk;

    public override void ActionsDuringFadeOut()
    {
        Rocket r = rocket.GetComponent<Rocket>();
        Guy g = FindObjectOfType<Guy>();

        if (CorrectAnswer())
        {
            AudioSource.PlayClipAtPoint(clipOk, Camera.main.transform.position);
            g.FinishedMinigame();
            r.isRestored = true;
            rocket.GetComponent<SpriteRenderer>().sprite = rocketFull;
        }
        else
        {
            blockingLayer.GetComponent<Image>().color = blockingLayerColor;
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
            g.ResetState();

        }

        g.transform.position = new Vector3(-0.557f, -5.76f, g.transform.position.z);

        r.DisableFires();
        blockingLayer.SetActive(false);
    }

    private void LoadNextPhase()
    {
        if (currentPhase == 0)
        {
            item1.sprite = spr11;
            item2.sprite = spr12;
            item3.sprite = spr13;
        }
        else if (currentPhase == 1)
        {
            item1.sprite = spr21;
            item2.sprite = spr22;
            item3.sprite = spr23;
        }
        else
        {
            item1.sprite = spr31;
            item2.sprite = spr32;
            item3.sprite = spr33;
        }
    }

    public override bool OnClick(Action action)
    {
        return false;
    }
}
