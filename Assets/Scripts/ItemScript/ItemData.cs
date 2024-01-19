using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public Sprite itemSprite;
    public string itemName;
    public  int dropChance;
    public ItemData(string itemName, int dropChance)
    {
        this.itemName = itemName;
        this.dropChance = dropChance;
    }
}
