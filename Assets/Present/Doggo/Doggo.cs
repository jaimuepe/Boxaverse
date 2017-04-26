using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doggo : Character
{

    private void Start()
    {
        randomMsgCoroutine = StartCoroutine(DisplayRandomMessage());
    }

    private void Update()
    {
        WalkAnimation();
        previousPosition = transform.position;
    }

    public override void WalkAnimation()
    {
        if (!a)
        {
            a = GetComponent<Animator>();
        }

        float diffX = (transform.position.x - previousPosition.x);
        float diffY = (transform.position.y - previousPosition.y);

        if (diffX > 0.0001f)
        {
            a.SetTrigger("moving_right");
        }
        else if (diffX < -0.0001f)
        {
            a.SetTrigger("moving_left");
        }
        else if (Mathf.Abs(diffY) > 0.0001f)
        {
            a.SetTrigger("moving_right");
        }
        else
        {
            a.SetTrigger("idle");
        }
        previousPosition = transform.position;
    }

    public override bool CanMoveRandomly()
    {
        return true;
    }

    public override void Interact(Action action, Clickable target)
    {
        throw new NotImplementedException();
    }

    public override bool OnClick(Action action)
    {
        return false;
    }

    GameManager gm;
    private void OnMouseDown()
    {

        if (gm == null)
        {
            gm = FindObjectOfType<GameManager>();
        }

        if (gm.GetSelectedAction() != Action.NONE)
        {
            return;
        }
        GetComponent<AudioSource>().Play();
    }
}
