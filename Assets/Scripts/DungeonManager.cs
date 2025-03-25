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
        
    }

    public void RollRoom() {
        int i;
        for(i = 0; i < 4; i++) {
            rooms[(i + 4 - rotations) % 4] = (currRoomRoom.GetExit(i)) ? roomPrefabs[Random.Range(1, roomPrefabs.Count)] : null;
        }

    }

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
        exiting = false;
    }

    void OpenChest(TreasureChest chest) {
        chest.Open(playerCharacter);
    }

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
}
