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
    [SerializeField] ParticleSystem particleSystem;
    bool canDamage = false;
    bool broken = false;
    
    [Header("Audio")]
    [SerializeField] AudioSource swingAudioSource;
    [SerializeField] AudioSource critAudioSource;
    [SerializeField] AudioSource breakAudioSource;

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
        swingAudioSource.Play();
    }

    public void Break() {
        breakAudioSource.Play();
        // gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = false;
        trail.emitting = false;
        broken = true;
        myInventory.RemoveItem(this);
        // myInventory.EquipNextWeapon(1);
        Destroy(this.gameObject,2f);
    }

    public bool IsBroken() {
        return broken;
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
            if(Random.Range(0, 1f) < critChance) {
                dmg *= 2;
                particleSystem.Play();
                critAudioSource.Play();
            }
            c.TakeDamage(dmg);
            CanDamage(false);
            UseDurability();
            if(currDurability <=0 ) {
                Break();
            }
        }
    }

    void OnDestroy()
    {
        particleSystem.Stop();
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

    public ParticleSystem GetParticle() {
        return particleSystem;
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
