using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, ICollectible
{
    public static event HandleItemCollect OnItemCollected;
    public delegate void HandleItemCollect(ItemData itemData);
    public ItemData item;
    public void Collect(){
        Destroy(gameObject);
        OnItemCollected?.Invoke(item);
    }
}
