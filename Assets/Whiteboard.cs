using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : Clickable
{

    public GameObject university;

    public override bool OnClick(Action action)
    {
        return false;
    }

    Camera magnifyingCamera;

    bool ready;

    public GameObject exclamationMark;
    public GameObject magnifyingLens;

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
                university.GetComponent<SpriteOutline>().enabled = true;
                university.GetComponent<University>().enabled = true;
                ready = true;
            }
        }
    }
}
