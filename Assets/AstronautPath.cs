using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AstronautPath : MonoBehaviour
{

    public GameObject path1;
    public GameObject path2;
    public GameObject path3;
    public GameObject path4;
    public GameObject path5;

    GameObject currentPath;
    GameObject nextPath;

    Astronaut a;

    private void Awake()
    {
        nextPath = path1;
        a = GetComponent<Astronaut>();
    }

    private void Update()
    {
        if (!animating)
        {
            return;
        }

        currentPath = nextPath;
        if (currentPath != null)
        {
            a.MoveTowardsTarget(currentPath.transform.position);
        }
    }

    private bool animating = true;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!animating)
        {
            return;
        }

        if (collision.gameObject == path1 && currentPath == path1)
        {
            nextPath = path2;
            AudioSource.PlayClipAtPoint(cheers1, Camera.main.transform.position);
        }

        if (collision.gameObject == path2 && currentPath == path2)
        {
            nextPath = path3;
            AudioSource.PlayClipAtPoint(cheers2, Camera.main.transform.position);
        }

        if (collision.gameObject == path3 && currentPath == path3)
        {
            nextPath = path4;
        }

        if (collision.gameObject == path4 && currentPath == path4)
        {
            AudioSource.PlayClipAtPoint(cheers3, Camera.main.transform.position);
            nextPath = path5;
        }


        if (collision.gameObject == path5 && currentPath == path5)
        {
            animating = false;
            rocket.GetComponent<Animator>().SetTrigger("start");
            rocket.GetComponent<AudioSource>().Play();

            foreach (Transform t in rocket.transform)
            {
                t.gameObject.SetActive(true);
            }
            transform.position = new Vector3(1000, 1000, 1000);
            StartCoroutine(WaitForNextLevel());
        }
    }

    IEnumerator WaitForNextLevel()
    {
        yield return new WaitForSeconds(3);
        a.GoToNextLevel();
    }

    public AudioClip cheers1;
    public AudioClip cheers2;
    public AudioClip cheers3;

    public GameObject rocket;
}
