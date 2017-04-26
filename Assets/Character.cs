using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : Clickable
{
    [Header("Interactons")]
    public List<Action> actions;
    public List<Clickable> targets;

    [Space(10)]

    [Header("Movement")]
    public float movementSpeed;

    [Space(10)]
    [Header("Sounds")]
    public AudioClip randomTrack1;
    public AudioClip randomTrack2;
    public AudioClip randomTrack3;

    public AudioClip noTrack;
    public AudioClip yesTrack;
    public AudioClip angryTrack;

    [Space(10)]
    [Header("Messages")]

    public Vector2 dialogOffset;
    public GameObject dialogBox;
    public string randomSentence1;
    public string randomSentence2;

    [Space(10)]

    public GameObject headFire;

    private Dictionary<Action, List<Clickable>> interactions;

    protected Animator a;
    protected Vector3 previousPosition;
    private AudioSource audioSource;

    protected Coroutine randomMsgCoroutine;

    private int lastKnownYDirection;

    public virtual IEnumerator DisplayRandomMessage()
    {
        if (dialogBox)
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(15f, 20f));
                dialogBox.SetActive(true);

                string sentence;

                if (GetType() == typeof(Guy01) && Random.Range(0, 1000) == 1)
                {
                    sentence = "GOD IS DEAD";
                }
                else
                if (Random.Range(0, 2) == 1)
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
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        audioSource.clip = clip;
        audioSource.Play();
    }

    public bool CanInteractWith(Action action, Clickable target)
    {
        if (!target.enabled)
        {
            return false;
        }

        if (interactions == null)
        {
            interactions = new Dictionary<Action, List<Clickable>>();
            for (int i = 0; i < actions.Count; i++)
            {
                if (!interactions.ContainsKey(actions[i]))
                {
                    interactions[actions[i]] = new List<Clickable>();
                }
                interactions[actions[i]].Add(targets[i]);

            }
        }

        if (interactions.ContainsKey(action))
        {
            foreach (Clickable c in interactions[action])
            {
                if (c.GetType() == target.GetType())
                {
                    PlaySound(yesTrack);
                    return true;
                }
            }
        }
        PlaySound(noTrack);
        return false;
    }

    public abstract void Interact(Action action, Clickable target);

    public virtual void WalkAnimation()
    {

        if (!a)
        {
            a = GetComponent<Animator>();
        }

        float diffX = (transform.position.x - previousPosition.x);
        float diffY = (transform.position.y - previousPosition.y);

        if (diffY > 0.0001f)
        {
            lastKnownYDirection = 1;
            a.SetTrigger("moving_up");
        }
        else if (diffY < -0.0001f)
        {
            lastKnownYDirection = -1;
            a.SetTrigger("moving_down");
        }
        else if (Mathf.Abs(diffX) > 0.0001f)
        {
            if (lastKnownYDirection == 1)
            {
                a.SetTrigger("moving_up");
            }
            else if (lastKnownYDirection == -1)
            {
                a.SetTrigger("moving_down");
            }
        }
        else
        {
            lastKnownYDirection = 0;
            a.SetTrigger("idle");
        }

        previousPosition = transform.position;
    }

    public abstract bool CanMoveRandomly();

    public void MoveTowardsTarget(Vector2 targetPosition)
    {
        Vector2 currentPosition = this.transform.position;

        Vector2 directionOfTravel = targetPosition - currentPosition;
        //now normalize the direction, since we only want the direction information
        directionOfTravel.Normalize();
        //scale the movement on each axis by the directionOfTravel vector components

        this.transform.Translate(
            (directionOfTravel.x * movementSpeed * Time.deltaTime),
            (directionOfTravel.y * movementSpeed * Time.deltaTime),
            0,
            Space.World);
    }

    SpriteRenderer sr;

    Camera magnifyingCamera;

    private void LateUpdate()
    {
        if (headFire)
        {

            GameObject lens = GameObject.FindGameObjectWithTag("MagnifyingLens");

            if (magnifyingCamera == null)
            {
                magnifyingCamera = FindObjectOfType<MagnifyingCamera>().GetComponent<Camera>();
            }

            Vector2 cameraPosition = magnifyingCamera.transform.position;
            Vector2 thisPosition = transform.position;

            bool isBeingRendered = Vector2.Distance(cameraPosition, thisPosition) < 0.3f;

            if (lens && isBeingRendered)
            {
                if (ext != null)
                {
                    StopCoroutine(ext);
                    ext = null;
                }

                if (start == null)
                {
                    start = StartCoroutine(StartFire());
                }
            }
            else
            {
                if (start != null)
                {
                    StopCoroutine(start);
                    start = null;
                }

                if (ext == null && headFire.activeSelf)
                {
                    ext = StartCoroutine(Extingish());
                }
            }
        }
        else if (GetType() != typeof(Doggo))
        {
            Debug.Log("No ParticleSystem found");
        }

        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }

        if (!dialogBox)
        {
            return;
        }

        Vector3 position = transform.position + new Vector3(sr.bounds.extents.x, dialogOffset.y, 0f);
        Vector3 positionInPixels = Camera.main.WorldToScreenPoint(position);
        dialogBox.transform.position = positionInPixels;
    }

    Coroutine ext;
    Coroutine start;

    IEnumerator StartFire()
    {
        yield return new WaitForSeconds(2f);
        headFire.SetActive(true);
    }

    IEnumerator Extingish()
    {
        yield return new WaitForSeconds(3f);
        headFire.SetActive(false);
    }
}
