using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy02 : Character
{

    public AudioClip water;

    public GameObject bucketAtWell;
    public GameObject bucketHead;
    public GameObject bucketAtCauldron;

    public GameObject wellTarget;
    public GameObject cauldronTarget;

    enum Guy02State
    {
        NONE, IDLE, WALKING_TO_WELL, FILLING_BUCKET, COMING_BACK, DONE
    }

    Guy02State _currentState = Guy02State.IDLE;

    Guy02State CurrentState
    {
        get
        {
            return _currentState;
        }
        set
        {
            _currentState = value;
        }
    }
    Guy02State NextState = Guy02State.NONE;

    private void Start()
    {
        randomMsgCoroutine = StartCoroutine(DisplayRandomMessage());
    }

    private void Update()
    {

        if (NextState == Guy02State.WALKING_TO_WELL || NextState == Guy02State.COMING_BACK)
        {
            RandomMovement rm = GetComponent<RandomMovement>();
            if (rm)
            {
                rm.StopAllCoroutines();
            }
        }

        if (NextState == Guy02State.FILLING_BUCKET)
        {
            StartCoroutine(FillBucket());
        }

        if (NextState == Guy02State.DONE)
        {
            bucketHead.SetActive(false);
            bucketAtCauldron.SetActive(true);
        }

        if (NextState != Guy02State.NONE && NextState != CurrentState)
        {
            CurrentState = NextState;
            NextState = Guy02State.NONE;
        }

        if (CurrentState == Guy02State.WALKING_TO_WELL)
        {
            MoveTowardsTarget(wellTarget.transform.position);
        }

        if (CurrentState == Guy02State.COMING_BACK)
        {
            MoveTowardsTarget(cauldronTarget.transform.position);
        }

        WalkAnimation();
        previousPosition = transform.position;
    }

    IEnumerator FillBucket()
    {
        AudioSource.PlayClipAtPoint(water, Camera.main.transform.position);
        yield return new WaitForSeconds(1.5f);
        bucketAtWell.SetActive(false);
        bucketHead.SetActive(true);
        NextState = Guy02State.COMING_BACK;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CurrentState == Guy02State.WALKING_TO_WELL && collision.gameObject.GetInstanceID() == wellTarget.GetInstanceID())
        {
            NextState = Guy02State.FILLING_BUCKET;
        }
        if (CurrentState == Guy02State.COMING_BACK && collision.gameObject.GetInstanceID() == cauldronTarget.GetInstanceID())
        {
            NextState = Guy02State.DONE;
        }
    }

    public override bool CanMoveRandomly()
    {
        return CurrentState == Guy02State.IDLE;
    }

    public override void Interact(Action action, Clickable target)
    {
        if (action == Action.PICKUP_WATER)
        {
            CurrentState = Guy02State.WALKING_TO_WELL;
        }
    }

    public override bool OnClick(Action action)
    {
        if (CurrentState == Guy02State.IDLE)
        {
            if (action == Action.PICKUP_WATER)
            {
                return true;
            }
        }
        AudioSource audioS = GetComponent<AudioSource>();
        audioS.clip = angryTrack;
        audioS.Play();
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(wellTarget.transform.position, 0.1f);
        Gizmos.DrawWireSphere(cauldronTarget.transform.position, 0.1f);
    }
}
