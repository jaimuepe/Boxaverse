using UnityEngine;

public class ItemGUI : MonoBehaviour
{
    public Action action;
    public Texture2D cursorTexture;

    private GameManager gm;
    private InputManager im;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gm = FindObjectOfType<GameManager>();
        im = FindObjectOfType<InputManager>();
    }

    public void ChangeCurrentAction()
    {
        audioSource.Play();
        MagnifyingGlass glass = FindObjectOfType<MagnifyingGlass>();
        if (glass)
        {
            glass.Disable();
        }
        im.ClearAction();
        im.SetCursor(cursorTexture);
        gm.ChangeAction(action);
    }
}
