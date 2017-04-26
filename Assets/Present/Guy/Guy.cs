using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : Character
{

    public GameObject rocket1Target;
    public GameObject rocket2Target;
    public GameObject rocket3Target;

    enum GuyState
    {
        NONE, IDLE, GOING_TOWARDS_ROCKET1, GOING_TOWARDS_ROCKET2, GOING_TOWARDS_ROCKET3,
        PLAYING_MINIGAME, MINIGAME_COMPLETED, DONE
    }

    GuyState CurrentState = GuyState.IDLE;
    GuyState NextState = GuyState.NONE;

    private void Start()
    {
        randomMsgCoroutine = StartCoroutine(DisplayRandomMessage());
    }

    public MinigameRocketPanel minigamePanel;

    public void ResetState()
    {
        NextState = GuyState.IDLE;
        RandomMovement rm = GetComponent<RandomMovement>();
        if (rm)
        {
            rm.Restart();
        }
    }

    private void Update()
    {
        if (CurrentState == GuyState.GOING_TOWARDS_ROCKET1 ||
            CurrentState == GuyState.GOING_TOWARDS_ROCKET2 ||
            CurrentState == GuyState.GOING_TOWARDS_ROCKET3)
        {
            RandomMovement rm = GetComponent<RandomMovement>();
            if (rm)
            {
                rm.StopAllCoroutines();
            }
        }

        if (NextState == GuyState.PLAYING_MINIGAME)
        {
            minigamePanel.StartGame();
        }

        if (NextState != GuyState.NONE && NextState != CurrentState)
        {
            CurrentState = NextState;
            NextState = GuyState.NONE;
        }

        if (CurrentState == GuyState.GOING_TOWARDS_ROCKET1)
        {
            MoveTowardsTarget(rocket1Target.transform.position);
        }

        if (CurrentState == GuyState.GOING_TOWARDS_ROCKET2)
        {
            MoveTowardsTarget(rocket2Target.transform.position);
        }

        if (CurrentState == GuyState.GOING_TOWARDS_ROCKET3)
        {
            MoveTowardsTarget(rocket3Target.transform.position);
        }

        WalkAnimation();
        previousPosition = transform.position;
    }

    public override bool CanMoveRandomly()
    {
        return CurrentState == GuyState.IDLE;
    }

    public void FinishedMinigame()
    {
        FindObjectOfType<GameManager>().RocketIsReady();
        NextState = GuyState.MINIGAME_COMPLETED;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CurrentState == GuyState.GOING_TOWARDS_ROCKET1 && collision.gameObject.GetInstanceID() == rocket1Target.GetInstanceID())
        {
            NextState = GuyState.GOING_TOWARDS_ROCKET2;
        }

        if (CurrentState == GuyState.GOING_TOWARDS_ROCKET2 && collision.gameObject.GetInstanceID() == rocket2Target.GetInstanceID())
        {
            NextState = GuyState.GOING_TOWARDS_ROCKET3;
        }

        if (CurrentState == GuyState.GOING_TOWARDS_ROCKET3 && collision.gameObject.GetInstanceID() == rocket3Target.GetInstanceID())
        {
            NextState = GuyState.PLAYING_MINIGAME;
        }
    }

    public override void Interact(Action action, Clickable target)
    {
        if (CurrentState == GuyState.IDLE && action == Action.REPAIR_ROCKET)
        {
            NextState = GuyState.GOING_TOWARDS_ROCKET1;
        }
    }

    public override bool OnClick(Action action)
    {
        if (action == Action.REPAIR_ROCKET)
        {
            return true;
        }

        AudioSource audioS = GetComponent<AudioSource>();
        audioS.clip = noTrack;
        audioS.Play();

        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(rocket1Target.transform.position, 0.1f);
        Gizmos.DrawWireSphere(rocket2Target.transform.position, 0.1f);
        Gizmos.DrawWireSphere(rocket3Target.transform.position, 0.1f);
    }
}
