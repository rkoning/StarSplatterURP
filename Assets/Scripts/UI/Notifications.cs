using UnityEngine;
using TMPro;

public class Notifications : MonoBehaviour
{
    public TextMeshProUGUI notifyText;
    public int maxNotifications = 5;

    private static TextMeshProUGUI[] notifications;

    public float fadeTime = 3f;
    private static int current;

    public Vector3 notificationsStart;
    public Vector3 notificationSize;

    void Start()
    {
        notifications = new TextMeshProUGUI[maxNotifications];
        notifications[0] = notifyText;
        notifications[0].gameObject.SetActive(false);
        for (int i = 1; i < maxNotifications; i++) {
            var g = GameObject.Instantiate(notifyText);
            notifications[i] = g.GetComponent<TextMeshProUGUI>();
            g.gameObject.SetActive(false);
        }
    }

    public static void Notify(string text, Color color) {
        notifications[current].gameObject.SetActive(true);
        notifications[current].text = text;
        notifications[current].color = color;

        current++;
        current = Mathf.Clamp(current, 0, notifications.Length - 1);
    }

    private static void SlideDown() {

    }
}
