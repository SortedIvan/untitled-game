using System;
using UnityEngine;

public class Stone : MonoBehaviour, IPickable
{
    public delegate void StoneCollected(ItemData itemData);
    public static event StoneCollected OnStonePickedUp;
    [Header("Stone Item Data")]
    public ItemData stoneData;

    public void PickUp()
    {
        Destroy(gameObject);
        OnStonePickedUp?.Invoke(stoneData);
    }
}
