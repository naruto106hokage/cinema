using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using VRKeyboard.Utils;

public class InputFieldJio : TMP_InputField
{
    // Override this method to prevent the native keyboard from opening
    public override void OnPointerClick(PointerEventData eventData)
    {
        // Do not call the base method to prevent the native keyboard
        // base.OnPointerClick(eventData);

        // Show your custom keyboard here
        Debug.Log($"[InputFieldJio] OnPointerClick");
        Debug.Log($"[InputFieldJio] OnPointerClick : KeboardType :  {keyboardType}");
        
        if (KeyboardManager.Instance) KeyboardManager.Instance.EnableKeyboard(this, keyboardType);
        ActivateInputField();
        shouldHideMobileInput = true;
    }

    // Override this if you want to control focus manually
    public override void OnSelect(BaseEventData eventData)
    {
        // Don't call the base method
        // base.OnSelect(eventData);
        Debug.Log($"[InputFieldJio] OnSelect");
        ActivateInputField();
        shouldHideMobileInput = true;
    }

    // Override this to prevent deselecting the input field    
    public override void OnDeselect(BaseEventData eventData)
    {
        //if (KeyboardManager.Instance) KeyboardManager.Instance.CloseKeyboard();
        Debug.Log($"[InputFieldJio] OnDeselect");
    }

    public void ActivateCustomKeyboard(int charcaterLimit = 50,bool hideInput = false)
    {
        Debug.Log($"[InputFieldJio] ActivateCustomKeyboard : KeboardType :  {keyboardType}");
        if (KeyboardManager.Instance) KeyboardManager.Instance.EnableKeyboard(this, keyboardType,charcaterLimit,hideInput);
        ActivateInputField();
        shouldHideMobileInput = true;
    }
    
  
    // public override void DeactivateInputField()
    // {
    //     // Prevent the native deactivation
    // }
}
