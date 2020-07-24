using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private PauseMenu pauseMenu;
    public GameObject pauseMenuPanel;

    public InventoryMenu inventoryMenu;
    public GameObject inventoryMenuPanel;

    public ShipSelectionMenu shipSelectionMenu;
    public GameObject shipSelectionMenuPanel;


    public static UIController instance;

    public PlayerUI playerUI;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        UIController.instance = this;
    }

    private void Start()
    {
        pauseMenu = GetComponent<PauseMenu>();
        inventoryMenu = GetComponent<InventoryMenu>();
        pauseMenu.Initialize(pauseMenuPanel);
        inventoryMenu.Initialize(inventoryMenuPanel);
        shipSelectionMenu.Initialize(shipSelectionMenuPanel);
    }

    public void OpenMenu(string selected) {
        OpenMenu((UIMenus) Enum.Parse(typeof(UIMenus), selected));
    }

    public void CloseAllMenus() {
        pauseMenu.Close();
        shipSelectionMenu.Close();
        inventoryMenu.Close();
    }
    public UIMenu OpenMenu(UIMenus selected) {
        switch (selected) {
            case UIMenus.Inventory:
                pauseMenu.Close();
                shipSelectionMenu.Close();
                inventoryMenu.Open();
                return inventoryMenu;
            case UIMenus.ShipSelection:
                pauseMenu.Close();
                inventoryMenu.Close();  
                shipSelectionMenu.Open();
                return shipSelectionMenu;
            case UIMenus.Pause:
                inventoryMenu.Close();
                shipSelectionMenu.Close();
                pauseMenu.Open();
                return pauseMenu;
            default:
                return pauseMenu;
        }
    }

    private void Update()
    {

        // if (Input.GetButtonDown("Pause"))
        // {
        //     if (pauseMenu.IsPaused())
        //     {
        //         pauseMenu.Close();
        //     }
        //     else
        //     {
        //         pauseMenu.Open();
        //     }
        // }        
    }
}
