using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : Item
{
    [SerializeField] float damage = 5;
    [SerializeField] float range = .35f;
    [SerializeField] float speed = .5f;
    [SerializeField] float knockback = 1;
    [SerializeField] float critChance = .05f;
    [SerializeField] float durability = 10;

    [SerializeField] LayerMask enemyLayers;
    [SerializeField] TrailRenderer trail;
    
    
    public float GetDamage() {  return damage;  }
    public float GetRange() {  return range;  }
    public float GetSpeed() {  return speed;  }

    void OnTriggerEnter2D(Collider2D other)
    {
        if((enemyLayers.value & (1 << other.gameObject.layer)) != 0) {
            Character c = other.GetComponent<Character>();
            c.TakeDamage(damage);
        }
        // if(other.CompareTag("Monster")) {
            
        // }
    }

    public void SetEnemyLayers(LayerMask enemyLayers) {
        this.enemyLayers = enemyLayers;
    }

    public TrailRenderer GetTrail() {
        return trail;
    }
}
