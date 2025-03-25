using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public enum DoorCardinal{North, East, South, West};
    [SerializeField] DoorCardinal myCardinal;
    [SerializeField] int myCardinalInt;
    [SerializeField] bool atDoor = false;
    void Awake()
    {
        //THIS ASSUMES THAT DOOR IS ONLY EVER ONE CARDINAL DIRECTION
        if((int)transform.localPosition.y > 0) {
            myCardinalInt = 0;
        } else if((int)transform.localPosition.x > 0) {
            myCardinalInt = 1;
        } else if((int)transform.localPosition.y < 0) {
            myCardinalInt = 2;
        } else if((int)transform.localPosition.x < 0) {
            myCardinalInt = 3;
        }
    }

    public bool GetAtDoor() {
        return atDoor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) {
            atDoor = true;
            DungeonManager.singleton.SetAtDoor(myCardinalInt, true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player")) {
            atDoor = false;
            DungeonManager.singleton.SetAtDoor(myCardinalInt, false);
        }
    }
}
