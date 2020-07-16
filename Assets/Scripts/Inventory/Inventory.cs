using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<BaseItem> initialItems;
    public List<Item> items;
    public float money;

    protected virtual void Start() { Init(); }
    public virtual List<Item> Init() {
        items = items = new List<Item>();
        if (initialItems != null) {
            for (int i = 0; i < initialItems.Count; i++) {
                items.Add(new Item(initialItems[i]));
            }
        }
        return items;
    }

    public virtual void AddItem(Item item)
    {
        items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public List<Item> GetPrimaries()
    {
        return items.FindAll((item) => item.itemType == ItemType.Primary);
    }

    public List<Item> GetSecondaries()
    {
        return items.FindAll((item) => item.itemType == ItemType.Secondary);
    }
    public List<Item> GetEquipment()
    {
        return items.FindAll((item) => item.itemType == ItemType.Equipment);
    }

    public List<Item> GetShips()
    {
        return items.FindAll((item) => item.itemType == ItemType.Ship);
    }
    
    public float GetMoney()
    {
        return money;
    }

    public float AddMoney(float value)
    {
        money += value;
        return money;
    }

    public float SubtractMoney(float value)
    {
        money -= value;
        return money;
    }
}
