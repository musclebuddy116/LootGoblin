using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TreasureChest : MonoBehaviour
{
    [SerializeField] bool atChest = false;
    [SerializeField] int minAwardCoins = 3;
    [SerializeField] int maxAwardCoins = 10;
    [SerializeField] AudioSource audioSource;
    bool opened = false;

    // Start is called before the first frame update
    void Start()
    {
        DungeonManager.singleton.RegisterChest(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) {
            atChest = true;
            DungeonManager.singleton.SetAtChest(this, atChest);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player")) {
            atChest = false;
            DungeonManager.singleton.SetAtChest(this, atChest);
        }
    }

    /// <summary>
    /// Opens the treasure chest, awarding some loot. Will always provide some coinage, along with a random item
    /// </summary>
    /// <param name="character"> The Character opening the chest, to give the loot to</param>
    public void Open(Character character) {
        if(opened) { return; }
        opened = true;
        audioSource.Play();
        character.GetInventory().AddCoins(Random.Range(minAwardCoins, maxAwardCoins+1));
        Item item = ItemManager.singleton.GetRandomItem();
        character.GetInventory().AddItem(item);
        Destroy(this.gameObject, .4f);
    }
}
