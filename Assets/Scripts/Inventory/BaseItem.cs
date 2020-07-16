using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 1)]
public class BaseItem : ScriptableObject
{
    public int cost;
    public ItemRarity rarity;
    public ItemType itemType;
    public GameObject prefab;
    public GameObject displayPrefab;
    public ShipStats shipStats;
}
