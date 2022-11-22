using System;
using UnityEngine;

public class Arrow : MonoBehaviour, IPickable
{
    public delegate void ArrowCollected(ItemData itemData);
    public static event ArrowCollected OnArrowPickedUp;
    [Header("Arrow Item Data")]
    public ItemData arrowData;

    public void PickUp()
    {
        Destroy(gameObject);
        OnArrowPickedUp?.Invoke(arrowData);
    }

}