using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> items;
    [SerializeField] Weapon heldWeapon;
    [SerializeField] int coins = 0;

    public Weapon GetHeldWeapon() {
        return heldWeapon;
    }

    public int GetCoins() {
        return coins;
    }

    public void AddCoins(int coins) {
        this.coins += coins;
    }

    void Start()
    {
        heldWeapon = (Weapon)items[0];
    }
}
