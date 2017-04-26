using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanelEnd : MonoBehaviour
{

    Image img;

    public Image img1;
    public Image img2;
    public Image img3;
    public Image img4;

    public float delay;
    public float decrement = 0.01f;

    void Start()
    {
        img = GetComponent<Image>();

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {

        yield return new WaitForSeconds(delay);

        while (img.color.a > 0)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - decrement);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        while (img.color.a < 0.59)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + 5 * decrement);
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        // NAME 1

        while (img1.color.a < 1)
        {
            img1.color = new Color(img1.color.r, img1.color.g, img1.color.b, img1.color.a + 5 * decrement);
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        while (img1.color.a > 0)
        {
            img1.color = new Color(img1.color.r, img1.color.g, img1.color.b, img1.color.a - 5 * decrement);
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        // NAME 2

        while (img2.color.a < 1)
        {
            img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, img2.color.a + 5 * decrement);
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        while (img2.color.a > 0)
        {
            img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, img2.color.a - 5 * decrement);
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        // NAME 3

        while (img3.color.a < 1)
        {
            img3.color = new Color(img3.color.r, img3.color.g, img3.color.b, img3.color.a + 5 * decrement);
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        while (img3.color.a > 0)
        {
            img3.color = new Color(img3.color.r, img3.color.g, img3.color.b, img3.color.a - 5 * decrement);
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        while (img4.color.a < 1)
        {
            img4.color = new Color(img4.color.r, img4.color.g, img4.color.b, img4.color.a + 5 * decrement);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        img4.GetComponent<Animator>().SetTrigger("start");
    }
}
