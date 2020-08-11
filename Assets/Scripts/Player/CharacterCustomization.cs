using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public Sprite[] headSprites;
    public Sprite[] bodySprites;

    private void Start()
    {
        uiDisplay.SetActive(false);
        inCustomiserParent.SetActive(false);
        outOfCustomiserParent.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Player>())
        {
            Player player = collision.GetComponentInParent<Player>();
            player.isTriggeringCharacterCustomizer = transform;
            CustomizerController controller = player.GetComponent<CustomizerController>();
            controller.customization = this;
            uiDisplay.SetActive(true);
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
