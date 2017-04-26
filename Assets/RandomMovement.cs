using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public LayerMask stopLayerMask;

    private bool ready;

    float nextMovementIn;

    Vector2 direction = Vector2.zero;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();

        nextMovementIn = Random.Range(1f, 3f);
        StartCoroutine(WaitForNextMovement());
    }

    public void Restart()
    {
        StopAllCoroutines();
        nextMovementIn = Random.Range(1f, 3f);
        StartCoroutine(WaitForNextMovement());
    }

    void Update()
    {
        if (ready)
        {
            if (character.CanMoveRandomly())
            {
                direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                StartCoroutine(Walk());
            }

            ready = false;
        }
    }

    IEnumerator Walk()
    {
        float startTime = Time.time;
        float duration = Random.Range(1f, 3f);

        while (Time.time < startTime + duration)
        {
            Vector3 newPosition = transform.position + new Vector3(direction.x, direction.y, 0) * character.movementSpeed * Time.deltaTime;
            if (Physics2D.Raycast(transform.position, direction, 1.5f * character.movementSpeed * Time.deltaTime, stopLayerMask))
            {
                break;
            }

            transform.position = newPosition;
            yield return null;
        }

        StartCoroutine(WaitForNextMovement());
    }

    IEnumerator WaitForNextMovement()
    {
        yield return new WaitForSeconds(nextMovementIn);
        nextMovementIn = Random.Range(1f, 3f);
        ready = true;
    }
}
