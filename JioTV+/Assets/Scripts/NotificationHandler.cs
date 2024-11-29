using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JioGames
{
    public class NotificationHandler : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Sprite positiveIcon;
        [SerializeField] private Sprite negativeIcon;

        [SerializeField] private TextMeshProUGUI notificationText;
        [SerializeField] private float notificationDuration = 3.0f;
        [SerializeField] private GameObject notificationPanel;
        [SerializeField] private RectTransform notificationPanelRT;

        public static NotificationHandler Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); // Ensures only one instance is kept
                return;
            }

            Instance = this;

            DontDestroyOnLoad(gameObject); // Optional: Keeps this instance alive between scenes
        }

        public void ShowNotification(string message, bool isPositive = true)
        {
            notificationText.text = message;

            if (isPositive)
                iconImage.sprite = positiveIcon;
            else
                iconImage.sprite = negativeIcon;


            StartCoroutine(ToggleNotification(true));
        }

        private IEnumerator ToggleNotification(bool flag)
        {
            notificationPanel.SetActive(flag);
            LayoutRebuilder.ForceRebuildLayoutImmediate(notificationPanelRT);
            yield return new WaitForSeconds(notificationDuration);
            notificationPanel.SetActive(!flag);
        }
    }
}