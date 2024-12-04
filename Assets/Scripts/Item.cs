using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    int id = -1;
    // int identifier = -1;
    [SerializeField] protected int value = 5;
    [SerializeField] float dropChance = .01f;

    public void SetId(int id) {
        this.id = id;
        // identifier = ItemManager.singleton.Identify();
    }

    public int GetId() {
        return id;
    }
    
    public String GetName() {
        return name[..^7];
    }

    public int GetValue() {
        return value;
    }

}
