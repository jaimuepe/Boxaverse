using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSuit : Clickable
{

    public GameObject magnifyingLens;

    public GameObject laundry;
    public GameObject laundryInside;

    public override bool OnClick(Action action)
    {
        return false;
    }

    Camera magnifyingCamera;

    bool ready;

    public GameObject exclamationMark;

    public float visionRange;

    private void LateUpdate()
    {
        if (!magnifyingLens || !magnifyingLens.activeSelf)
        {
            return;
        }

        if (!ready)
        {
            if (magnifyingCamera == null)
            {
                magnifyingCamera = FindObjectOfType<MagnifyingCamera>().GetComponent<Camera>();
            }

            Vector2 cameraPosition = magnifyingCamera.transform.position;
            Vector2 thisPosition = transform.position;

            bool isBeingRendered = Vector2.Distance(cameraPosition, thisPosition) < 0.1f;

            if (isBeingRendered)
            {
                exclamationMark.GetComponent<AudioSource>().Play();
                exclamationMark.GetComponent<Animator>().SetTrigger("play");
                laundry.GetComponent<SpriteOutline>().enabled = true;
                laundry.GetComponent<Laundry>().enabled = true;
                ready = true;
            }
        }
    }
}
