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
        if(transform.localPosition.y > 0) {
            //myCardinal = DoorCardinal.North;
            myCardinalInt = 0;
        } else if(transform.localPosition.x > 0) {
            //myCardinal = DoorCardinal.East;
            myCardinalInt = 1;
        } else if(transform.localPosition.y < 0) {
            //myCardinal = DoorCardinal.South;
            myCardinalInt = 2;
        } else if(transform.localPosition.x < 0) {
            //myCardinal = DoorCardinal.West;
            myCardinalInt = 3;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //DungeonManager.singleton.RegisterDoorCardinal(myCardinal);
    }

    // Update is called once per frame
    void Update()
    {
        
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
