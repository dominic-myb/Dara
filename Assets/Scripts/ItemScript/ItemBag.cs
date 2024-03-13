using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<ItemData> itemList = new List<ItemData>();

    ItemData GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101); //1-100
        List<ItemData> possibleItems = new();
        foreach (ItemData item in itemList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0)
        {
            ItemData droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        Debug.Log("No loot dropped!");
        return null;
    }
    public void InstantiateLoot(Vector3 spawnPosition)
    {
        ItemData droppedItem = GetDroppedItem();
        if (droppedItem != null)
        {
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.itemSprite;

            // /*
            // --- We add the loot animation here ---

            
            float dropForce = 1f;
            Vector2 dropDirection = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
            lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection*dropForce, ForceMode2D.Impulse);
            // */
        }
    }
}
