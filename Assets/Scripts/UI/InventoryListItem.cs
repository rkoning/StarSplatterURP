using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryListItem : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI costText;

    public Button actionButton;
    public TextMeshProUGUI actionButtonText;

    public Image background;
    public Image border;

    public Color dimmedBGColor;
    public Color dimmedButtonColor;

    private Color defaultBGColor; 
    private Color defaultButtonColor;

    private Func<Item, bool> dimmedIf;
    private bool dimmed;

    private Item item;

    /// <summary>
    /// Initializes the list item with item values, actionName and callback events.
    /// </summary>
    /// <param name="item">The item to populate this list item with</param>
    /// <param name="actionName">Name to place on the button 'Buy', 'Sell', 'Equip'</param>
    /// <param name="clickEvent">Function that will be called when the button is clicked</param>
    /// <param name="dimmedIf">Function that will be called on update and will change the appearance of the item</param>
    public void Setup(Item item, Color color, string actionName, Action<Item, GameObject> clickEvent, Func<Item, bool> dimmedIf) {
        this.item = item;
        this.nameText.text = item.name;
        this.typeText.text = item.itemType.ToString();
        this.costText.text = item.cost.ToString();
        this.actionButtonText.text = actionName;
        this.actionButton.onClick.AddListener(delegate { clickEvent(item, gameObject); });

        this.dimmedIf = dimmedIf;

        border.color = color;
        defaultBGColor = background.color;
        defaultButtonColor = actionButton.image.color;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// 
    /// Watches the state of the dimmedIf() function and changes the color of the button accordingly
    /// </summary>
    private void Update()
    {
        if (dimmedIf(item)) {
            if (!dimmed) {
                // if the item should be dimmed and has not been, set it's colors to the dimmed colors
                dimmed = true;
                this.background.color = dimmedBGColor;
                this.actionButton.image.color = dimmedButtonColor;
            }
        } else {
            if (dimmed) {
                // if the item should not be dimmed but is, reset the color
                dimmed = false;
                this.background.color = defaultButtonColor;
                this.actionButton.image.color = defaultButtonColor;
            }
        }
    }

    /// <summary>
    /// Called when the object is destroyed, removes all listeners from the actionButton's onClick event
    /// </summary>
    private void OnDestroy() {
        actionButton.onClick.RemoveAllListeners();
    }
}
