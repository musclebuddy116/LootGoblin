using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items;
    // [SerializeField] LinkedList<Item> items;
    [SerializeField] Weapon heldWeapon;
    int currWeaponIndex = 0;
    [SerializeField] int coins = 0;
    [SerializeField] bool hasStartWeapon;
    [SerializeField] Weapon startWeapon;
    int weaponCount = 0;


    void Awake()
    {
        // items = new LinkedList<Item>();
    }

    void Start()
    {
        if(hasStartWeapon) {
            AddItem(startWeapon);
        }
        /*if(items.Count > 0) {
            heldWeapon = (Weapon)items[0];
        }*/
        /*if(items.Count > 0) {
            heldWeapon = (Weapon)items.First.Value;
        }*/
    }

    public Weapon GetCurrWeapon() {
        if(weaponCount == 0) {
            return null;
        }
        while(items[currWeaponIndex].GetType() != typeof(Weapon)) {
            // currWeaponIndex = ++currWeaponIndex % items.Count;
            EquipNextWeapon(1);
        }
        // items[currWeaponIndex].gameObject.SetActive(true);
        return (Weapon)items[currWeaponIndex];
        /*if(heldWeapon == null && items.Count > 0) {
            heldWeapon = (Weapon)items[0];
        }
        return heldWeapon;*/
    }

    /*public Weapon GetHeldWeapon() {
        if(heldWeapon == null && items.Count > 0) {
            heldWeapon = (Weapon)items.First.Value;
        }
        return heldWeapon;
    }*/

    public int GetCoins() {
        return coins;
    }
    
    public void AddCoins(int coins) {
        this.coins += coins;
    }

    public void AddItem(Item item) {
        items.Add(item);
        item.transform.SetParent(this.transform);
        if(item.GetType() == typeof(Weapon)) {
            weaponCount++;
        }
    }

    /*public void AddItem(Item item) {
        LinkedListNode<Item> node = new LinkedListNode<Item>(item);
        items.AddLast(node);
        //items.AddLast(item);
    }*/

    public void RemoveItem(Item item) {
        items.Remove(item);
        if(item.GetType() == typeof(Weapon)) {
            weaponCount--;
        }
    }

    /*public void RemoveItem(Item item) {
        items.Remove(item);
    }*/

    /*public void BuyItem(Item item) {
        if(coins < item.GetValue()) {
            return -1;
        }
        coins -= item.GetValue();
        AddItem(item);
    }*/

    public int BuyItem(Item item) {
        if(coins < item.GetValue()) {
            return -1;
        }
        coins -= item.GetValue();
        AddItem(item);
        return 0;
    }

    public void SaveItems(string key) {
        SaveLoad.SetfileName(Strings.inventoryFileName);
        string itemStr = "";
        if(items.Count > 0) { //Grabbing first for clean item separation
            itemStr += items[0].GetId().ToString();
        } else {
            itemStr = "-1";
        }
        int i;
        for(i = 1; i < items.Count; i++) {
            itemStr += "," + items[i].GetId().ToString();
        }

        SaveLoad.SaveString(key, itemStr);
        SaveLoad.Flush();
    }

    public void SaveItemsEmpty(string key) {
        SaveLoad.SetfileName(Strings.inventoryFileName);
        string itemStr = "-1";
        SaveLoad.SaveString(key, itemStr);
        SaveLoad.Flush();
    }

    public void SaveCoins (string key) {
        SaveLoad.SetfileName(Strings.inventoryFileName);
        SaveLoad.SaveInt(key, coins);
        SaveLoad.Flush();
    }

    /*public void SaveInventory(string key) {
        SaveLoad.SetfileName("Inventory");
        string itemStr = "";
        if(items.Count > 0) { //Grabbing first for clean item separation
            itemStr+= items.First.Value.GetId().ToString();
        }
        LinkedListNode<Item> node = items.First.Next;
        while(node != null) {
            itemStr += "," + node.Value.GetId().ToString();
            node = node.Next;
        }

        SaveLoad.SaveString(key, itemStr);
        SaveLoad.Flush();
    }*/

    public void LoadItems(string key) {
        SaveLoad.SetfileName(Strings.inventoryFileName);
        SaveLoad.LoadFromFile();
        int[] ids = SaveLoad.LoadIntList(key);
        int i;
        if(ids.Length == 0 || ids[0] == -1) {
            return;
        }
        // int i;
        for(i = 0; i < ids.Length; i++) {
            Item item = Instantiate(ItemManager.singleton.GetItem(ids[i]), Vector3.zero, Quaternion.identity, this.transform).GetComponent<Item>();
            item.SetId(ids[i]);
            AddItem(item);
            //if(items[currWeaponIndex] != item) { 
                item.gameObject.SetActive(false);
                //}
        }
    }
    
    public void LoadCoins(string key) {
        SaveLoad.SetfileName(Strings.inventoryFileName);
        SaveLoad.LoadFromFile();
        coins = SaveLoad.LoadInt(key);
    }

    /*public void LoadInventory(string key) {
        SaveLoad.SetfileName("Inventory");
        SaveLoad.LoadFromFile();
        int[] ids = SaveLoad.LoadIntList(key);
        for(int i = 0; i < ids.Length; i++) {
            Item item = Instantiate(ItemManager.singleton.GetItem(ids[i]), this.transform).GetComponent<Item>();
            item.SetId(ids[i]);
            Debug.Log("i: " + i + "\nid: " + ids[i] + "\nitemId: " + item.GetId());
            AddItem(item);
        }
    }*/

    public void EquipNextWeapon(int y) {
        if(weaponCount == 0) {
            return;
        }
        if(items[currWeaponIndex] != null) {
            items[currWeaponIndex].gameObject.SetActive(false);
        }
        for(int i = 0; i < Math.Abs(y); i++) {
            do {
                currWeaponIndex = (currWeaponIndex + items.Count + (1 * y/Math.Abs(y))) % items.Count;
            } while(items[currWeaponIndex].GetType() != typeof(Weapon));
        }
        /*for(int i = 0; i < -y; i++) {
            do {
                currWeaponIndex = (--currWeaponIndex + items.Count) % items.Count;
            } while(items[currWeaponIndex].GetType() != typeof(Weapon));
        }*/
        // items[currWeaponIndex].gameObject.SetActive(true);
        // currWeaponIndex = (currWeaponIndex + y) % items.Count;
    }

    /*public void RotateInventory(float y) {
        Debug.Log("Rotating inventory");
        if(y > 0) {
            Debug.Log("Up");
            for(int i = 0; i < y; i++) {
                Weapon weapon = GetHeldWeapon();
                items.Remove(weapon);
                items.Add(weapon);
                GetHeldWeapon();
            }
        } else if (y < 0) {
            Debug.Log("Down");
            for(int i = 0; i < -y; i++) {
                Weapon weapon = (Weapon)items[items.Count-1];
                for(int j = items.Count-2; j > 0; j--) {
                    items[j+1] = items[j];
                    GetHeldWeapon();
                }
                items[0] = weapon;
                GetHeldWeapon();
            }
        }
        
    }*/

    /*public void RotateInventory(float y) {
        Debug.Log("Rotating inventory");
        if(y > 0) {
            Debug.Log("Up");
            LinkedListNode<Item> node = items.First;
            heldWeapon = (Weapon)node.Next.Value;
            items.RemoveFirst();
            items.AddLast(node);
            
        } else if (y < 0) {
            Debug.Log("Down");
            LinkedListNode<Item> node = items.Last;
            heldWeapon = (Weapon)node.Value;
            items.RemoveLast();
            items.AddFirst(node);
        }
    }*/
}
