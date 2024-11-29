using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using JioGames;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JioButton : Button
{
    [SerializeField] private RectTransform selfRectTransform;
    public bool hoverEffectEnabled = true;

   private float duration = 0.3f;

    private bool isHovering;
    /// <summary>
    /// param="bool" if true then hover enter, if false then hover exit;
    /// </summary>
    public event Action<bool> HoverEvent;

    public void OnValidate()
    {
        selfRectTransform = GetComponent<RectTransform>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        PinchInteraction.OnPinchEvent += PinchClicked;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PinchInteraction.OnPinchEvent -= PinchClicked;
    }

    private void PinchClicked()
    {
        Debug.Log($"Pinch Clicked  isHovering : {isHovering} || interactable : {interactable}");
        if (isHovering && interactable)
        {
            // Debug.Log($"Pinch Cliked Firing Event");

            onClick?.Invoke();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        // Debug.Log($"Button Hovered");
        HoverEvent?.Invoke(true);
        isHovering = true;

        if (selfRectTransform && hoverEffectEnabled)
        {
            selfRectTransform.DOLocalMoveZ(-20, duration);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        HoverEvent?.Invoke(false);
        // Debug.Log($"Button UnHovered");
        isHovering = false;

        if (selfRectTransform && hoverEffectEnabled)
        {
            selfRectTransform.DOLocalMoveZ(0, duration);
        }
    }

    public void SnapHover(bool isHover)
    {
        isHovering = isHover;
        if (isHover)
        {
            if (selfRectTransform && hoverEffectEnabled)
            {
                selfRectTransform.DOLocalMoveZ(-20, 0f);
            }
        }
        else
        {
            if (selfRectTransform && hoverEffectEnabled)
            {
                selfRectTransform.DOLocalMoveZ(0, 0f);
            }
        }
        
    }
}
