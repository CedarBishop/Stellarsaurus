using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MaterialInstance : MonoBehaviour
{
    private static string MAIN_TEXTURE = "_MainTex";
    private static string PALETTE_TEXTURE = "_PaletteTex";

    public Texture2D paletteTexture;

    private SpriteRenderer rend;
    private MaterialPropertyBlock block;
    private Texture2D mainTex;  // Store the main texture here
    private int colorID;

    [ColorUsage(true, true)] public Color myColor;

    private void OnValidate()
    {
        rend = GetComponent<SpriteRenderer>();
        block = new MaterialPropertyBlock();
        mainTex = GetSpriteTexture();  // Assign main texture in Start()
        colorID = Shader.PropertyToID("_Color");

        Debug.Log(
            rend.material.HasProperty(PALETTE_TEXTURE) ?
            ToString() + " - exists" :
            ToString() + " - DOES NOT EXIST");

        block.SetTexture(MAIN_TEXTURE, mainTex);  // Apply the sprite's main texture
        block.SetTexture(PALETTE_TEXTURE, paletteTexture);
        block.SetColor(colorID, myColor);
        rend.SetPropertyBlock(block);
        rend.sprite = rend.sprite;
    }

    private Texture2D GetSpriteTexture()
    {
        // Get main texture trough MaterialPropertyBlock from sprite renderer
        MaterialPropertyBlock getBlock = new MaterialPropertyBlock();
        rend.GetPropertyBlock(getBlock);
        return (Texture2D)getBlock.GetTexture(MAIN_TEXTURE);
    }
}
