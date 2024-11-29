using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JioCinema;
using JioGames;

namespace VRKeyboard.Utils
{
    public class KeyboardManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Public Variables
 
        [Header("User defined")]
        [Tooltip("If the character is uppercase at the initialization")]
        public bool isUppercase = false;
 
        public int maxInputLength;
 
        [Header("UI Elements")]
        public TextMeshProUGUI inputText;
 
        public string currentText;
 
        [Header("Essentials")]
        public Transform keys;

        public GameObject numberRow;
        public GameObject specialCharRow;
        
        
        public static event Action<string> KeyPressEvent;
        public static event Action BackPressEvent;
        public static event Action<bool> OnKeyboardOpenEvent; 
 
 
        #endregion Public Variables
 
        public bool IsKeyboardOpen;
 
        #region Private Variables
        private TouchScreenKeyboardType _keyboardType;
        private TMP_InputField _inputField;
        [SerializeField] private GameObject _normalKeyboard;
        [SerializeField] private TextMeshProUGUI _keyboardInputField;
        [SerializeField] private GameObject _phonePadKeyboard;
        [SerializeField] private TextMeshProUGUI _phoneInputField;
        private bool isGazeOnKeyboard = false;
        [SerializeField] private Image phonePadImage;
 
        public string Input
        {
            get
            {
                currentText = inputText.text;
                return inputText.text;
            }
            set
            {
                currentText = value;
                inputText.text = value;
                if (_inputField) _inputField.text = value;
            }
        }
 
        private Key[] keyList;
        private bool capslockFlag;
 
        #endregion Private Variables
 
        public static KeyboardManager Instance;
 
        #region Monobehaviour Callbacks
 
        private void OnEnable()
        {
            Debug.Log("## Keyboard Enable");
            keyList = keys.GetComponentsInChildren<Key>();
 
            isUppercase = false;
            capslockFlag = isUppercase;
            CapsLock();
            ChangeShift();
 
           
            PinchInteraction.OnPinchEvent += OnPinchClicked;
        }
 
        public void KeyboardType(TouchScreenKeyboardType touchScreenKeyboardType)
        {
            Debug.Log($"[Keyboard] KeyboardType Method : {touchScreenKeyboardType}");
            _keyboardType = touchScreenKeyboardType;
            inputText = (_keyboardType != TouchScreenKeyboardType.PhonePad) ? _keyboardInputField : _phoneInputField;
            _normalKeyboard.SetActive(_keyboardType != TouchScreenKeyboardType.PhonePad);
            _phonePadKeyboard.SetActive(_keyboardType == TouchScreenKeyboardType.PhonePad);
            IsKeyboardOpen = true;
            OnKeyboardOpenEvent?.Invoke(true);
            PinchInteraction.OnPinchEvent += OnPinchClicked;
        }
 
        private void OnDisable()
        {
           
            PinchInteraction.OnPinchEvent -= OnPinchClicked;
        }
 
        public void OnPinchClicked()
        {
            
            Debug.Log($"[Keyboard] Pinched GazeOnKeyboard : {isGazeOnKeyboard}");
 
#if !APPLE_VISION
            if (!isGazeOnKeyboard)
            {
                CloseKeyboard();
            }
#endif
        }
 
        public void CloseKeyboard()
        {
            Debug.Log($"[Keyboard] CloseKeyboard : {isGazeOnKeyboard}");
            _normalKeyboard.SetActive(false);
            _phonePadKeyboard.SetActive(false);
            IsKeyboardOpen = false;
            OnKeyboardOpenEvent?.Invoke(false);
            PinchInteraction.OnPinchEvent -= OnPinchClicked;
            Clear();
        }
 
 
        private void Awake()
        {
            // Check if there is already an instance of KeyboardManager
            if (Instance == null)
            {
                // If not, set this as the instance and don't destroy it
                Instance = this;
                DontDestroyOnLoad(gameObject); // Keeps the GameObject between scenes
            }
            else
            {
                // If an instance already exists, destroy the new one to ensure only one instance
                Destroy(gameObject);
            }
 
            keyList = keys.GetComponentsInChildren<Key>();
        }
 
        private void Start()
        {
            foreach (var key in keyList)
            {
                key.OnKeyClicked += GenerateInput;
            }
            capslockFlag = isUppercase;
            CapsLock();
        }
 
