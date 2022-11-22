using System;

[Serializable] // Represents the inventory slot
public class InventoryItem
{
    public ItemData itemData;
    public int stackSize;

    public InventoryItem(ItemData item)
    {
        itemData = item;
        AddItemToStack(); // When an inventory slot is created, we create a stack slot
    }

    public void AddItemToStackMultiple(int itemAmount)
    {
        stackSize += itemAmount;
    }

    public void RemoveItemFromStackMultiple(int itemAmount)
    {
        if ((stackSize - itemAmount) < 0)
        {
            return;
        }
        stackSize -= itemAmount;
    }

    public void AddItemToStack()
    {
        stackSize++;
    }

    public void RemoveItemFromStack()
    {
        stackSize--;
    }

}

