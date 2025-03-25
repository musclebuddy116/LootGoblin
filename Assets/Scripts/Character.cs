using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Loot")]
    [SerializeField] int minAwardCoins = 1;
    [SerializeField] int maxAwardCoins = 4;

    [Header("Boring important stuff")]
    Rigidbody2D rb;
    [SerializeField] Transform body;
    [SerializeField] SpriteRenderer healthBarSprite;
    [SerializeField] Transform attackPoint;
    // [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] Transform aimerTransform;
    [SerializeField] Inventory inventory;

    [Header("Stats")]
    [SerializeField] float maxHealth = 10;
    float currHealth;
    [SerializeField] float attackCooldownTime = .5f;
    [SerializeField] float dodgeCooldownTime = .25f;
    [SerializeField] float windupTime = .2f;
    [SerializeField] float deathFadeTime = 1;

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
    bool windingUp = false;
    bool invincible = false;
    bool dead = false;

    [Header("Audio")]
    [SerializeField] AudioSource owAudioSource;
    [SerializeField] AudioSource deathAudioSource;
    [SerializeField] AudioSource monsterAttackAudioSource;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        SetHealthBar();

        weapon = inventory.GetCurrWeapon();

        if(this.CompareTag("Player")) {
            return; // ENDS HERE FOR PLAYER --------------------
        }
        DungeonManager.singleton.RegisterMonster();
    }

    void Update()
    {
        if(!attacking && !attackCooldown) {
            weapon = inventory.GetCurrWeapon();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    public void AimWeapon(Transform targetTransform) {
        AimWeapon(targetTransform.position);
    }

    public void QuickAim(Transform targetTransform) {
        QuickAim(targetTransform.position);
    }

    public void QuickAim(Vector3 aimPos) {
        if(attacking || attackCooldown || weapon == null || windingUp) {
            return;
        }
        weapon.transform.position = attackPoint.position;
        weapon.gameObject.SetActive(true);
        Quaternion goalRotation = Quaternion.LookRotation(Vector3.forward, aimPos - aimerTransform.position);
        aimerTransform.rotation = goalRotation;
        weapon.transform.rotation = aimerTransform.rotation;
    }

    public void AimWeapon(Vector3 aimPos) {
        if(attacking || attackCooldown || weapon == null || windingUp) {
            return;
        }
        weapon.transform.position = attackPoint.position;
        weapon.gameObject.SetActive(true);
        Quaternion goalRotation = Quaternion.LookRotation(Vector3.forward, aimPos - aimerTransform.position);
        Quaternion currRotation = aimerTransform.rotation;

        aimerTransform.rotation = Quaternion.Lerp(currRotation, goalRotation, Time.deltaTime * rotationSpeed);
        weapon.transform.rotation = aimerTransform.rotation;
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

    public void Stop() {
        Move(Vector3.zero);
    }

    
    public void WindUp() {
        if(!CanAttack()) {
            return;
        }
        windingUp = true;
        StartCoroutine(WindUpRoutine());
        IEnumerator WindUpRoutine() {
            weapon.transform.rotation = aimerTransform.rotation;
            float timer = 0;
            monsterAttackAudioSource.Play();
            while(timer < windupTime/2) {
                timer += Time.deltaTime;
                weapon.transform.Rotate(new Vector3(0,0,Time.deltaTime/(windupTime/2) * 60));
                yield return null;
            }
            timer = 0;
            while(timer < windupTime/2) {
                timer += Time.deltaTime;
                weapon.transform.Rotate(new Vector3(0,0,Time.deltaTime/(windupTime/2) * -60));
                yield return null;
            }
            windingUp = false;
            Attack();
        }
    }
    
    public void Attack() {
        if(!CanAttack()) {
            return;
        }
        StartCoroutine(AttackRoutine());
        IEnumerator AttackRoutine() {
            attacking = true;
            weapon.SetEnemyLayers(enemyLayers);
            weapon.transform.position = attackPoint.position;
            weapon.transform.rotation = aimerTransform.rotation;
            weapon.transform.Rotate(new Vector3(0,0,45));
            
            weapon.GetTrail().emitting = true;
            weapon.CanDamage(true);
            float timer = 0;
            weapon.Swing();
            while(timer < weapon.GetSpeed()) {
                timer += Time.deltaTime;
                weapon.transform.Rotate(new Vector3(0,0,Time.deltaTime/weapon.GetSpeed() * -135));
                yield return null;
            }
            weapon.GetTrail().emitting = false;
            weapon.GetParticle().Stop();
            weapon.CanDamage(false);
            if(weapon == null || weapon.IsBroken()) {
                weapon = inventory.GetCurrWeapon();
            }
            
            StartCoroutine(AttackCooldownRoutine());
            attacking = false;
        }
    }

    IEnumerator AttackCooldownRoutine() {
        attackCooldown = true;
        float timer = 0;
        while(timer < attackCooldownTime) {
            timer += Time.deltaTime;
            if(weapon != null) {
                weapon.transform.Rotate(new Vector3(0,0,Time.deltaTime/weapon.GetSpeed() * -135 / 2));
            }
            yield return null;
        }

        attackCooldown = false;
    }
    IEnumerator DodgeCooldownRoutine() {
        dodgeCooldown = true;
        yield return new WaitForSeconds(dodgeCooldownTime);
        dodgeCooldown = false;
    }


    public void Dodge() {
        if(!CanDodge()) {
            return;
        }
        StartCoroutine(DodgeRoutine());
        IEnumerator DodgeRoutine() {
            dodging = true;
            invincible = true;
            float timer = 0;
            while(timer < dodgeSpeed) {
                timer += Time.deltaTime;
                int x = (movement.x < 0) ? -1 : 1;
                body.transform.rotation = Quaternion.Euler(0,0,timer/dodgeSpeed * 360 * -x);
                yield return null;
            }
            body.transform.rotation = Quaternion.identity;
            StartCoroutine(DodgeCooldownRoutine());
            invincible = false;
            dodging = false;
        }
        
    }

    public void TakeDamage(float damage) {
        if(invincible || dead) {
            return;
        }
        currHealth -= damage;
        SetHealthBar();

        if(currHealth <= 0) {
            Die();
        } else {
            owAudioSource.Play();
        }
    }

    public void SetHealthBar() {
        float value = currHealth / maxHealth;
        healthBarSprite.material.SetFloat("_Value", value);
    }

    void Die() {
        deathAudioSource.Play();
        dead = true;
        StartCoroutine(DieCoroutine());
        IEnumerator DieCoroutine() {
            float timer = 0;
            Color color = body.GetComponent<SpriteRenderer>().color;
            while(timer < deathFadeTime) {
                timer += Time.deltaTime;
                body.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1-timer/deathFadeTime);
                yield return null;
            }
            Destroy(this.gameObject);
            if(this.CompareTag("Player")) {
                DungeonManager.singleton.PlayerDead();
            }
            if(this.CompareTag("Monster")) {
                DungeonManager.singleton.GrantCoins(Random.Range(minAwardCoins, maxAwardCoins + 1));
                DungeonManager.singleton.MonsterDead();
            }
        }
    }
    
    public Inventory GetInventory() {
        return inventory;
    }

    public float GetSpeed() {
        return speed;
    }

    public bool CanAttack() {
        return !attacking && !attackCooldown && !dodging && !windingUp && weapon != null && !dead;
    }

    public bool CanDodge() {
        return !dodging && !dodgeCooldown && !(movement == Vector3.zero) && !attacking && !dead;
    }

    public bool IsAttacking() {
        return attacking;
    }
}
