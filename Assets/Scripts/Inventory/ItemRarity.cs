using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemRarity {
    Common,
    Rare,
    Epic,
    Legendary,
}

public static class ItemUtils {
    public static Dictionary<ItemRarity, Color> RarityColors = new Dictionary<ItemRarity, Color>() {
        { ItemRarity.Common, new Color32(255, 255, 255, 255) },
        { ItemRarity.Rare, new Color32(0, 187,239, 255) },
        { ItemRarity.Epic, new Color32(178, 59, 115, 255) },
        { ItemRarity.Legendary, new Color32(255, 91, 32, 255) },
    };

        public static Dictionary<ItemRarity, int> RarityIndices = new Dictionary<ItemRarity, int>() {
        { ItemRarity.Common, 0 },
        { ItemRarity.Rare, 1 },
        { ItemRarity.Epic, 2 },
        { ItemRarity.Legendary, 3 },
    };
}