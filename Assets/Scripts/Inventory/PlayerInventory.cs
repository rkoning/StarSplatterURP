using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    public List<BaseItem> initialEquipped;

    public BaseItem initialShip;

    public Item currentShip;

    public Dictionary<Item, GameObject> equipmentMap = new Dictionary<Item, GameObject>();

    public PlayerFighter playerFighter;

    public Player player;

    public GameObject playerSpeedParticleOuter;
    public GameObject playerSpeedParticleInner;

    private Transform dockTransform;

    protected override void Start() {  }

    public override List<Item> Init() {
        base.Init();
        currentShip = new Item(initialShip);
        items.Add(currentShip);

        List<Item> toEquip = new List<Item>();
        
        for (int i = 0; i < initialEquipped.Count; i++) {
            for (int k = 0; k < items.Count; k++) {
                if (initialEquipped[i] == items[k].baseItem)
                    toEquip.Add(items[i]);
            }
        }
        return toEquip;
    }

    public GameObject EquipShip(Vector3 position, Quaternion rotation) {
        return EquipShip(currentShip, position, rotation);
    }

    public GameObject EquipShip(Item shipItem, Vector3 position, Quaternion rotation) {
        GameObject shipPrefab = shipItem.prefab;
        // *
        // * Create ship
        // *
        var shipGameObject = GameObject.Instantiate(shipPrefab, position, rotation);
        shipGameObject.layer = 12; // PlayerObject layer
        var ship = shipGameObject.GetComponent<Ship>();

        // *
        // * Assign ship MinorFaction
        // *
        // TODO
        // ship.Faction = faction;
        // faction.AddTargetable(shipGameObject.GetComponent<Health>());

        // *
        // * Setup particles and add PlayerFighter to the ship
        // *
        var playerFighter = shipGameObject.GetComponent<PlayerFighter>();
        playerFighter.player = player;
        playerFighter.playerInput = player.input;
        this.playerFighter = playerFighter;
        // this.playerFighter.Init();
                
        List<Item> equipped = new List<Item>();
        foreach(Item key in equipmentMap.Keys) {
            // Debug.Log(items.Find((item) => item.id == key.id && item.itemType != ItemType.Ship));
            equipped.Add(items.Find((item) => item.id == key.id && item.itemType != ItemType.Ship));
        }
        equipmentMap.Clear();
        foreach(Item toEquip in equipped) {
            EquipItem(toEquip);
        }

        // UIController.instance.playerUI.SetPlayerFighter(this.playerFighter);

        equipmentMap.Add(shipItem, shipGameObject);

        return shipGameObject;
    }

    public void EquipItem(Item i) {
        Weapon temp = null;
        switch (i.itemType) {
            case ItemType.Ship:
                // Destroy old ship
                if (this.playerFighter && currentShip != null) {
                    UnequipItem(currentShip);
                }
                // set currentShip to the new one
                currentShip = i;
                // equip the new ship
                EquipShip(dockTransform.position, dockTransform.rotation);
                // Camera needs to be reset to new player fighter
                player.camera.target = this.playerFighter.transform;
                return;
            case ItemType.Primary:
                if (playerFighter.primaryAnchors.Length <= 0)
                    break;
                temp = InstantiateWeapon(
                    i.prefab.GetComponent<Weapon>(),
                    playerFighter.transform,
                    playerFighter.primaryAnchors
                );
                playerFighter.primary = temp;
                // playerFighter.primary.owner = playerFighter.GetComponent<Target>();
                break;
            case ItemType.Secondary:
                if (playerFighter.secondaryAnchors.Length <= 0)
                    break;
                temp = InstantiateWeapon(
                    i.prefab.GetComponent<Weapon>(),
                    playerFighter.transform,
                    playerFighter.secondaryAnchors
                );
                playerFighter.secondary = temp;
                // playerFighter.secondary.owner = playerFighter.GetComponent<Target>();
                break;
            case ItemType.Equipment:
                if (playerFighter.equipmentAnchors.Length <= 0)
                    break;
                temp = InstantiateWeapon(
                    i.prefab.GetComponent<Weapon>(),
                    playerFighter.transform,
                    playerFighter.equipmentAnchors
                );
                playerFighter.equipment = temp;
                // playerFighter.equipment.owner = playerFighter.GetComponent<Target>();
                break;
        }

        // sets the faction of the ship's weapons
        if (playerFighter) {
            playerFighter.SetupWeapons();
        }

        List<Item> toRemove = new List<Item>();
        foreach (KeyValuePair<Item, GameObject> kvp in equipmentMap) {
            if (kvp.Key.itemType == i.itemType) {
                toRemove.Add(kvp.Key);
            }
        }

        foreach (Item item in toRemove) {
            UnequipItem(item);
        }

        equipmentMap.Add(i, temp.gameObject);
    }

    public void UnequipItem(Item i) {
        Debug.Log(i.name + " " + i.id);
        Debug.Log(equipmentMap[i].gameObject.name);
        Destroy(equipmentMap[i].gameObject);
        equipmentMap.Remove(i);
        if (i.itemType == ItemType.Ship) {
            currentShip = null;
            playerFighter = null;
        }

    }

    private Weapon InstantiateWeapon(Weapon weapon, Transform ship, Transform[] anchors) {
        var weaponObject = GameObject.Instantiate(weapon.gameObject, ship);
        var newWeapon = weaponObject.GetComponent<Weapon>();
        newWeapon.ResetProjectors();
        foreach(var a in anchors) {
            newWeapon.AddProjectorTo(a);
        }
        newWeapon.SetupFiremodes();

        return newWeapon;
    }

    public void SetDockInfo(Transform dockTransform) {
        this.dockTransform = dockTransform;
    }
}
