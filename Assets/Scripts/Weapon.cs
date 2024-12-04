using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : Item
{
    [Header("Weapon")]
    [SerializeField] float damage = 5;
    // [SerializeField] float range = .35f;
    [SerializeField] float swingTime = .5f;
    [SerializeField] float knockback = 1;
    [SerializeField] float critChance = .05f;
    [SerializeField] float maxDurability = 10;
    [SerializeField] float currDurability = 10;

    [SerializeField] LayerMask enemyLayers;
    [SerializeField] TrailRenderer trail;
    bool canDamage = false;
    
    [Header("Audio")]
    [SerializeField] AudioSource audioSource;

    [Header("ProceduralGeneration")]
    [SerializeField] float minDamageMod = 1;
    [SerializeField] float maxDamageMod = 3;
    [SerializeField] float minSwingTimeMod = .05f;
    [SerializeField] float maxSwingTimeMod = .15f;
    [SerializeField] float minCritChanceMod = .02f;
    [SerializeField] float maxCritChanceMod = .05f;
    [SerializeField] int minDurabilityMod = 1;
    [SerializeField] int maxDurabilityMod = 3;

    
    
    public float GetDamage() {  return damage;  }
    // public float GetRange() {  return range;  }
    public float GetSpeed() {  return swingTime;  }

    // void Start()
    // {
    //     currDurability = maxDurability;
    // }

    void Awake()
    {
        currDurability = maxDurability;
    }

    public void Swing() {
        audioSource.Play();
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

    /// <summary>
    /// Raises or lowers a stat based on sign value
    /// Then alters weapon value accordingly
    /// </summary>
    /// <param name="sign"> 1 for buff, -1 for debuff </param>
    
    public void Buff(int sign) {
        float buffRoll = Random.Range(0,1f);
        if(buffRoll < .25f) { damage += (Random.Range(minDamageMod, maxDamageMod) * sign); }
        else if (buffRoll < .5f) { swingTime -= (Random.Range(minSwingTimeMod, maxSwingTimeMod) * sign); }
        else if (buffRoll < .75f) { critChance += (Random.Range(minCritChanceMod, maxCritChanceMod) * sign); }
        else { maxDurability += (Random.Range(minDurabilityMod, maxDurabilityMod) * sign);
            currDurability = maxDurability; }
        value += sign;
    }
}
