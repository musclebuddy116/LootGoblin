using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] string currStateString;
    [SerializeField] Character monster;
    [SerializeField] Character playerCharacter;

    [Header("Config")]
    [SerializeField] float sightDistance = 5;
    [SerializeField] float meleeDistance = 1.5f;

    delegate void AIState();
    AIState currState;

    //---------- Trackers ----------
    float stateTime = 0;
    bool justChangedState = false;
    bool moving = false;
    Vector3 moveToward;

    void Awake()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(IdleState);
        playerCharacter = DungeonManager.singleton.GetPC();
    }

    void ChangeState(AIState newState) {
        currState = newState;
        justChangedState = true;
    }

    bool CanSeeTarget() {
        return Vector3.Distance(monster.transform.position, playerCharacter.transform.position) < sightDistance;
    }

    void IdleState() {
        if(stateTime == 0) {
            currStateString = "IdleState";
        }
        monster.Stop();
        if(CanSeeTarget()) {
            ChangeState(ChaseState);
            return;
        }
    }

    void ChaseState() {
        monster.MoveToward(playerCharacter.transform.position);

        monster.AimWeapon(playerCharacter.transform);
        if(stateTime == 0) {
            currStateString = "ChaseState";
        }

        if(Vector3.Distance(monster.transform.position, playerCharacter.transform.position) < meleeDistance) {
            ChangeState(AttackState);
            return;
        }

        if(!CanSeeTarget()) {
            ChangeState(IdleState);
            return;
        }
    }

    void AttackState() {
        if(stateTime == 0) {
            currStateString = "AttackState";
        }
        monster.Stop();
        monster.QuickAim(playerCharacter.transform);
        if(!monster.CanAttack()) {
            return;
        }
        if(Vector3.Distance(monster.transform.position, playerCharacter.transform.position) > meleeDistance) {
            ChangeState(ChaseState);
            return;
        }
        monster.WindUp();
    }

    void AITick() {
        if(justChangedState) {
            stateTime = 0;
            justChangedState = false;
        }
        currState();
        stateTime += Time.deltaTime;
    }
    
    // Update is called once per frame
    void Update()
    {
        AITick();
    }
}
