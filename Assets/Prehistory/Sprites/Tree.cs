using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Clickable
{
    private AudioSource audioSource;

    public bool beingCut;

    public void StopPlayingSound()
    {
        StopAllCoroutines();
    }

    public void PlaySoundInLoop()
    {
        StopAllCoroutines();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        while (true)
        {
            audioSource.Play();
            yield return new WaitForSeconds(0.85f);
        }
    }

    public override bool OnClick(Action action)
    {
        return false;
    }
}
