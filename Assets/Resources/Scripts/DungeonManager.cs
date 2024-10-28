using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager singleton;

    [SerializeField] Character pc;
    [SerializeField] ScreenFader dungeonScreenFader;
    [Header("Rooms")]
    [SerializeField] List<GameObject> roomPrefabs;
    [SerializeField] GameObject currRoom;
    Room currRoomRoom;
    //[SerializeField] GameObject northRoom, eastRoom, southRoom, westRoom;
    [SerializeField] GameObject[] rooms = new GameObject[4]; //NESW, adjusted for room rotations
    //[SerializeField] Room northRoomRoom, eastRoomRoom, southRoomRoom, westRoomRoom;
    //[SerializeField] DoorTrigger.DoorCardinal atDoorCardinal;
    [SerializeField] int atDoor;
    bool playerAtDoor;
    bool exiting = false;
    int rotations = 0;

    void Awake()
    {
        if(singleton == null) {
            singleton = this;
        } else {
            Destroy(this.gameObject);
        }
        Generate(Random.Range(int.MinValue, int.MaxValue));
    }

    void Generate(int seed) {
        Random.InitState(seed);
    }

    // public void RegisterRoom(Room r) {
    //     rooms.Add(r);
    // }
    // Start is called before the first frame update
    void Start()
    {
        EnterRoom(roomPrefabs[0], 2);
        // pc.transform.position = Vector3.zero;
        // currRoom = Instantiate(roomPrefabs[0], Vector3.zero, Quaternion.identity);
        // currRoomRoom = currRoom.GetComponent<Room>();
        //RollRoom();
        
    }

    public void RollRoom() {
        int i;
        for(i = 0; i < 4; i++) {
            rooms[(i + 4 - rotations) % 4] = (currRoomRoom.GetExit(i)) ? roomPrefabs[Random.Range(1, roomPrefabs.Count)] : null;
            /*if(currRoomRoom.GetExit(i)) {
                rooms[(i + 4 - rotations) % 4] = roomPrefabs[Random.Range(1, roomPrefabs.Count)];
            } else {
                rooms[(i + 4 - rotations) % 4] = null;
            }*/
        }
        /*if(currRoomRoom.GetNorthExit()) {
            rooms[(0 + 4 - rotations) % 4] = roomPrefabs[Random.Range(1, roomPrefabs.Count)];
        } else {  rooms[(0 + 4 - rotations) % 4] = null;  }
        if(currRoomRoom.GetEastExit()) {
            rooms[(1 + 4 - rotations) % 4] = roomPrefabs[Random.Range(1, roomPrefabs.Count)];
        } else {  rooms[(1 + 4 - rotations) % 4] = null;  }
        if(currRoomRoom.GetSouthExit()) {
            rooms[(2 + 4 - rotations) % 4] = roomPrefabs[Random.Range(1, roomPrefabs.Count)];
        } else {  rooms[(2 + 4 - rotations) % 4] = null;  }
        if(currRoomRoom.GetWestExit()) {
            rooms[(3 + 4 - rotations) % 4] = roomPrefabs[Random.Range(1, roomPrefabs.Count)];
        } else {  rooms[(3 + 4 - rotations) % 4] = null;  }*/
        /*// if(currRoomRoom.GetNorthExit()) {
        //     northRoom = roomPrefabs[Random.Range(1, roomPrefabs.Count)];
        //     northRoomRoom = northRoom.GetComponent<Room>();
        // } else {  northRoom = null; northRoomRoom = null;  }
        // if(currRoomRoom.GetEastExit()) {
        //     eastRoom = roomPrefabs[Random.Range(1, roomPrefabs.Count)];
        //     eastRoomRoom = eastRoom.GetComponent<Room>();
        // } else {  eastRoom = null; eastRoomRoom = null;  }
        // if(currRoomRoom.GetSouthExit()) {
        //     southRoom = roomPrefabs[Random.Range(1, roomPrefabs.Count)];
        //     southRoomRoom = southRoom.GetComponent<Room>();
        // } else {  southRoom = null; southRoomRoom = null;  }
        // if(currRoomRoom.GetWestExit()) {
        //     westRoom = roomPrefabs[Random.Range(1, roomPrefabs.Count)];
        //     westRoomRoom = westRoom.GetComponent<Room>();
        // } else {  westRoom = null; westRoomRoom = null;  }*/

    }

    /*public void RegisterDoorCardinal(DoorTrigger.DoorCardinal doorCardinal) {
        // if(doorCardinal == DoorTrigger.DoorCardinal.North)  {   currRoomRoom.SetNorthExit(true);    }
        // if(doorCardinal == DoorTrigger.DoorCardinal.East)   {   currRoomRoom.SetEastExit(true);     }
        // if(doorCardinal == DoorTrigger.DoorCardinal.South)  {   currRoomRoom.SetSouthExit(true);    }
        // if(doorCardinal == DoorTrigger.DoorCardinal.West)   {   currRoomRoom.SetWestExit(true);     }
    }*/

    public void RegisterMonster() {
        currRoomRoom.RegisterMonster();
    }

    public void MonsterDead() {
        currRoomRoom.MonsterDead();
    }

    public void PlayerDead() {
        SceneManager.LoadScene("MainMenu");
    }

    public void WinGame() {
        SceneManager.LoadScene("WinScreen");
    }

    /*public void SetAtDoor(DoorTrigger.DoorCardinal doorCardinal, bool isAtDoor) {
        atDoorCardinal = doorCardinal;
        playerAtDoor = isAtDoor;
    }*/

    public void SetAtDoor(int doorCardinal, bool isAtDoor) {
        atDoor = (doorCardinal + 4 - rotations) % 4;
        playerAtDoor = isAtDoor;
    }

    public void ExitRoom() {
        if(!playerAtDoor || exiting) {
            return;
        }
        exiting = true;
        EnterRoom(rooms[atDoor], atDoor);
        /*switch(atDoor) {
            case 0:
                //EnterRoom(northRoom, atDoorCardinal);
                EnterRoom(rooms[0], 0);
                break;
            case 1:
                //EnterRoom(eastRoom, atDoorCardinal);
                EnterRoom(rooms[1], 1);
                break;
            case 2:
                //EnterRoom(southRoom, atDoorCardinal);
                EnterRoom(rooms[2], 2);
                break;
            case 3:
                //EnterRoom(westRoom, atDoorCardinal);
                EnterRoom(rooms[3], 3);
                break;
        }*/
        /*switch(atDoorCardinal) {
            case DoorTrigger.DoorCardinal.North:
                //EnterRoom(northRoom, atDoorCardinal);
                EnterRoom(rooms[0], atDoorCardinal);
                break;
            case DoorTrigger.DoorCardinal.East:
                //EnterRoom(eastRoom, atDoorCardinal);
                EnterRoom(rooms[1], atDoorCardinal);
                break;
            case DoorTrigger.DoorCardinal.South:
                //EnterRoom(southRoom, atDoorCardinal);
                EnterRoom(rooms[2], atDoorCardinal);
                break;
            case DoorTrigger.DoorCardinal.West:
                //EnterRoom(westRoom, atDoorCardinal);
                EnterRoom(rooms[3], atDoorCardinal);
                break;
        }*/
        exiting = false;
    }

    /*public void EnterRoom(GameObject room, DoorTrigger.DoorCardinal doorCardinal) {
        Destroy(currRoom);
        rotations = 0;
        Vector3 playerPos = Vector3.zero;
        currRoomRoom = room.GetComponent<Room>();
        switch(doorCardinal) {
            case DoorTrigger.DoorCardinal.North:
                playerPos = new Vector3(0,-3.5f,0);
                if(!currRoomRoom.GetSouthExit()) {
                    if(currRoomRoom.GetWestExit()) {
                        rotations = 1;
                    } else if(currRoomRoom.GetNorthExit()) {
                        rotations = 2;
                    } else if(currRoomRoom.GetEastExit()) {
                        rotations = 3;
                    }
                }
                break;
            case DoorTrigger.DoorCardinal.East:
                playerPos = new Vector3(-3.5f,0,0);
                if(!currRoomRoom.GetWestExit()) {
                    if(currRoomRoom.GetNorthExit()) {
                        rotations = 1;
                    } else if(currRoomRoom.GetEastExit()) {
                        rotations = 2;
                    } else if(currRoomRoom.GetSouthExit()) {
                        rotations = 3;
                    }
                }
                break;
            case DoorTrigger.DoorCardinal.South:
                playerPos = new Vector3(0,3.5f,0);
                if(!currRoomRoom.GetNorthExit()) {
                    if(currRoomRoom.GetEastExit()) {
                        rotations = 1;
                    } else if(currRoomRoom.GetSouthExit()) {
                        rotations = 2;
                    } else if(currRoomRoom.GetWestExit()) {
                        rotations = 3;
                    }
                }
                break;
            case DoorTrigger.DoorCardinal.West:
                playerPos = new Vector3(3.5f,0,0);
                if(!currRoomRoom.GetEastExit()) {
                    if(currRoomRoom.GetSouthExit()) {
                        rotations = 1;
                    } else if(currRoomRoom.GetWestExit()) {
                        rotations = 2;
                    } else if(currRoomRoom.GetNorthExit()) {
                        rotations = 3;
                    }
                }
                break;
        }
        pc.transform.position = playerPos;
        currRoom = Instantiate(room, Vector3.zero, Quaternion.Euler(0,0,90*rotations));
        RollRoom();
    }*/

    /*public void EnterRoom(GameObject room, int doorCardinal) {
        Destroy(currRoom);
        rotations = 0;
        Vector3 playerPos = Vector3.zero;
        currRoomRoom = room.GetComponent<Room>();
        switch(doorCardinal) {
            case 0:
                playerPos = new Vector3(0,-3.5f,0);
                if(!currRoomRoom.GetSouthExit()) {
                    if(currRoomRoom.GetWestExit()) {
                        rotations = 1;
                    } else if(currRoomRoom.GetNorthExit()) {
                        rotations = 2;
                    } else if(currRoomRoom.GetEastExit()) {
                        rotations = 3;
                    }
                }
                break;
            case 1:
                playerPos = new Vector3(-3.5f,0,0);
                if(!currRoomRoom.GetWestExit()) {
                    if(currRoomRoom.GetNorthExit()) {
                        rotations = 1;
                    } else if(currRoomRoom.GetEastExit()) {
                        rotations = 2;
                    } else if(currRoomRoom.GetSouthExit()) {
                        rotations = 3;
                    }
                }
                break;
            case 2:
                playerPos = new Vector3(0,3.5f,0);
                if(!currRoomRoom.GetNorthExit()) {
                    if(currRoomRoom.GetEastExit()) {
                        rotations = 1;
                    } else if(currRoomRoom.GetSouthExit()) {
                        rotations = 2;
                    } else if(currRoomRoom.GetWestExit()) {
                        rotations = 3;
                    }
                }
                break;
            case 3:
                playerPos = new Vector3(3.5f,0,0);
                if(!currRoomRoom.GetEastExit()) {
                    if(currRoomRoom.GetSouthExit()) {
                        rotations = 1;
                    } else if(currRoomRoom.GetWestExit()) {
                        rotations = 2;
                    } else if(currRoomRoom.GetNorthExit()) {
                        rotations = 3;
                    }
                }
                break;
        }
        pc.transform.position = playerPos;
        currRoom = Instantiate(room, Vector3.zero, Quaternion.Euler(0,0,90*rotations));
        RollRoom();
    }*/

    public void EnterRoom(GameObject room, int doorCardinal) {
        if(pc.GetInventory().GetCoins() >= 10) {
            WinGame();
            return;
        }
        Destroy(currRoom);
        rotations = 0;
        Vector3 playerPos = Vector3.zero;
        currRoomRoom = room.GetComponent<Room>();
        float x = (doorCardinal == 1) ? -3.5f : (doorCardinal == 3) ? 3.5f : 0;
        float y = (doorCardinal == 0) ? -3.5f : (doorCardinal == 2) ? 3.5f : 0;
        playerPos = new Vector3(x,y,0);
        int i;
        for(i = 0; i < 4; i++) {
            if(currRoomRoom.GetExit((doorCardinal + 2 + i) % 4)) {
                rotations = i;
                break;
            }
        }
        /*if(!currRoomRoom.GetExit((doorCardinal + 2) % 4)) {
            if(currRoomRoom.GetExit((doorCardinal + 3) % 4)) {
                rotations = 1;
            } else if(currRoomRoom.GetExit((doorCardinal + 4) % 4)) {
                rotations = 2;
            } else if(currRoomRoom.GetExit((doorCardinal + 5) % 4)) {
                rotations = 3;
            }
        }*/
        pc.transform.position = playerPos;
        currRoom = Instantiate(room, Vector3.zero, Quaternion.Euler(0,0,90*rotations));
        currRoomRoom = currRoom.GetComponent<Room>();
        RollRoom();
    }

    public void GrantCoins (int coins) {
        pc.GetInventory().AddCoins(coins);
    }

    public Character GetPC() {
        return pc;
    }

    public GameObject GetCurrRoom() {
        return currRoom;
    }

    // public void LeaveRoom() {
        
    // }
}
