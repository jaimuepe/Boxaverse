using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : Clickable
{

    public Sprite plant1;
    public Sprite plant2;
    public Sprite plant3;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private int[] correctSequence = new int[] { 1, 3, 1 };
    private int numberOfPlants = 0;

    int[] plantsInCauldron = new int[3];

    public override bool OnClick(Action action)
    {
        return false;
    }

    public AudioClip audioClipOk;
    public AudioClip audioClipWrong;

    public bool CheckPotion()
    {
        bool isCorrect = true;
        for (int i = 0; i < 3; i++)
        {
            if (plantsInCauldron[i] != correctSequence[i])
            {
                isCorrect = false;
            }
        }

        if (isCorrect)
        {
            audioSource.clip = audioClipOk;
            audioSource.Play();
            RemoveAllPlants();
            return true;
        }

        audioSource.clip = audioClipWrong;
        audioSource.Play();
        RemoveAllPlants();
        return false;
    }

    public bool IsFull()
    {
        return numberOfPlants == 3;
    }

    public AudioClip dropSound;

    public void AddPlant(int plantNumber)
    {
        audioSource.clip = dropSound;
        audioSource.Play();
        GameObject g = transform.GetChild(numberOfPlants).gameObject;

        g.SetActive(true);

        if (plantNumber == 1)
        {
            g.GetComponent<SpriteRenderer>().sprite = plant1;
        }
        else if (plantNumber == 2)
        {
            g.GetComponent<SpriteRenderer>().sprite = plant2;
        }
        else
        {
            g.GetComponent<SpriteRenderer>().sprite = plant3;
        }
        plantsInCauldron[numberOfPlants] = plantNumber;
        numberOfPlants++;
    }

    public void RemoveAllPlants()
    {
        numberOfPlants = 0;
        plantsInCauldron = new int[3];

        foreach (Transform t in transform)
        {
            GameObject g = t.gameObject;
            g.GetComponent<SpriteRenderer>().sprite = null;
            g.SetActive(false);
        }
    }
}
