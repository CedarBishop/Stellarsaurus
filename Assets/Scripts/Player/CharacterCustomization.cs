using System.Collections;
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

    private void Start()
    {
        uiDisplay.SetActive(false);
        inCustomiserParent.SetActive(false);
        outOfCustomiserParent.SetActive(true);
        isInUse = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInUse)
        {
            return;
        }
        if (collision.GetComponentInParent<Player>())
        {
            Player player = collision.GetComponentInParent<Player>();
            player.isTriggeringCharacterCustomizer = transform;
            CustomizerController controller = player.GetComponent<CustomizerController>();
            controller.customization = this;
            uiDisplay.SetActive(true);
            inCustomiserParent.SetActive(false);
            outOfCustomiserParent.SetActive(true);
            buttonToInteract.sprite = GameManager.instance.controlSchemeSpriteHandler.GetControlSprite(ControlActions.Grab, player.GetComponent<PlayerInput>().currentControlScheme);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>())
        {
            Player player = collision.GetComponentInParent<Player>();
            player.isTriggeringCharacterCustomizer = false;
            CustomizerController controller = player.GetComponent<CustomizerController>();
            controller.customization = null;
            uiDisplay.SetActive(false);
        }
    }

    public void Enter (int headNum, int bodyNum, string controlScheme)
    {
        print("Enter");
        isInUse = true;
        inCustomiserParent.SetActive(true);
        outOfCustomiserParent.SetActive(false);
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
}
