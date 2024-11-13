using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Loot")]
    [SerializeField] int minCoins = 1;
    [SerializeField] int maxCoins = 4;

    [Header("Boring important stuff")]
    Rigidbody2D rb;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] Transform aimerTransform;
    [SerializeField] Inventory inventory;

    [Header("Stats")]
    [SerializeField] float maxHealth = 10;
    float currHealth;
    [SerializeField] float attackDamage = 5;
    [SerializeField] float attackCooldownTime = .5f;
    [SerializeField] float dodgeCooldownTime = .25f;

    [Header("Movement")]
    [SerializeField] float speed = 3;
    [SerializeField] float dodgeSpeed = .5f;
    Vector3 movement = Vector3.zero;
    
    [Header("Rotation")]
    [SerializeField] float rotationSpeed = 10;

    [Header("Combat")]
    Weapon weapon;
    bool attacking = false;
    bool attackCooldown = false;
    bool dodging = false;
    bool dodgeCooldown = false;
    bool invincible = false;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;

        if(this.CompareTag("Player")) {
            return;
        }
        DungeonManager.singleton.RegisterMonster();
    }

    void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    public void AimWeapon(Transform targetTransform) {
        AimWeapon(targetTransform.position);
    }

    public void AimWeapon(Vector3 aimPos) {
        Quaternion goalRotation = Quaternion.LookRotation(Vector3.forward, aimPos - aimerTransform.position);
        Quaternion currRotation = aimerTransform.rotation;

        aimerTransform.rotation = Quaternion.Lerp(currRotation, goalRotation, Time.deltaTime * rotationSpeed);
    }

    public void Move(Vector3 newMovement) {
        if(dodging) {
            return;
        }
        movement = newMovement;
    }

    public void MoveToward(Vector3 goalPos) {
        goalPos.z = 0;
        Vector3 direction = goalPos - transform.position;
        Move(direction.normalized);
    }

    
    public void Attack() {
        if(attacking || attackCooldown || dodging) {
            return;
        }
        weapon = inventory.GetHeldWeapon();
        StartCoroutine(AttackRoutine());
        IEnumerator AttackRoutine() {
            attacking = true;
            weapon.SetEnemyLayers(enemyLayers);
            weapon.transform.position = attackPoint.position;
            weapon.transform.rotation = aimerTransform.rotation * Quaternion.Euler(0,0,22.5f);
            weapon.gameObject.SetActive(true);
            weapon.GetTrail().emitting = true;
            float timer = 0;
            Quaternion baseRotation = weapon.transform.rotation;
            while(timer < weapon.GetSpeed()) {
                timer += Time.deltaTime;
                weapon.transform.rotation = baseRotation * Quaternion.Euler(0,0,timer/weapon.GetSpeed()*-135);
                yield return null;
            }
            weapon.transform.rotation = baseRotation * Quaternion.Euler(0,0,-135);
            /*//Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(weapon.transform.position, weapon.GetRange(), enemyLayers);

            // foreach(Collider2D enemy in hitEnemies) {
            //     enemy.GetComponent<Character>().TakeDamage(weapon.GetDamage());
            // }*/
            weapon.GetTrail().emitting = false;
            weapon.gameObject.SetActive(false);
            StartCoroutine(AttackCooldownRoutine());
            attacking = false;
        }

        /*// weapon.transform.position = attackPoint.position;
        // weapon.gameObject.SetActive(true);
        // Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, weapon.GetRange(), enemyLayers);

        // foreach(Collider2D enemy in hitEnemies) {
        //     enemy.GetComponent<Character>().TakeDamage(weapon.GetDamage());
        // }
        // weapon.gameObject.SetActive(false);*/
    }

    IEnumerator AttackCooldownRoutine() {
        attackCooldown = true;
        yield return new WaitForSeconds(attackCooldownTime);
        attackCooldown = false;
    }
    IEnumerator DodgeCooldownRoutine() {
        dodgeCooldown = true;
        yield return new WaitForSeconds(dodgeCooldownTime);
        dodgeCooldown = false;
    }


    public void Dodge() {
        if(dodging || movement == Vector3.zero) {
            return;
        }
        StartCoroutine(DodgeRoutine());
        IEnumerator DodgeRoutine() {
            dodging = true;
            invincible = true;
            Debug.Log("Dodging");
            float timer = 0;
            while(timer < dodgeSpeed) {
                timer += Time.deltaTime;
                int x = (movement.x < 0) ? -1 : 1;
                transform.rotation = Quaternion.Euler(0,0,timer/dodgeSpeed * 360 * -x);
                yield return null;
            }
            transform.rotation = Quaternion.identity;
            StartCoroutine(DodgeCooldownRoutine());
            invincible = false;
            dodging = false;
        }
        
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null) {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(float damage) {
        if(invincible) {
            return;
        }
        currHealth -= damage;

        //Play hurt animation??

        if(currHealth <= 0) {
            Die();
        }
    }

    void Die() {
        if(this.CompareTag("Player")) {
            DungeonManager.singleton.PlayerDead();
            return;
        }
        DungeonManager.singleton.GrantCoins(Random.Range(minCoins, maxCoins + 1));
        DungeonManager.singleton.MonsterDead();
        Destroy(this.gameObject);
    }
    
    public Inventory GetInventory() {
        return inventory;
    }

    public float GetSpeed() {
        return speed;
    }
}
