using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject blockingPanel;
    public string currentEra;

    [Header("Prehistoric")]
    public GameObject lumber;
    public GameObject fire;

    [Space]

    [Header("Medieval")]
    [Space]

    private bool astronautReady = false;
    private bool rocketIsReady = false;
    private bool girlIsReady = false;

    public void GirlIsReady()
    {
        girlIsReady = true;
        CheckModernEraLevelEnd();
    }

    public void AstronautIsReady()
    {
        astronautReady = true;
        CheckModernEraLevelEnd();
    }

    public void RocketIsReady()
    {
        rocketIsReady = true;
        CheckModernEraLevelEnd();
    }

    Action selectedAction = Action.NONE;

    private void CheckModernEraLevelEnd()
    {
        if (!astronautReady)
        {
            return;
        }
        if (!girlIsReady)
        {
            return;
        }
        if (!rocketIsReady)
        {
            return;
        }

        StartCoroutine(ModernEraEndAnimation());
    }

    private IEnumerator ModernEraEndAnimation()
    {
        DontDestroyOnLoad(gameObject);
        yield return new WaitForSeconds(3f);
        Astronaut n = FindObjectOfType<Astronaut>();
        StartCoroutine(n.FadeOut(false));
    }

    public bool IsSceneBlocked()
    {
        return blockingPanel && blockingPanel.activeSelf;
    }

    public void ChangeAction(Action newAction)
    {
        selectedAction = newAction;
    }

    public Action GetSelectedAction()
    {
        return selectedAction;
    }
}
