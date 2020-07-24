using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryMenu : UIMenu
{
    private GameObject inventoryMenuPanel;

    private bool open;

    public Button buttonPrefab;

    public GameObject optionsContent;

    private List<OptionButton> currentButtons = new List<OptionButton>();

    public TextMeshProUGUI optionsTitleText;

    public GameObject tabPrefab;
    public GameObject tabsContent;

    public GameObject listItemPrefab;

    public GameObject listContent;
    private List<Button> listButtons;

    public GameObject promptPanel;
    
    [SerializeField]
    private TextMeshProUGUI promptText;
    [SerializeField]
    private Button promptYes;
    [SerializeField]
    private Button promptNo;
    [SerializeField]
    private Button promptOk;

    private Button[] tabs;

    public void Initialize(GameObject inventoryMenuPanel)
    {
        this.inventoryMenuPanel = inventoryMenuPanel;
        this.inventoryMenuPanel.SetActive(false);
        ClosePrompt();
    }

    public void SetOptionsTitle(string title) {
        optionsTitleText.text = title;
    }

    public void CreateOptionButton(string name, UnityAction onClick) {
        var newButton = GameObject.Instantiate(buttonPrefab, optionsContent.transform);
        var buttonComp = newButton.GetComponent<OptionButton>();
        buttonComp.SetValues(name, "", "", onClick);
        currentButtons.Add(buttonComp);
    }

    public void CreateOptionButton(string name, string cost, string hotkey, UnityAction onClick) {
        var newButton = GameObject.Instantiate(buttonPrefab, optionsContent.transform);
        var buttonComp = newButton.GetComponent<OptionButton>();
        buttonComp.SetValues(name, cost, hotkey, onClick);
        currentButtons.Add(buttonComp);
    }

    public void SelectFirstOption() {
        if (currentButtons.Count > 0) {
            EventSystem.current.SetSelectedGameObject(currentButtons[0].gameObject);
        }
    }

    public void Prompt(string text, UnityAction yesAction) {
        promptPanel.SetActive(true);
        promptText.text = text;
        promptYes.onClick.AddListener(yesAction);
        promptYes.onClick.AddListener(ClosePrompt);
        promptNo.onClick.AddListener(ClosePrompt);

        promptYes.gameObject.SetActive(true);
        promptNo.gameObject.SetActive(true);
    }
    
    public void Prompt(string text) {
        promptPanel.SetActive(true);
        promptText.text = text;
        promptOk.onClick.AddListener(ClosePrompt);

        promptOk.gameObject.SetActive(true);
    }

    public void ClosePrompt() {
        promptPanel.SetActive(false);
        promptYes.onClick.RemoveAllListeners();
        promptNo.onClick.RemoveAllListeners();
        promptOk.gameObject.SetActive(false);
        promptYes.gameObject.SetActive(false);
        promptNo.gameObject.SetActive(false);
    }
    
    public void ClearOptions() {
        foreach(Transform child in optionsContent.transform) {
            Destroy(child.gameObject);
        }
        currentButtons.Clear();
    }
    
    public override void Close()
    {
        // hide the inventory menu
        inventoryMenuPanel.SetActive(false);

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;

        // Remove old buttons from the options menu
        ClearOptions();

        // clear any remaining items from the item list
        ClearItemList();
    }
    
    public override void Open()
    {
        currentButtons[0].actionButton.Select();
        inventoryMenuPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SelectFirstOption();
    }


    public bool IsOpen()
    {
        return open;
    }

    public void PopulateTabs(TabDefinition[] tabs) {
        ClearTabs();
        
        for (int i = 0; i < tabs.Length; i++) {
            var currentTab = tabs[i];
            var tab = GameObject.Instantiate(tabPrefab, tabsContent.transform).GetComponent<Button>();
            currentTab.Setup(tab);            
            if (i == 0) {
                tab.onClick.Invoke(); // opens the first tab by default
            }
        }
    }

    public void ClearTabs() {
        ClearChildren(tabsContent.transform);
    }

    public void PopulateItemList(List<Item> items, string actionName, Action<Item, GameObject> action, Func<Item, bool> dimmedIf) {
        // destroy items from previous list
        ClearItemList();

        for (int i = 0; i < items.Count; i++) {
            // create new list item.
            var listItem = GameObject.Instantiate(listItemPrefab, listContent.transform);
            Item current = items[i];
            // set it's action button to the given action
            listItem.GetComponent<InventoryListItem>().Setup(current, ItemUtils.RarityColors[items[i].rarity], actionName, action, dimmedIf);
            listButtons.Add(listItem.GetComponent<Button>());
        }
    }

    public void ClearItemList() {
        ClearChildren(listContent.transform);
        listButtons = new List<Button>();
    }

    private void ClearChildren(Transform parent) {
        foreach(Transform child in parent) {
            Destroy(child.gameObject);
        }
    }
}

public class TabDefinition {
    public string name;
    public UnityAction onClick;
    public Color color;
    public TabDefinition(string name, UnityAction onClick, Color color) {
        this.name = name;
        this.onClick = onClick;
        this.color = color;
    }

    public void Setup(Button tabButton) {
        tabButton.GetComponentInChildren<TextMeshProUGUI>().text = name;
        tabButton.onClick.AddListener(onClick);
    }
}
