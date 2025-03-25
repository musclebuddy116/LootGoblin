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
