using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Kid : Character
{

    public GameObject dogTarget;

    public string idleRandomMsg1;
    public string idleRandomMsg2;

    public GameObject doggoGUIButton;

    public GameObject laundry;
    public GameObject laundryInside;

    public Image panel;

    public enum KidState
    {
        NONE, LOOKING_FOR_DOGGO, GOING_TOWARDS_DOGGO, FOUND_DOGGO, DONE
    }

    KidState CurrentState = KidState.LOOKING_FOR_DOGGO;
    KidState NextState = KidState.NONE;

    private void Start()
    {
        randomMsgCoroutine = StartCoroutine(DisplayRandomMessage());
    }

    private void Update()
    {

        if (NextState == KidState.GOING_TOWARDS_DOGGO)
        {
            RandomMovement rm = GetComponent<RandomMovement>();
            if (rm)
            {
                rm.StopAllCoroutines();
            }
        }

        if (NextState == KidState.FOUND_DOGGO)
        {
            AudioSource.PlayClipAtPoint(barkClip, Camera.main.transform.position);
            StartCoroutine(DogWaiting());
        }

        if (NextState != KidState.NONE && NextState != CurrentState)
        {
            CurrentState = NextState;
            NextState = KidState.NONE;
        }

        if (CurrentState == KidState.GOING_TOWARDS_DOGGO)
        {
            MoveTowardsTarget(dogTarget.transform.position);
        }

        WalkAnimation();
        previousPosition = transform.position;
    }

    public AudioClip barkClip;

    public IEnumerator DogWaiting()
    {
        Doggo d = FindObjectOfType<Doggo>();
        d.transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, 0f);

        yield return new WaitForSeconds(1f);
        NextState = KidState.DONE;

        StartCoroutine(FadeOut(true));
    }

    public GameObject spaceSuit;

    public override void ActionsDuringFadeOut()
    {
        Doggo d = FindObjectOfType<Doggo>();
        d.gameObject.SetActive(false);

        Oldman o = FindObjectOfType<Oldman>();
        o.EnableDoggo();

        laundry.layer = LayerMask.NameToLayer("OnlyMainCameraClickable");
        laundryInside.gameObject.SetActive(true);
        spaceSuit.SetActive(true);

        doggoGUIButton.SetActive(true);
    }

    public override bool CanMoveRandomly()
    {
        return true;
    }

    public override void Interact(Action action, Clickable target)
    {
        if (action == Action.FIND_DOGO)
        {
            NextState = KidState.GOING_TOWARDS_DOGGO;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CurrentState == KidState.GOING_TOWARDS_DOGGO && collision.gameObject.GetInstanceID() == dogTarget.GetInstanceID())
        {
            NextState = KidState.FOUND_DOGGO;
        }
    }

    public override bool OnClick(Action action)
    {

        if (action == Action.FIND_DOGO && CurrentState == KidState.LOOKING_FOR_DOGGO)
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

            if (UnityEngine.Random.Range(0, 100) == 1)
            {
                sentence = "FUZZY PICKES";
            }
            else if (CurrentState == KidState.LOOKING_FOR_DOGGO)
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
            else
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    sentence = randomSentence1;
                }
                else
                {
                    sentence = randomSentence2;
                }
            }

            dialogBox.GetComponentInChildren<Text>().text = sentence;
            yield return new WaitForSeconds(3f);
            dialogBox.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(dogTarget.transform.position, 0.1f);
    }
}
