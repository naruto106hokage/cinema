using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VRKeyboard.Utils;
using TMPro;

public class KeyboardButton : CommonButton
{
    public enum ESpecialButtonType { None, Backspace, CapsLock, Enter }

    [SerializeField] private ESpecialButtonType specialButtonType;
    public string character;
    private RectTransform selfRectTransform;

    private float duration = 0.1f;

    private string key;


    Image backGroundImage;
    [SerializeField] Color originalColor = new Color(0.57f, 0.53f, 0.51f, 1);
    Color selectedColor = Color.black;

    void Awake()
    {
        key = transform.Find("Text").GetComponent<TextMeshProUGUI>().text;
    }

    void Start()
    {
        selfRectTransform = GetComponent<RectTransform>();
        backGroundImage = GetComponent<Image>();
        originalColor = backGroundImage.color;
        //xselectedColor = new Color(0.79f, 0.79f, 0.79f, 255);
    }


    protected override void OnHoverEnter()
    {
       // Debug.Log("KeyboardButton : OnHoverEnter Called");
        selfRectTransform?.DOLocalMoveZ(-10, duration);

        base.OnHoverEnter();
    }

    public override void OnSelectEnter()
    {
        base.OnSelectEnter();
        
        backGroundImage.DOColor(selectedColor, duration).OnComplete(() =>
        {
            if (specialButtonType == ESpecialButtonType.None)
            {
                FindObjectOfType<KeyboardManager>().KeyPressed(key);
            }
            else if (specialButtonType == ESpecialButtonType.Backspace)
            {
                FindObjectOfType<KeyboardManager>().Backspace();
            }
            else if (specialButtonType == ESpecialButtonType.Enter)
            {
                FindObjectOfType<KeyboardManager>().CloseKeyboard();
            }
            else if (specialButtonType == ESpecialButtonType.CapsLock)
            {
                FindObjectOfType<KeyboardManager>().CapsLock();
            }

            backGroundImage.color = originalColor;
        });
        //Debug.Log("KeyboardButton : OnSelectEnter Called");
    }

    protected override void OnHoverExit()
    {
        selfRectTransform?.DOLocalMoveZ(0, duration);

        base.OnHoverExit();
    }

}
