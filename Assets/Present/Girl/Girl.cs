using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Girl : Character
{

    enum GirlState
    {
        NONE, IDLE, MOVING_TOWARDS_UNI, DONE
    }

    GirlState CurrentState = GirlState.IDLE;
    GirlState NextState = GirlState.NONE;

    private void Start()
    {
        randomMsgCoroutine = StartCoroutine(DisplayRandomMessage());
    }

    public GameObject universityTarget;

    private void Update()
    {
        if (NextState == GirlState.MOVING_TOWARDS_UNI)
        {
            RandomMovement rm = GetComponent<RandomMovement>();
            if (rm)
            {
                rm.StopAllCoroutines();
            }
        }

        if (NextState == GirlState.DONE)
        {
            FindObjectOfType<GameManager>().GirlIsReady();
            transform.position = new Vector3(1000, 1000, 1000);
            girlInLibrary.SetActive(true);
        }

        if (NextState != GirlState.NONE && NextState != CurrentState)
        {
            CurrentState = NextState;
            NextState = GirlState.NONE;
        }

        if (CurrentState == GirlState.MOVING_TOWARDS_UNI)
        {
            MoveTowardsTarget(universityTarget.transform.position);
        }

        WalkAnimation();
        previousPosition = transform.position;
    }

    public GameObject girlInLibrary;

    public override bool CanMoveRandomly()
    {
        return CurrentState == GirlState.IDLE;
    }

    public override void Interact(Action action, Clickable target)
    {
        if (!target.enabled)
        {
            return;
        }

        if (action == Action.DO_SICK_MATH)
        {
            NextState = GirlState.MOVING_TOWARDS_UNI;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CurrentState == GirlState.MOVING_TOWARDS_UNI && collision.gameObject.GetInstanceID() == universityTarget.GetInstanceID())
        {
            NextState = GirlState.DONE;
        }
    }

    public override bool OnClick(Action action)
    {

        if (action == Action.DO_SICK_MATH)
        {
            return true;
        }

        AudioSource audioS = GetComponent<AudioSource>();
        audioS.clip = noTrack;
        audioS.Play();

        return false;
    }

    public string msg1Phase1;
    public string msg2Phase1;

    public string msg1Phase2;
    public string msg2Phase2;

    public override IEnumerator DisplayRandomMessage()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(15f, 20f));
            dialogBox.SetActive(true);

            string sentence = "";

            if (CurrentState == GirlState.IDLE || CurrentState == GirlState.MOVING_TOWARDS_UNI)
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    sentence = msg1Phase1;
                }
                else
                {
                    sentence = msg2Phase1;
                }
            }
            else if (CurrentState == GirlState.DONE)
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    sentence = msg1Phase2;
                }
                else
                {
                    sentence = msg2Phase2;
                }
            }

            if (sentence != "")
            {
                dialogBox.GetComponentInChildren<Text>().text = sentence;
                yield return new WaitForSeconds(3f);
                dialogBox.SetActive(false);
            }
        }
    }
}
