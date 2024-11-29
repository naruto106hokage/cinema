using System;
using System.Collections;
using System.Collections.Generic;
using JioGames;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VRKeyboard.Utils
{
    public enum State
    {
        Enabled,
        Disabled,
    }
    [System.Serializable]
    public struct ButtonState
    {
        public State state;
        public Sprite sprite;
    }

    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public abstract class CommonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private UnityEngine.UI.Button button;
        [SerializeField] ButtonState[] buttonStates;
        private State currentState;

        public Action OnClick;

        protected bool isHoverd;

        private void OnValidate()
        {
            button = GetComponent<UnityEngine.UI.Button>();
        }

        public virtual void OnEnable()
        {
            button.onClick.AddListener(OnSelectEnter);
            PinchInteraction.OnPinchEvent += PinchClicked;
        }

        private void PinchClicked()
        {
            if (isHoverd && button.interactable) OnSelectEnter();
        }

        public virtual void OnDisable()
        {
            OnHoverExit();

            button.onClick.RemoveListener(OnSelectEnter);
            PinchInteraction.OnPinchEvent -= PinchClicked;
        }

        protected virtual void OnHoverEnter()
        {
            isHoverd = true;
        }

        protected virtual void OnHoverExit()
        {
            isHoverd = false;
        }

        public virtual void OnSelectEnter()
        {
            Debug.Log("CommonButton : OnSelectEnter : OnClick : " + OnClick);
            OnClick?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHoverEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoverExit();
        }

        public void ToggleInteractivity(bool interactable)
        {
            // Debug.Log("button : " + interactable);
            button.interactable = interactable;
        }

        public void ChangeButtonImage(Sprite image)
        {
            button.image.sprite = image;
        }

        public void ChangeState(State state)
        {
            if (currentState == state)
            {
                return;
            }
            currentState = state;
            foreach (var buttonState in buttonStates)
            {
                if (buttonState.state == currentState)
                {
                    button.image.sprite = buttonState.sprite;
                }
            }
        }
    }
}
