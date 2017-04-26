using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oldman : Character
{

    public GameObject doggoFollower;

    public enum OldManState
    {
        NONE, IDLE, GOT_DOGGO, GOT_SPACE_SUIT, MOVING_TO_LAUNDRY, AT_LAUNDRY, DONE
    }

    OldManState CurrentState = OldManState.IDLE;
    OldManState NextState = OldManState.NONE;

    public string idleRandomMsg1;
    public string idleRandomMsg2;

    public string afterDoggoRandomMsg1;
    public string afterDoggoRandomMsg2;

    public string afterSpaceSuitRandomMsg1;
    public string afterSpaceSuitRandomMsg2;

    public void EnableDoggo()
    {
        doggoFollower.SetActive(true);
        NextState = OldManState.GOT_DOGGO;
    }

    private void Start()
    {
        randomMsgCoroutine = StartCoroutine(DisplayRandomMessage());
    }

    private void Update()
    {

        if (NextState == OldManState.MOVING_TO_LAUNDRY)
        {
            RandomMovement rm = GetComponent<RandomMovement>();
            if (rm)
            {
                rm.StopAllCoroutines();
            }
        }

        if (NextState == OldManState.AT_LAUNDRY)
        {

            StartCoroutine(FadeOut(true));
        }

        if (NextState != OldManState.NONE && NextState != CurrentState)
        {
            CurrentState = NextState;
            NextState = OldManState.NONE;
        }

        if (CurrentState == OldManState.MOVING_TO_LAUNDRY)
        {
            MoveTowardsTarget(laundryTarget.transform.position);
        }

        WalkAnimation();
        previousPosition = transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CurrentState == OldManState.MOVING_TO_LAUNDRY && collision.gameObject.GetInstanceID() == laundryTarget.GetInstanceID())
        {
            NextState = OldManState.AT_LAUNDRY;
        }
    }


    public GameObject astronaut;
    public GameObject laundryTarget;

    public override void ActionsDuringFadeOut()
    {
        astronaut.gameObject.SetActive(true);
        FindObjectOfType<GameManager>().AstronautIsReady();
        transform.position = new Vector3(1000, 1000, 1000);
        Destroy(gameObject, 5);
    }

    public override bool CanMoveRandomly()
    {
        return true;
    }

    public override void Interact(Action action, Clickable target)
    {
        if (!target.enabled)
        {
            return;
        }
        if (action == Action.GET_SUIT && CurrentState == OldManState.GOT_DOGGO)
        {
            NextState = OldManState.MOVING_TO_LAUNDRY;
        }
    }

    public override bool OnClick(Action action)
    {

        if (action == Action.GET_SUIT)
        {
            return true;
        }

        AudioSource audioS = GetComponent<AudioSource>();
        audioS.clip = noTrack;
        audioS.Play();

        return false;
    }

    public override IEnumerator DisplayRandomMessage()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(15f, 20f));
            dialogBox.SetActive(true);

            string sentence;

            if (CurrentState == OldManState.IDLE)
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    sentence = idleRandomMsg1;
                }
                else
                {
                    sentence = idleRandomMsg2;
                }
            }
            else if (CurrentState == OldManState.GOT_DOGGO || CurrentState == OldManState.MOVING_TO_LAUNDRY)
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    sentence = afterDoggoRandomMsg1;
                }
                else
                {
                    sentence = afterDoggoRandomMsg2;
                }
            }

            else
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    sentence = randomSentence1;
                }
                else
                {
                    sentence = randomSentence1;
                }
            }

            dialogBox.GetComponentInChildren<Text>().text = sentence;
            yield return new WaitForSeconds(3f);
            dialogBox.SetActive(false);
        }
    }
}
