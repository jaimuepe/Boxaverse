using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Kid02 : Clickable
{
    public float dialogueYOffset;
    public Camera magnifyingCamera;
    public GameObject headFire;

    public AudioClip clipCough;
    public AudioClip clipGrunt;

    public GameObject dialogBox;
    public string randomSentence1;
    public string randomSentence2;

    GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        StartCoroutine(DisplayRandomMessage());
    }

    private void OnMouseDown()
    {
        if (gm.GetSelectedAction() != Action.NONE)
        {
            return;
        }

        AudioSource audioS = GetComponent<AudioSource>();
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            audioS.clip = clipCough;
        }
        else
        {
            audioS.clip = clipGrunt;
        }
        audioS.Play();
    }

    public override bool OnClick(Action action)
    {

        return false;
    }

    protected Coroutine randomMsgCoroutine;

    private void Update()
    {
        if (magnifyingCamera == null)
        {
            magnifyingCamera = FindObjectOfType<MagnifyingCamera>().GetComponent<Camera>();
        }


        Vector3 viewPos = magnifyingCamera.WorldToViewportPoint(transform.position);

        bool isBeingRendered = 0f <= viewPos.x && viewPos.x <= 1f && 0f <= viewPos.y && viewPos.y <= 1f;

        if (isBeingRendered)
        {
            if (ext != null)
            {
                StopCoroutine(ext);
                ext = null;
            }
            headFire.SetActive(true);
        }
        else if (headFire.activeSelf)
        {
            if (ext == null)
            {
                ext = StartCoroutine(Extingish());
            }
        }
    }

    Coroutine ext;

    IEnumerator Extingish()
    {
        yield return new WaitForSeconds(3f);
        headFire.SetActive(false);
    }


    public IEnumerator DisplayRandomMessage()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
            dialogBox.SetActive(true);

            string sentence;

            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                sentence = randomSentence1;
            }
            else
            {
                sentence = randomSentence2;
            }
            dialogBox.GetComponentInChildren<Text>().text = sentence;
            yield return new WaitForSeconds(3f);
            dialogBox.SetActive(false);
        }
    }

    SpriteRenderer sr;

    private void LateUpdate()
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }

        if (!dialogBox)
        {
            return;
        }

        Vector3 position = transform.position + new Vector3(sr.bounds.extents.x, dialogueYOffset, 0f);
        Vector3 positionInPixels = Camera.main.WorldToScreenPoint(position);
        dialogBox.transform.position = positionInPixels;
    }
}