        #endregion Monobehaviour Callbacks
 
        #region Public Methods
 
        public void KeyPressed(string character)
        {
            if(!capslockFlag && character != ".com"){character = character.ToUpper();}
            
            Debug.Log($"Input Text Lenght : {inputText.text.Length} || max : {maxInputLength}");
            if (inputText.text.Length >= maxInputLength && maxInputLength!=0) { return; }
            // Input += character;
            KeyPressEvent?.Invoke(character);
            if (inputText.text.Length > maxInputLength&& maxInputLength!=0) { return; }
            int caretPos = _inputField.caretPosition;
               Input = Input.Insert(caretPos, character);
                //Input += character;
            if (_inputField != null && _inputField.isFocused)
            {
                _inputField.caretPosition++;// = _inputField.text.Length;
            }
        }
 
        public void Backspace()
        {
            // Get the current caret position
            int caretPos = _inputField.caretPosition;

            // Ensure the caret position is greater than 0 to avoid errors
            if (caretPos > 0)
            {
                // Remove the character before the caret position
                Input = Input.Remove(caretPos - 1, 1);

                // Update the input field text
                _inputField.text = Input;

                // Move the caret to the previous position
                _inputField.caretPosition = caretPos - 1;
            }
            else
            {
                // If the caret is at the beginning, do nothing
                return;
            }
        }
 
        public void EnableKeyboard(string inputString, TouchScreenKeyboardType touchScreenKeyboardType = TouchScreenKeyboardType.Default,int maxCharacterLength = 50,bool hideInput = false)
        {
            KeyboardType(touchScreenKeyboardType);
            maxInputLength = maxCharacterLength;
            Input = inputString;
            if (hideInput)
            {
                _phoneInputField.DOColor(Color.clear, 0);
                phonePadImage.enabled = false;
            }
            else
            {
                phonePadImage.enabled = true;
                _phoneInputField.DOColor(Color.white, 0);
            }
        }
 
        public void EnableKeyboard(TMP_InputField inputField, TouchScreenKeyboardType touchScreenKeyboardType = TouchScreenKeyboardType.Default,int maxCharacterLength = 50,bool hideInput = false)
        {
            Debug.Log($"[Keyboard] Enable Keyboard : {touchScreenKeyboardType}");
            maxInputLength = inputField.characterLimit;
            KeyboardType(touchScreenKeyboardType);
            this._inputField = inputField;
            Input = inputField.text;
            if (hideInput)
            {
                _phoneInputField.DOColor(Color.clear, 0);
                phonePadImage.enabled = false;
            }
            else
            {
                phonePadImage.enabled = true;
                _phoneInputField.DOColor(Color.white, 0);
            }
        }
 
        public void UpdateText(string character)
        {
            if (inputText.text.Length > maxInputLength && maxInputLength!=0) { return; }
            int caretPos = _inputField.caretPosition;
            Input = Input.Insert(caretPos, character);
            // inputText.text += character;
        }
 
 
        public void Clear()
        {
            _inputField = null;
            Input = "";
        }
 
        public void CapsLock()
        {
            foreach (var key in keyList)
            {
                if (key is Alphabet)
                {
                    key.CapsLock(capslockFlag);
                }
            }

                numberRow.SetActive(!capslockFlag);
                
                
                
                specialCharRow.SetActive(capslockFlag);
            
            capslockFlag = !capslockFlag;
        }
 
        public void Shift()
        {
            foreach (var key in keyList)
            {
                if (key is Shift)
                {
                    key.ShiftKey();
                }
            }
        }
 
        public void ChangeShift()
        {
            foreach (var key in keyList)
            {
                if (key is Shift)
                {
                    key.ShiftKeyT();
                }
            }
        }
 
        public void GenerateInput(string s)
        {
            if (Input.Length > maxInputLength && maxInputLength!=0) { return; }
            Debug.Log("GenerateInput : " + s);
            Input += s;
        }
 
        #endregion Public Methods
 
        #region Interface Implementations
 
        public void OnPointerEnter(PointerEventData eventData)
        {
            isGazeOnKeyboard = true;
        }
 
        public void OnPointerExit(PointerEventData eventData)
        {
            isGazeOnKeyboard = false;
        }
 
        #endregion
    }
}
 
public enum KeyboardType
{
    NORMAL
}