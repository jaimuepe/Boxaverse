using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public bool outline;
    public Color color = Color.white;

    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateOutline();
    }

    void OnDisable()
    {
        outline = false;
        UpdateOutline();
    }

    public void EnableOutline()
    {
        if (color != Color.yellow)
        {
            outline = true;
            color = Color.white;
        }
    }

    public void DisableOutline()
    {
        if (color != Color.yellow)
        {
            outline = false;
            color = Color.white;
        }
    }

    void Update()
    {
        UpdateOutline();
    }

    void UpdateOutline()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}