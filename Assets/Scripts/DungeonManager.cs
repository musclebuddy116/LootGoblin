using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager singleton;

    [SerializeField] Character playerCharacter;
    [Header("Visual")]
    [SerializeField] ScreenFader dungeonScreenFader;
    [SerializeField] Volume volume;
    
    [Header("Rooms")]
    [SerializeField] List<GameObject> roomPrefabs;
    [SerializeField] GameObject currRoom;
    Room currRoomRoom;
    [SerializeField] GameObject[] rooms = new GameObject[4]; //NESW, adjusted for room rotations
    int rotations = 0;
    bool exiting = false;
    
    [Header("Doors")]
    [SerializeField] int atDoor;
    bool playerAtDoor;
    
    [Header("Chests")]
    [SerializeField] TreasureChest atChest;
    bool playerAtChest;

    

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
        playerCharacter.GetInventory().LoadItems(Strings.playerItemsString);
        playerCharacter.GetInventory().LoadCoins(Strings.playerCoinsString);
        EnterRoom(roomPrefabs[0], 0);
        int arcadeModeToggleOption = PlayerPrefs.GetInt("ArcadeMode", 0);
        if(arcadeModeToggleOption == 1) {
            volume.weight = 1;
        } else {
            volume.weight = 0;
        }
        // playerCharacter.transform.position = Vector3.zero;
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

    public void RegisterChest(TreasureChest chest) {
        currRoomRoom.RegisterChest(chest);
    }

    public void MonsterDead() {
        currRoomRoom.MonsterDead();
    }

    public void PlayerDead() {
        playerCharacter.GetInventory().SaveItemsEmpty(Strings.playerItemsString);
        playerCharacter.GetInventory().SaveCoins(Strings.playerCoinsString);
        SceneManager.LoadScene(Strings.mainMenuScene);
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

    public void SetAtChest(TreasureChest chest, bool isAtChest) {
        atChest = chest;
        playerAtChest = isAtChest;
    }

    public void Interact() {
        if(exiting) {
            return;
        }
        if(playerAtDoor) { ExitRoom(); }
        else if(playerAtChest) { OpenChest(atChest); }
    }
    
    public void ExitRoom() {
        if(!playerAtDoor || exiting) {
            return;
        }
        exiting = true;
        currRoomRoom.ClearChests();
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

    void OpenChest(TreasureChest chest) {
        chest.Open(playerCharacter);
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
        playerCharacter.transform.position = playerPos;
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
        playerCharacter.transform.position = playerPos;
        currRoom = Instantiate(room, Vector3.zero, Quaternion.Euler(0,0,90*rotations));
        RollRoom();
    }*/

    public void EnterRoom(GameObject room, int doorCardinal) {
        if(playerCharacter.GetInventory().GetCoins() >= 100) {
            WinGame();
            return;
        }
        Destroy(currRoom);
        rotations = 0;
        currRoomRoom = room.GetComponent<Room>();
        float x = (doorCardinal == 1) ? -8.5f : (doorCardinal == 3) ? 8.5f : 0;
        float y = (doorCardinal == 0) ? -8.5f : (doorCardinal == 2) ? 8.5f : 0;
        Vector3 playerPos = new Vector3(x,y,0);
        int rSign = Random.Range(0,2);
        if(rSign == 0) { rSign = -1; }
        int i;
        for(i = 0; i < 4; i++) {
            if(currRoomRoom.GetExit((doorCardinal + 2 + 4 + (i * rSign)) % 4)) {
                rotations = (4 + (i * rSign)) % 4;
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
        playerCharacter.transform.position = playerPos;
        currRoom = Instantiate(room, Vector3.zero, Quaternion.Euler(0,0,90*rotations));
        currRoomRoom = currRoom.GetComponent<Room>();
        RollRoom();
    }

    public void GrantCoins (int coins) {
        playerCharacter.GetInventory().AddCoins(coins);
    }

    public Character GetPC() {
        return playerCharacter;
    }

    public GameObject GetCurrRoom() {
        return currRoom;
    }

    // public void LeaveRoom() {
        
    // }
}
