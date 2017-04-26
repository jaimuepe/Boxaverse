using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public LayerMask clickableMask;

    public Camera magnifyingCamera;

    public Texture2D defaultCursor;

    public Texture2D clickCursor;

    private Texture2D cursor;

    GameManager gm;
    Camera mainCamera;

    ActionInfo currentActionInfo = new ActionInfo()
    {
        source = null,
        target = null
    };

    public void RestoreDefaultCursor()
    {
        cursor = defaultCursor;
    }

    public void SetCursor(Texture2D t)
    {
        cursor = t;
    }

    private void Awake()
    {
        cursor = defaultCursor;
    }

    void Start()
    {
        Cursor.visible = false;
        mainCamera = Camera.main;
        gm = FindObjectOfType<GameManager>();
    }

    private void OnGUI()
    {
        Cursor.visible = false;
        if (cursor)
        {
            GUI.DrawTexture(new Rect(Input.mousePosition.x - 40, Screen.height - Input.mousePosition.y - 40, 80, 80), cursor);
        }
    }

    public GameObject blockingLayer;

    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            MagnifyingGlass glass = FindObjectOfType<MagnifyingGlass>();
            if (glass)
            {
                glass.Disable();
            }
            ClearAction();
        }

        if (blockingLayer != null && blockingLayer.activeSelf)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            MagnifyingGlass glass = FindObjectOfType<MagnifyingGlass>();
            if (!glass || !glass.IsActive())
            {
                foreach (SpriteOutline so in FindObjectsOfType<SpriteOutline>())
                {
                    so.EnableOutline();
                }
            }

        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            foreach (SpriteOutline so in FindObjectsOfType<SpriteOutline>())
            {
                so.DisableOutline();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit2D hit = Physics2D.Raycast(new Vector2(
                mainCamera.ScreenToWorldPoint(Input.mousePosition).x,
                mainCamera.ScreenToWorldPoint(Input.mousePosition).y),
                Vector2.zero, 100f, clickableMask, -Mathf.Infinity, Mathf.Infinity);

            if (hit)
            {
                Clickable clickable = hit.collider.GetComponent<Clickable>();

                if (clickable && clickable.enabled)
                {
                    Character c = hit.collider.GetComponent<Character>();
                    if (c && c.GetType() != typeof(Doggo))
                    {
                        ClearCurrentOutline();
                        if (c.OnClick(gm.GetSelectedAction()))
                        {
                            currentActionInfo.source = c;
                            currentActionInfo.target = null;
                            SetCurrentOutline();
                        }
                    }
                    else
                    {
                        if (currentActionInfo.source && !currentActionInfo.target)
                        {
                            currentActionInfo.target = clickable;
                        }
                    }
                }
            }

            if (currentActionInfo.source != null && currentActionInfo.target != null)
            {
                if (currentActionInfo.source.CanInteractWith(gm.GetSelectedAction(), currentActionInfo.target))
                {
                    currentActionInfo.source.Interact(gm.GetSelectedAction(), currentActionInfo.target);
                }
                else
                {
                    currentActionInfo.source.GetComponent<AudioSource>().clip = currentActionInfo.source.angryTrack;
                    currentActionInfo.source.GetComponent<AudioSource>().Play();
                }

                ClearAction();
            }
        }
    }

    private void ClearCurrentOutline()
    {
        if (currentActionInfo.source != null)
        {
            currentActionInfo.source.GetComponent<SpriteOutline>().color = Color.white;
            currentActionInfo.source.GetComponent<SpriteOutline>().outline = false;
        }
    }

    private void SetCurrentOutline()
    {
        if (currentActionInfo.source != null)
        {
            currentActionInfo.source.GetComponent<SpriteOutline>().outline = true;
            currentActionInfo.source.GetComponent<SpriteOutline>().color = Color.yellow;
        }
    }

    public void ClearAction()
    {
        gm.ChangeAction(Action.NONE);
        RestoreDefaultCursor();
        ClearCurrentOutline();

        currentActionInfo.source = null;
        currentActionInfo.target = null;
    }
}

public class ActionInfo
{
    public Character source;
    public Clickable target;
}