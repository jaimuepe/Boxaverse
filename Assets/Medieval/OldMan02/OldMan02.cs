using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMan02 : Character
{

    public AudioClip clipFlowers;

    public GameObject plant1Target;
    public GameObject plant2Target;
    public GameObject plant3Target;

    public GameObject cauldronTarget;

    enum OldManState02
    {
        NONE, IDLE, GOING1, GOING2, GOING3, PICKINGPLANT1, PICKINGPLANT2, PICKINGPLANT3, RETURN1, RETURN2, RETURN3
    }

    OldManState02 _currentState = OldManState02.IDLE;
    OldManState02 CurrentState
    {
        get
        {
            return _currentState;
        }
        set
        {
            _currentState = value;
            if (_currentState == OldManState02.IDLE)
            {
                RandomMovement rm = GetComponent<RandomMovement>();
                if (rm)
                {
                    rm.Restart();
                }
            }
        }
    }

    OldManState02 NextState = OldManState02.NONE;

    private void Start()
    {
        randomMsgCoroutine = StartCoroutine(DisplayRandomMessage());
    }

    private void Update()
    {

        if (NextState == OldManState02.GOING1 || NextState == OldManState02.GOING2 || NextState == OldManState02.GOING3 || NextState == OldManState02.RETURN1
            || NextState == OldManState02.RETURN2 || NextState == OldManState02.RETURN3)
        {
            RandomMovement rm = GetComponent<RandomMovement>();
            if (rm)
            {
                rm.StopAllCoroutines();
            }
        }

        if (NextState == OldManState02.PICKINGPLANT1)
        {
            StartCoroutine(PickPlant(1));
        }

        if (NextState == OldManState02.PICKINGPLANT2)
        {
            StartCoroutine(PickPlant(2));
        }

        if (NextState == OldManState02.PICKINGPLANT3)
        {
            StartCoroutine(PickPlant(3));
        }

        if (NextState != OldManState02.NONE && NextState != CurrentState)
        {
            CurrentState = NextState;
            NextState = OldManState02.NONE;
        }

        if (CurrentState == OldManState02.GOING1)
        {
            MoveTowardsTarget(plant1Target.transform.position);
        }

        if (CurrentState == OldManState02.GOING2)
        {
            MoveTowardsTarget(plant2Target.transform.position);
        }

        if (CurrentState == OldManState02.GOING3)
        {
            MoveTowardsTarget(plant3Target.transform.position);
        }

        if (CurrentState == OldManState02.RETURN1)
        {
            MoveTowardsTarget(cauldronTarget.transform.position);
        }

        if (CurrentState == OldManState02.RETURN2)
        {
            MoveTowardsTarget(cauldronTarget.transform.position);
        }

        if (CurrentState == OldManState02.RETURN3)
        {
            MoveTowardsTarget(cauldronTarget.transform.position);
        }

        WalkAnimation();


        previousPosition = transform.position;
    }

    IEnumerator PickPlant(int plant)
    {
        Coroutine c = StartCoroutine(PlayFlowersSound());
        yield return new WaitForSeconds(2f);
        if (plant == 1)
        {
            NextState = OldManState02.RETURN1;
        }
        if (plant == 2)
        {
            NextState = OldManState02.RETURN2;
        }
        if (plant == 3)
        {
            NextState = OldManState02.RETURN3;
        }
        StopCoroutine(c);
    }

    public IEnumerator PlayFlowersSound()
    {
        while (true)
        {
            AudioSource.PlayClipAtPoint(clipFlowers, Camera.main.transform.position, 1f);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.2f));
        }
    }

    public override bool CanMoveRandomly()
    {
        return CurrentState == OldManState02.IDLE;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CurrentState == OldManState02.GOING1 && collision.gameObject.GetInstanceID() == plant1Target.GetInstanceID())
        {
            NextState = OldManState02.PICKINGPLANT1;
        }

        if (CurrentState == OldManState02.GOING2 && collision.gameObject.GetInstanceID() == plant2Target.GetInstanceID())
        {
            NextState = OldManState02.PICKINGPLANT2;
        }

        if (CurrentState == OldManState02.GOING3 && collision.gameObject.GetInstanceID() == plant3Target.GetInstanceID())
        {
            NextState = OldManState02.PICKINGPLANT3;
        }

        if (CurrentState == OldManState02.RETURN1 || CurrentState == OldManState02.RETURN2 || CurrentState == OldManState02.RETURN3)
        {
            int plant = CurrentState == OldManState02.RETURN1 ? 1 : CurrentState == OldManState02.RETURN2 ? 2 : 3;

            if (collision.gameObject.GetInstanceID() == cauldronTarget.GetInstanceID())
            {
                FindObjectOfType<Cauldron>().AddPlant(plant);
                NextState = OldManState02.IDLE;
            }

        }
    }

    public override void Interact(Action action, Clickable target)
    {
        if (action == Action.PICKUP_PLANT)
        {
            Cauldron c = FindObjectOfType<Cauldron>();
            if (!c.IsFull())
            {
                if (target.GetType() == typeof(Plant1))
                {
                    NextState = OldManState02.GOING1;
                }
                else if (target.GetType() == typeof(Plant2))
                {
                    NextState = OldManState02.GOING2;
                }
                else
                {
                    NextState = OldManState02.GOING3;
                }
            }
        }
    }

    public override bool OnClick(Action action)
    {
        if (CurrentState == OldManState02.IDLE)
        {
            if (action == Action.PICKUP_PLANT)
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
        Gizmos.DrawWireSphere(plant1Target.transform.position, 0.1f);
        Gizmos.DrawWireSphere(plant2Target.transform.position, 0.1f);
        Gizmos.DrawWireSphere(plant3Target.transform.position, 0.1f);

        Gizmos.DrawWireSphere(cauldronTarget.transform.position, 0.1f);
    }
}
