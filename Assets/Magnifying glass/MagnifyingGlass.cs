using UnityEngine;

public class MagnifyingGlass : MonoBehaviour
{
    public Texture magnifyingGlassTexture;
    public GameObject magnifyingGlass;
    public Camera magnifyingCamera;

    private AudioSource audioSource;

    Camera mainCamera;
    InputManager inputManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Shader.SetGlobalTexture("_TimeCrackTexture", magnifyingGlassTexture);
    }

    private void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        mainCamera = Camera.main;
    }

    public bool IsActive()
    {
        return magnifyingGlass.gameObject.activeSelf;
    }

    public void Disable()
    {
        magnifyingGlass.gameObject.SetActive(false);
    }

    public void SwitchMode()
    {
        inputManager.ClearAction();
        if (IsActive())
        {
            Disable();
        }
        else
        {
            audioSource.Play();
            inputManager.SetCursor(null);
            magnifyingGlass.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (magnifyingGlass.gameObject.activeSelf)
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            float mainHeight = 2f * mainCamera.orthographicSize;
            float mainWidth = mainHeight * mainCamera.aspect;

            float height = 2f * magnifyingCamera.orthographicSize;
            float width = height * magnifyingCamera.aspect;

            worldPos.x = Mathf.Clamp(worldPos.x, -mainWidth / 2 + width / 2, mainWidth / 2 - width / 2);
            worldPos.y = Mathf.Clamp(worldPos.y, mainCamera.transform.position.y - mainHeight / 2 + height / 2, mainCamera.transform.position.y + mainHeight / 2 - height / 2);

            magnifyingCamera.transform.position = new Vector3(worldPos.x, worldPos.y, magnifyingCamera.transform.position.z);
            magnifyingGlass.transform.position = new Vector3(worldPos.x, worldPos.y, magnifyingGlass.transform.position.z);
        }
    }
}
