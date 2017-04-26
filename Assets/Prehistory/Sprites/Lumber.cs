using System.Collections;
using UnityEngine;

public class Lumber : Clickable
{

    public float fireDelay;

    public Sprite spr1;
    public Sprite spr2;

    public Camera magnifyingCamera;
    public ParticleSystem fireParticleSystem;
    public GameObject fire;

    bool finished = false;

    public int ammount = 0;

    public void Increase()
    {
        ammount += 1;
        if (ammount == 1)
        {
            GetComponent<SpriteRenderer>().sprite = spr1;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = spr2;
        }
    }

    public GameObject magnifyingLens;

    void Update()
    {

        if (finished || ammount < 2)
        {
            return;
        }

        Vector3 viewPos = magnifyingCamera.WorldToViewportPoint(transform.position);

        GameObject lens = GameObject.FindGameObjectWithTag("MagnifyingLens");

        if (magnifyingCamera == null)
        {
            magnifyingCamera = FindObjectOfType<MagnifyingCamera>().GetComponent<Camera>();
        }

        bool isBeingRendered = 0f <= viewPos.x && viewPos.x <= 1f && 0f <= viewPos.y && viewPos.y <= 1f;

        if (lens && isBeingRendered)
        {
            if (start == null)
            {
                start = StartCoroutine(WaitForFire());
            }
        }
        else
        {
            if (start != null)
            {
                fireParticleSystem.gameObject.SetActive(false);
                StopCoroutine(start);
                start = null;
            }
        }
    }

    Coroutine start;

    public override bool OnClick(Action action)
    {
        return false;
    }

    IEnumerator WaitForFire()
    {
        fireParticleSystem.gameObject.SetActive(true);
        yield return new WaitForSeconds(fireDelay);
        fire.SetActive(true);
        finished = true;

        foreach (Guy01 g in FindObjectsOfType<Guy01>())
        {
            g.nextState = State.YAY;
        }
    }
}
