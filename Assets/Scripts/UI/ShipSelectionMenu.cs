using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class ShipSelectionMenu : UIMenu
{

    public Image accelerationDisplay;
    public Image topSpeedDisplay;
    public Image maneuverabilityDisplay;
    public Image maxHealthDisplay;
    public Image maxShieldDisplay;
    public Image shieldRechargeDisplay;

    private BarDisplay acceleration;
    private BarDisplay topSpeed;
    private BarDisplay maneuverability;
    private BarDisplay maxHealth;
    private BarDisplay maxShield;
    private BarDisplay shieldRecharge;

    public TextMeshProUGUI primaryHardpointsText;
    public TextMeshProUGUI secondaryHardpointsText;
    public TextMeshProUGUI equipmentHardpointsText;

    public TextMeshProUGUI shipNameText;
    public TextMeshProUGUI shipRarityText;
    public TextMeshProUGUI costText;

    public float statMax = 10f;

    public float barFillSpeed = 5f;

    public GameObject buttonPrefab;
    public Transform optionsContent;

    private List<OptionButton> currentButtons = new List<OptionButton>();

    public GameObject shipSelectionMenuPanel;

    public Button closeButton;

    public void Initialize(GameObject shipSelectionMenuPanel)  {
        acceleration = new BarDisplay(accelerationDisplay, barFillSpeed);
        topSpeed = new BarDisplay(topSpeedDisplay, barFillSpeed);
        maneuverability = new BarDisplay(maneuverabilityDisplay, barFillSpeed);
        maxHealth = new BarDisplay(maxHealthDisplay, barFillSpeed);
        maxShield = new BarDisplay(maxShieldDisplay, barFillSpeed);
        shieldRecharge = new BarDisplay(shieldRechargeDisplay, barFillSpeed);
        this.shipSelectionMenuPanel = shipSelectionMenuPanel;
        this.shipSelectionMenuPanel.SetActive(false);

        closeButton.onClick.AddListener(ClearActions);
    }

    public void Populate(Item shipItem) {
        var stats = shipItem.baseItem.shipStats;
        if (stats == null) {
            acceleration.SetAmount(0);
            topSpeed.SetAmount(0);
            maneuverability.SetAmount(0);
            maxHealth.SetAmount(0);
            maxShield.SetAmount(0);
            shieldRecharge.SetAmount(0);

            primaryHardpointsText.text = "0";
            secondaryHardpointsText.text = "0";
            equipmentHardpointsText.text = "0";
            costText.text = "";

            shipNameText.text = "None";
            shipRarityText.text = "";
            shipRarityText.color = ItemUtils.RarityColors[0];
            return;
        }
        acceleration.SetAmount(stats.acceleration / statMax);
        topSpeed.SetAmount(stats.topSpeed / statMax);
        maneuverability.SetAmount(stats.maneuverability / statMax);
        maxHealth.SetAmount(stats.maxHealth / statMax);
        maxShield.SetAmount(stats.maxShield / statMax);
        shieldRecharge.SetAmount(stats.shieldRecharge / statMax);

        primaryHardpointsText.text = stats.primaryHardpoints.ToString();
        secondaryHardpointsText.text = stats.secondaryHardpoints.ToString();
        equipmentHardpointsText.text = stats.equipmentHardpoints.ToString();

        costText.text = shipItem.cost.ToString();

        shipNameText.text = shipItem.name;
        shipRarityText.text = shipItem.rarity.ToString();
        shipRarityText.color = ItemUtils.RarityColors[shipItem.rarity];
    }

    public void AddAction(string label, UnityAction onClickAction) {
        var newButton = GameObject.Instantiate(buttonPrefab, optionsContent.transform);
        var buttonComp = newButton.GetComponent<OptionButton>();
        buttonComp.SetValues(label, "", "", onClickAction);
        currentButtons.Add(buttonComp);
        EventSystem.current.SetSelectedGameObject(newButton);
    }

    public void ClearActions() {
        for (int i = 0; i < currentButtons.Count; i++) {
            Destroy(currentButtons[i].gameObject);
        }
        currentButtons.Clear();
    }

    private void Update() {
        acceleration.Update();
        topSpeed.Update();
        maneuverability.Update();
        maxHealth.Update();
        maxShield.Update();
        shieldRecharge.Update();
    }

    public void UpdateFillAmount(Image image, float amount) {
        image.fillAmount = amount;
    }

    public override void Open()
    {
        shipSelectionMenuPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void Close() {
        shipSelectionMenuPanel.SetActive(false);
        // Lock the cursor and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private class BarDisplay {

        float barFillSpeed;
        Image image;
        float currentAmount = 0f;
        float targetAmount = 0f;

        public BarDisplay(Image image, float barFillSpeed) {
            this.image = image;
            this.image.fillAmount = currentAmount;
            this.barFillSpeed = barFillSpeed;
        }

        public void Update() {
            currentAmount = Mathf.Lerp(currentAmount, targetAmount, Time.deltaTime * barFillSpeed);
            image.fillAmount = currentAmount;
        }

        public void SetAmount(float amount) {
            this.targetAmount = amount;
        }
    }
}
