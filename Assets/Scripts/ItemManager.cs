using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager singleton;
    // int identifier = 1;

    [SerializeField] List<Item> items;

    [Header("LOOT")]
    [SerializeField] float itemChance;
    [SerializeField] float uniqueChance;
    [SerializeField] float debuffChance = .25f;
    [SerializeField] float buffChance = .3f;
    

    void Awake()
    {
        if(singleton == null) {
            singleton = this;
        } else {
            Destroy(this.gameObject);
        }
        
    }
    
    /// <summary>
    /// Gets the Item at a specified index
    /// </summary>
    /// <param name="index"> The index from which to pull the item</param>
    /// <returns> The item at the specified index</returns>
    public Item GetItem(int index) {
        return items[index];
    }

    /// <summary>
    /// Creates a random Item from the items list, with a potential for debuff and/or buff
    /// </summary>
    /// <returns> The random Item, already instantiated </returns>
    public Item GetRandomItem() {
        int index = Random.Range(0, items.Count);
        Item item = Instantiate(items[index]).GetComponent<Item>();
        item.SetId(index);
        item.gameObject.SetActive(false);
        // Item item = items[Random.Range(0, items.Count)];
        if(!(item.GetType() == typeof(Weapon))) {
            return item;
        }
        Weapon weapon = (Weapon)item;
        float debuffRoll = Random.Range(0,1f);
        float buffRoll = Random.Range(0,1f);
        if(debuffRoll < debuffChance) {
            weapon.Buff(-1);
        }
        if(buffRoll < buffChance) {
            weapon.Buff(1);
        }
        return weapon;
    }

    public int GetCount() {
        return items.Count;
    }

    public List<Item> GetItems() {
        return items;
    }

    // public int Identify() {
    //     return identifier++;
    // }
}
