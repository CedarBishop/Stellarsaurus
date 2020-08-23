using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerHighlight : MonoBehaviour
{
    public SpriteRenderer arrowSpriteRenderer;
    public SpriteRenderer highlightBeamSpriteRenderer;
    public Light2D light;
    public float timeTillDestroy;


    public void Initialise (int playerNumber)
    {
        Color playerColour = GameManager.instance.playerColours[playerNumber - 1];
        arrowSpriteRenderer.color = playerColour;
        playerColour.a = 0.5f;
        highlightBeamSpriteRenderer.color = playerColour;
        Destroy(gameObject, timeTillDestroy);
    }
}
