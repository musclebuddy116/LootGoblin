using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] Character playerCharacter;
    [SerializeField] float scale = .1f;

    // Update is called once per frame
    void Update()
    {
        playerCharacter.AimWeapon(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        if(Input.GetMouseButtonDown(1)) {
            playerCharacter.Dodge();
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            DungeonManager.singleton.Interact();
            // DungeonManager.singleton.ExitRoom();
        }
        if(Input.GetMouseButtonDown(0)) {
            playerCharacter.Attack();
        }
        if(Input.mouseScrollDelta.y != 0) {
            playerCharacter.GetInventory().EquipNextWeapon((int)(Input.mouseScrollDelta.y * scale));
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
