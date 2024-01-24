using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> inventory = new();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();
    private void OnEnable() {
        Item.OnItemCollected += Add;
    }
    private void OnDisable() {
        Item.OnItemCollected -= Add;
    }
    public void Add(ItemData itemData){
        if(itemDictionary.TryGetValue(itemData, out InventoryItem item)){
            item.AddToStack();
            Debug.Log($"{item.itemData.itemName} total stack is now {item.stackSize}");
        }else{
            InventoryItem newItem = new InventoryItem(itemData);
            if (inventory != null) {
                inventory.Add(newItem);
                Debug.Log($"Item {itemData.itemName} added to the inventory.");
            } else {
                Debug.LogError("Inventory is null. Make sure it is properly initialized.");
            }
            
            if (itemDictionary != null) {
                itemDictionary.Add(itemData, newItem);
                Debug.Log($"Added {itemData.itemName} for the first time.");
            } else {
                Debug.LogError("Item Dictionary is null. Make sure it is properly initialized.");
            }
        }
    }
    public void Remove(ItemData itemData){
        if(itemDictionary.TryGetValue(itemData, out InventoryItem item)){
            item.RemoveFromStack();
            if(item.stackSize == 0){
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
        }

    }
}
