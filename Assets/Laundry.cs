using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laundry : Clickable
{
    private void Start()
    {
        enabled = false;
    }

    public override bool OnClick(Action action)
    {
        return false;
    }
}
