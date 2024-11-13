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
        //moving = false;
        if(CanSeeTarget()) {
            ChangeState(AttackState);
            return;
        }
    }

    void AttackState() {
        //moving = true;
        //moveToward = playerCharacter.transform.position;
        
        //
        monster.MoveToward(playerCharacter.transform.position);
        //

        monster.AimWeapon(playerCharacter.transform);
        if(stateTime == 0) {
            currStateString = "AttackState";
        }

        if(Vector3.Distance(monster.transform.position, playerCharacter.transform.position) < 1) {
            //monster.MoveToward(moveToward);
            monster.Attack();
        }
        //ATTACK

        if(!CanSeeTarget()) {
            ChangeState(IdleState);
            return;
        }
    }

    void AITick() {
        if(justChangedState) {
            stateTime = 0;
            justChangedState = false;
        }
        currState();
        stateTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        /*if(moving) {
            monster.MoveToward(moveToward);
        }*/
    }
    
    // Update is called once per frame
    void Update()
    {
        AITick();
    }
}
