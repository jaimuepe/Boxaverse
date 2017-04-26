using System;
using System.Collections;
using UnityEngine;


public enum State
{
    NONE, IDLE, GOING_TOWARDS_WOOD, PICKING_WOOD, RETURNING_WOOD, DONE, YAY
}

public class Guy01 : Character
{
    public float gatheringTimeInSecs;
    public GameObject woodTarget;
    public GameObject bonfireTarget;

    public GameObject lumberStack;

    private State _currentState = State.IDLE;

    public State nextState = State.NONE;

    public State CurrentState
    {
        get
        {
            return _currentState;
        }
        private set
        {
            _currentState = value;
        }
    }

    private void Awake()
    {
        a = GetComponent<Animator>();
        if (dialogBox)
        {
            randomMsgCoroutine = StartCoroutine(DisplayRandomMessage());
        }
    }

    public override bool CanMoveRandomly()
    {
        return CurrentState == State.IDLE;
    }

    public override bool OnClick(Action action)
    {
        if (CurrentState == State.IDLE)
        {
            if (action == Action.PICKUP_WOOD)
            {
                return true;
            }
        }

        PlaySound(angryTrack);
        return false;
    }

    private void Update()
    {

        if (nextState == State.DONE)
        {
            if (!lumberStack.gameObject.activeSelf)
            {
                lumberStack.gameObject.SetActive(true);
            }
            lumberStack.GetComponent<Lumber>().Increase();
        }

        if (nextState == State.PICKING_WOOD)
        {
            Tree tree;
            if (woodTarget == treeTarget1)
            {
                tree = tree1.GetComponent<Tree>();
            }
            else
            {
                tree = tree2.GetComponent<Tree>();
            }

            a.SetTrigger("start_picking_wood");
            tree.GetComponent<Animator>().SetTrigger("shake");
            tree.GetComponent<Tree>().PlaySoundInLoop();
            tree.GetComponent<Tree>().beingCut = true;
            StartCoroutine(CutTree());
        }

        if (nextState == State.GOING_TOWARDS_WOOD)
        {
            RandomMovement rm = GetComponent<RandomMovement>();
            if (rm)
            {
                rm.StopAllCoroutines();
            }
        }

        if (nextState != State.NONE && nextState != CurrentState)
        {
            CurrentState = nextState;
            nextState = State.NONE;
        }

        if (CurrentState == State.GOING_TOWARDS_WOOD)
        {
            MoveTowardsTarget(woodTarget.transform.position);
        }

        if (CurrentState == State.RETURNING_WOOD)
        {
            MoveTowardsTarget(bonfireTarget.transform.position);
        }


        if (CurrentState == State.YAY)
        {
            // a.SetTrigger("go_bananas");
        }

        WalkAnimation();
        previousPosition = transform.position;
    }

    public IEnumerator CutTree()
    {
        yield return new WaitForSeconds(gatheringTimeInSecs);
        Tree tree;
        if (woodTarget == treeTarget1)
        {
            tree = tree1.GetComponent<Tree>();
        }
        else
        {
            tree = tree2.GetComponent<Tree>();
        }

        tree.StopPlayingSound();
        tree.GetComponent<Animator>().SetTrigger("fall");
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 1f));
        nextState = State.RETURNING_WOOD;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CurrentState == State.GOING_TOWARDS_WOOD && collision.gameObject.GetInstanceID() == woodTarget.GetInstanceID())
        {
            nextState = State.PICKING_WOOD;
        }

        if (CurrentState == State.RETURNING_WOOD && collision.gameObject.GetInstanceID() == bonfireTarget.GetInstanceID())
        {
            nextState = State.DONE;
        }
    }

    public GameObject tree1;
    public GameObject tree2;

    public GameObject treeTarget1;
    public GameObject treeTarget2;

    public override void Interact(Action action, Clickable target)
    {
        if (CurrentState == State.IDLE && action == Action.PICKUP_WOOD)
        {
            if (target.GetType() == typeof(Tree))
            {
                Tree t = (Tree)target;
                if (!t.beingCut)
                {
                    if (t.gameObject.GetInstanceID() == tree1.GetInstanceID())
                    {
                        woodTarget = treeTarget1;
                    }
                    else
                    {
                        woodTarget = treeTarget2;
                    }

                    t.beingCut = true;
                    nextState = State.GOING_TOWARDS_WOOD;
                }
            }
        }
    }
}
