using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> _itemDictionary = new Dictionary<ItemData, InventoryItem>();


    // To do: Make extendable for multiple items in inventory;
    private void OnEnable()
    {
        Stone.OnStonePickedUp += AddToInventory;
        Wood.OnWoodPickedUp += AddToInventory;
    }

    private void OnDisable()
    {
        Stone.OnStonePickedUp -= AddToInventory;
        Wood.OnWoodPickedUp -= AddToInventory;
    }

    public void AddToInventory(ItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            // If we already have the item in the inventory, we add one to its stack
            item.AddItemToStack();
            Debug.Log($"{item.itemData.displayName} amount is: {item.stackSize}");
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            _itemDictionary.Add(itemData, newItem);
            Debug.Log($"{newItem.itemData.displayName} amount is: {newItem.stackSize}");
        }
    }

    public void RemoveFromInventory(ItemData itemData)
    {
        if (_itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveItemFromStack();
            if (item.stackSize == 0)
            {
                // If after removing the item we are at 0, we remove the item itself from the inventory
                // and remove the item slot as well
                inventory.Remove(item);
                _itemDictionary.Remove(itemData);
                Debug.Log($"{item.itemData.displayName} amount is: {item.stackSize}");
            }
        }
    }
}
