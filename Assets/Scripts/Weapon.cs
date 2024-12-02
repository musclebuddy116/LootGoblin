using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : Item
{
    [Header("Weapon")]
    [SerializeField] float damage = 5;
    // [SerializeField] float range = .35f;
    [SerializeField] float speed = .5f;
    [SerializeField] float knockback = 1;
    [SerializeField] float critChance = .05f;
    [SerializeField] float maxDurability = 10;
    [SerializeField] float currDurability = 10;

    [SerializeField] LayerMask enemyLayers;
    [SerializeField] TrailRenderer trail;
    bool canDamage = false;
    
    
    public float GetDamage() {  return damage;  }
    // public float GetRange() {  return range;  }
    public float GetSpeed() {  return speed;  }

    void Start()
    {
        currDurability = maxDurability;
    }

    public void CanDamage(bool can) {
        canDamage = can;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if((enemyLayers.value & (1 << other.gameObject.layer)) != 0) {
            if(!canDamage) {
                return;
            }
            Character c = other.GetComponent<Character>();
            float dmg = damage;
            if(Random.Range(0, 1f) < critChance) { dmg *= 2; }
            c.TakeDamage(dmg);
            CanDamage(false);
            UseDurability();
        }
    }
    /*void OnTriggerEnter2D(Collider2D other)
    {
        if((enemyLayers.value & (1 << other.gameObject.layer)) != 0) {
            if(!canDamage) {
                return;
            }
            Character c = other.GetComponent<Character>();
            float dmg = damage;
            if(Random.Range(0, 1f) < critChance) { dmg *= 2; }
            c.TakeDamage(dmg);
            UseDurability();
            CanDamage(false);
        }
        // if(other.CompareTag("Monster")) {
            
        // }
    }*/

    public void SetEnemyLayers(LayerMask enemyLayers) {
        this.enemyLayers = enemyLayers;
    }

    public TrailRenderer GetTrail() {
        return trail;
    }

    public void UseDurability() {
        currDurability--;
    }

    public float GetMaxDurability() {
        return maxDurability;
    }

    public float GetDurability() {
        return currDurability;
    }

    public void Debuff() {
        //Lowers a stat, with even chance to hit each stat, by a random amount
        //Damage, speed, knockback, crit chance, durability
        /*float debuffRoll = Random.Range(0,1f);
        if(debuffRoll < .2f) { //Debuff damage
            if(debuffRoll < .02f) { damage *= .7f ; }
            else if (debuffRoll < .1f) { damage *= .8f ; }
            else { damage *= .9f ; }
        }*/
    }

    public void Buff() {
        
    }
}
