using System;
using UnityEngine;

public class Wood : MonoBehaviour, IPickable
{
    public delegate void WoodCollected(ItemData itemData);
    public static event WoodCollected OnWoodPickedUp;
    [Header("Wood Item Data")]
    public ItemData woodData;

    public void PickUp()
    {
        Destroy(gameObject);
        OnWoodPickedUp?.Invoke(woodData);
    }

}