using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Clickable
{
    public override bool OnClick(Action action)
    {
        return false;
    }

    public bool isRestored = false;

    public void EnableFires()
    {
        fire1.gameObject.SetActive(true);
        fire2.gameObject.SetActive(true);
        fire3.gameObject.SetActive(true);
    }

    public void DisableFires()
    {
        fire1.gameObject.SetActive(false);
        fire2.gameObject.SetActive(false);
        fire3.gameObject.SetActive(false);
    }

    public GameObject fire1;
    public GameObject fire2;
    public GameObject fire3;

}
