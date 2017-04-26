using System;
using System.Collections;
using UnityEngine;

public class Fire : Clickable
{
    private void OnEnable()
    {
        StartCoroutine(FadeOutSound());
        GoToNextLevel();
    }

    private IEnumerator FadeOutSound()
    {
        float elapsedTime = 0.0f;
        float totalTime = 1.0f;

        AudioSource aSource = GetComponent<AudioSource>();
        float initialVolume = aSource.volume;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            aSource.volume = Mathf.Lerp(initialVolume, 0f, elapsedTime / totalTime);
            yield return null;
        }
    }

    public override bool OnClick(Action action)
    {
        return false;
    }
}
