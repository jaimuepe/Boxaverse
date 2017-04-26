using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gal02 : Character
{
    public GameObject popupOk;
    public GameObject popupWrong;

    public GameObject cauldronTarget;
    public GameObject bucketAtCauldron;

    enum Gal02State
    {
        NONE, IDLE, APROACHING_CAULDRON, CHECKING_POTION, DONE
    }

    Gal02State _currentState = Gal02State.IDLE;

    Gal02State CurrentState
    {
        get
        {
            return _currentState;
        }
        set
        {
            _currentState = value;
            if (_currentState == Gal02State.IDLE)
            {
                RandomMovement rm = GetComponent<RandomMovement>();
                if (rm)
                {
                    rm.Restart();
                }
            }
        }
    }

    Gal02State NextState = Gal02State.NONE;

    private void Start()
    {
        randomMsgCoroutine = StartCoroutine(DisplayRandomMessage());
    }

    private void Update()
    {

        if (NextState == Gal02State.APROACHING_CAULDRON)
        {
            RandomMovement rm = GetComponent<RandomMovement>();
            if (rm)
            {
                rm.StopAllCoroutines();
            }
        }

        if (NextState != Gal02State.NONE && NextState != CurrentState)
        {
            CurrentState = NextState;
            NextState = Gal02State.NONE;
        }

        if (CurrentState == Gal02State.APROACHING_CAULDRON)
        {
            MoveTowardsTarget(cauldronTarget.transform.position);
        }

        if (CurrentState == Gal02State.CHECKING_POTION)
        {
            AudioSource audioS = GetComponent<AudioSource>();
            Cauldron c = FindObjectOfType<Cauldron>();
            if (!bucketAtCauldron.activeSelf)
            {
                // NO WATER
                NextState = Gal02State.IDLE;
                audioS.clip = angryTrack;
            }
            else if (!c.CheckPotion())
            {
                popupWrong.SetActive(true);
                popupWrong.GetComponentInChildren<Animator>().SetTrigger("play");
                NextState = Gal02State.IDLE;
                audioS.clip = angryTrack;
            }
            else
            {
                popupOk.SetActive(true);
                popupOk.GetComponentInChildren<Animator>().SetTrigger("play");
                potionIsReady = true;
                NextState = Gal02State.DONE;
                audioS.clip = yesTrack;
            }
        }

        WalkAnimation();
        previousPosition = transform.position;
    }

    bool potionIsReady = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CurrentState == Gal02State.APROACHING_CAULDRON && collision.gameObject.GetInstanceID() == cauldronTarget.GetInstanceID())
        {
            NextState = Gal02State.CHECKING_POTION;
        }
    }

    public override bool CanMoveRandomly()
    {
        return CurrentState == Gal02State.IDLE;
    }

    public override void Interact(Action action, Clickable target)
    {
        if (action == Action.MAKE_POTION)
        {
            AudioSource audioS = GetComponent<AudioSource>();
            if (!bucketAtCauldron.activeSelf)
            {
                // NO WATER
                NextState = Gal02State.IDLE;
                audioS.clip = angryTrack;
                audioS.Play();
                return;
            }
            Cauldron c = (Cauldron)target;
            if (!c.IsFull())
            {
                // NOT FULL
                audioS.clip = angryTrack;
                audioS.Play();
                NextState = Gal02State.IDLE;
                return;
            }
            audioS.clip = yesTrack;
            audioS.Play();
            NextState = Gal02State.APROACHING_CAULDRON;
        }
        else if (action == Action.GIVE_POTION && potionIsReady)
        {
            happyKid.SetActive(true);
            dyingKid.SetActive(false);
            GoToNextLevel();
        }
    }

    public GameObject dyingKid;
    public GameObject happyKid;

    public override bool OnClick(Action action)
    {
        if (action == Action.GIVE_POTION)
        {
            return true;
        }
        if (CurrentState == Gal02State.IDLE)
        {
            if (action == Action.MAKE_POTION || action == Action.GIVE_POTION)
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
        Gizmos.DrawWireSphere(cauldronTarget.transform.position, 0.1f);
    }
}
