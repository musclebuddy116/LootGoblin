using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{

    //NOTE: This does not work

    void Start()
    {
        //currHealth = maxHealth;
        Debug.Log("YO what's up guys, today we're back with Loot Goblin");
        DungeonManager.singleton.RegisterMonster();
    }

    void Die() {
        Debug.Log("I have been released from my mortal coil");
        DungeonManager.singleton.MonsterDead();
        Destroy(this.gameObject);
    }
}
