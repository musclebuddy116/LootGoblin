using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] Character playerCharacter;

    // Update is called once per frame
    void Update()
    {
        playerCharacter.AimWeapon(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        if(Input.GetMouseButtonDown(1)) {
            playerCharacter.Dodge();
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            DungeonManager.singleton.ExitRoom();
        }
        if(Input.GetMouseButtonDown(0)) {
            playerCharacter.Attack();
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = Vector3.zero;
        if(Input.GetKey(KeyCode.W)) {
            movement += new Vector3(0,1,0);
        }
        if(Input.GetKey(KeyCode.A)) {
            movement += new Vector3(-1,0,0);
        }
        if(Input.GetKey(KeyCode.S)) {
            movement += new Vector3(0,-1,0);
        }
        if(Input.GetKey(KeyCode.D)) {
            movement += new Vector3(1,0,0);
        }
        playerCharacter.Move(movement);
    }

    public Character GetPlayerCharacter() {
        return playerCharacter;
    }
}
