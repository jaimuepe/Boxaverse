using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonStart : MonoBehaviour
{

    public void LoadFirstLevel()
    {
        StartCoroutine(FirstLevelCoroutine());
    }

    private IEnumerator FirstLevelCoroutine()
    {
        Image p = GameObject.FindGameObjectWithTag("FadeLevelPanel").GetComponent<Image>();

        while (p.color.a < 1)
        {
            p.color = new Color(p.color.r, p.color.g, p.color.b, p.color.a + 0.01f);
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
