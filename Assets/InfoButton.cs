using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoButton : MonoBehaviour
{
    private bool opened;

    public void Close()
    {
        opened = false;
        gameObject.SetActive(false);
    }

    public void Open()
    {
        opened = true;
        gameObject.SetActive(true);
    }

    InfoPanel infoPanel;

    public void Swap()
    {
        if (infoPanel == null)
        {
            infoPanel = FindObjectOfType<InfoPanel>();
        }

        opened = !opened;

        InfoButton[] buttons = infoPanel.infoButtons;

        foreach (InfoButton infoButton in buttons)
        {
            if (infoButton != this)
            {
                infoButton.Close();
            }
        }

        if (opened)
        {
            Open();
        } else
        {
            Close();
        }
    }
}
