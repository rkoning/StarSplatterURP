using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class OptionButton : MonoBehaviour
{
    public TextMeshProUGUI actionText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI hotKeyText;

    public Button actionButton;

    public void SetValues(string actionName, string cost, string hotKey, UnityAction action) {
        name = actionName;
        actionText.text = actionName;
        costText.text = cost;
        hotKeyText.text = hotKey;
        actionButton.onClick.AddListener(action);
    }

    private void OnDestroy() {
        actionButton.onClick.RemoveAllListeners();    
    }
}
