using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //bool roomComplete = false;
    [SerializeField] string roomHint;
    [SerializeField] int monsterCount = 0;
    
    [Header("Exits")]
    [SerializeField] bool northExit = false;
    [SerializeField] bool eastExit = false;
    [SerializeField] bool southExit = false;
    [SerializeField] bool westExit = false;
    [SerializeField] bool[] exits = new bool[4];

    [Header("Chests")]
    [SerializeField] List<TreasureChest> chests;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterMonster() {
        monsterCount += 1;
    }

    public void RegisterChest(TreasureChest chest) {
        chests.Add(chest);
    }

    public void ClearChests() {
        chests.Clear();
    }

    public void MonsterDead() {
        monsterCount -= 1;
    }

    /*public void SetNorthExit(bool b)    {   northExit = b;  }
    public void SetEastExit(bool b)     {   eastExit = b;   }
    public void SetSouthExit(bool b)    {   southExit = b;  }
    public void SetWestExit(bool b)     {   westExit = b;   }*/

    public bool GetNorthExit()    {   return northExit;   }
    public bool GetEastExit()     {   return eastExit;    }
    public bool GetSouthExit()    {   return southExit;   }
    public bool GetWestExit()     {   return westExit;    }
    public bool GetExit(int exit) {
        return exits[exit];
    }

    public bool IsRoomComplete() {
        //return roomComplete;
        return (monsterCount == 0);
    }

    public string Peek() {
        if(IsRoomComplete()) {
            return roomHint;
        } else {
            return "...";
        }
    }
}
