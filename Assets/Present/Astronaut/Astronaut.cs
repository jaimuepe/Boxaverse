using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Astronaut : Character
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

    public override bool CanMoveRandomly()
    {
        return true;
    }

    public override void Interact(Action action, Clickable target)
    {
        return;
    }

    public override bool OnClick(Action action)
    {
        return false;
    }

    public override void ActionsDuringFadeOut()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
