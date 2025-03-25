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
    [SerializeField] Weapon heldWeapon;
    int currWeaponIndex = 0;
    [SerializeField] int coins = 0;
    [SerializeField] bool hasStartWeapon;
    [SerializeField] Weapon startWeapon;
    int weaponCount = 0;

    void Start()
    {
        if(hasStartWeapon) {
            AddItem(startWeapon);
        }
    }

    public Weapon GetCurrWeapon() {
        if(weaponCount == 0) {
            return null;
        }
        if(currWeaponIndex >= items.Count) {
            currWeaponIndex = 0;
        }
        while(items[currWeaponIndex].GetType() != typeof(Weapon)) {
            EquipNextWeapon(1);
        }
        return (Weapon)items[currWeaponIndex];
    }

    public int GetCoins() {
        return coins;
    }
    
    public void AddCoins(int coins) {
        this.coins += coins;
    }

    public void AddItem(Item item) {
        items.Add(item);
        item.SetInventory(this);
        if(item.GetType() == typeof(Weapon)) {
            weaponCount++;
        }
    }

    public void RemoveItem(Item item) {
        items.Remove(item);
        if(item.GetType() == typeof(Weapon)) {
            weaponCount--;
        }
    }

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

    public void LoadItems(string key) {
        SaveLoad.SetfileName(Strings.inventoryFileName);
        SaveLoad.LoadFromFile();
        int[] ids = SaveLoad.LoadIntList(key);
        int i;
        if(ids.Length == 0 || ids[0] == -1) {
            Debug.Log("ids.Length: " + ids.Length);
            Debug.Log("ids[0]: " + ids[0]);
            return;
        }
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

    public void EquipNextWeapon(int y) {
        if(weaponCount == 0) {
            return;
        }
        if(currWeaponIndex >= items.Count) {
            currWeaponIndex = 0;
        }
        if(items[currWeaponIndex] != null) {
            items[currWeaponIndex].gameObject.SetActive(false);
        }
        for(int i = 0; i < Math.Abs(y); i++) {
            do {
                currWeaponIndex = (currWeaponIndex + items.Count + (1 * y/Math.Abs(y))) % items.Count;
            } while(items[currWeaponIndex].GetType() != typeof(Weapon) || ((Weapon)items[currWeaponIndex]).IsBroken());
        }
    }
}
