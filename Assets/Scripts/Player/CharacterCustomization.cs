using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterCustomization : MonoBehaviour
{
    public GameObject uiDisplay;
    public GameObject inCustomiserParent;
    public GameObject outOfCustomiserParent;
    public Text headText;
    public Text bodyText;
    public Image backgroundImage;
    public Image headImage;
    public Image bodyImage;
    public Image previousHeadImage;
    public Image nextHeadImage;
    public Image previousBodyImage;
    public Image nextBodyImage;
    public Image buttonToInteract;

    public Sprite[] headSprites;
    public Sprite[] bodySprites;

    private bool isInUse;

    private List<Controller> triggeredPlayers = new List<Controller>();

    private void Start()
    {
        uiDisplay.SetActive(false);
        inCustomiserParent.SetActive(false);
        outOfCustomiserParent.SetActive(true);
        isInUse = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.GetComponentInParent<Controller>())
        {
            Controller player = collision.GetComponentInParent<Controller>();
            triggeredPlayers.Add(player);
            player.isTriggeringCharacterCustomizer = transform;
            CustomizerController controller = player.GetComponent<CustomizerController>();
            controller.customization = this;

            if (isInUse)
            {
                return;
            }

            Setup(player);            
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {        
        if (collision.GetComponentInParent<Controller>())
        {
            Controller player = collision.GetComponentInParent<Controller>();
            triggeredPlayers.Remove(player);
            player.isTriggeringCharacterCustomizer = false;
            CustomizerController controller = player.GetComponent<CustomizerController>();
            controller.customization = null;

            if (isInUse)
            {
                return;
            }

            if (triggeredPlayers != null)
            {
                if (triggeredPlayers.Count >= 1)
                {
                    Setup(triggeredPlayers[0]);
                    return;
                }
            }

            uiDisplay.SetActive(false);
        }
    }

    void Setup(Controller player)
    {
        SetBackgroundImageColor(player.playerNumber);
        uiDisplay.SetActive(true);
        inCustomiserParent.SetActive(false);
        outOfCustomiserParent.SetActive(true);
        buttonToInteract.sprite = GameManager.instance.controlSchemeSpriteHandler.GetControlSprite(ControlActions.Grab, player.GetComponent<PlayerInput>().currentControlScheme);
    }

    public void Enter (int headNum, int bodyNum, string controlScheme, int playerNumber)
    {
        print("Enter");
        isInUse = true;
        inCustomiserParent.SetActive(true);
        outOfCustomiserParent.SetActive(false);
        SetBackgroundImageColor(playerNumber);
        previousHeadImage.sprite = GameManager.instance.controlSchemeSpriteHandler.GetControlSprite(ControlActions.Down, controlScheme);
        nextHeadImage.sprite = GameManager.instance.controlSchemeSpriteHandler.GetControlSprite(ControlActions.Up, controlScheme);
        previousBodyImage.sprite = GameManager.instance.controlSchemeSpriteHandler.GetControlSprite(ControlActions.Left, controlScheme);
        nextBodyImage.sprite = GameManager.instance.controlSchemeSpriteHandler.GetControlSprite(ControlActions.Right, controlScheme);

        SetHead(headNum);
        SetBody(bodyNum);
    }

    public void Exit ()
    {
        isInUse = false;
        inCustomiserParent.SetActive(false);
        outOfCustomiserParent.SetActive(true);
    }

    public void SetHead (int num)
    {
        headText.text = "Head " + (num + 1).ToString();
        headImage.sprite = headSprites[num];
    }

    public void SetBody (int num)
    {
        bodyText.text = "Body " + (num + 1).ToString();
        bodyImage.sprite = bodySprites[num];
    }

    void SetBackgroundImageColor (int playerNumber)
    {
        Color color = GameManager.instance.playerColours[playerNumber - 1];
        color.a = 0.5f;
        backgroundImage.color = color;
    }
}
