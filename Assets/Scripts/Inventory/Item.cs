using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Item {

    public string name;
    public int cost;
    public ItemRarity rarity;
    public ItemType itemType;
    public GameObject prefab;
    public GameObject displayPrefab;

    public System.Guid id;

    public BaseItem baseItem;

    public Item(BaseItem baseItem) {
        this.id = System.Guid.NewGuid();
        this.name = baseItem.name;
        this.baseItem = baseItem;
        this.rarity = baseItem.rarity;
        this.cost = baseItem.cost;
        this.itemType = baseItem.itemType;
        this.prefab = baseItem.prefab;
        this.displayPrefab = baseItem.displayPrefab;
    }
}
