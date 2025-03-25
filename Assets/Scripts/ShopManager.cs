using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public static ShopManager singleton;

    [SerializeField] int numItems = 12;
    [SerializeField] int minItems = 6;
    [SerializeField] int maxItems = 16;
    
    [SerializeField] Transform canvasTransform;
    [SerializeField] GameObject itemWindowPrefab;
    [SerializeField] Inventory playerInventory;
    [SerializeField] ScreenFader shopScreenFader;

    [SerializeField] Transform shopInventoryTransform;
    [SerializeField] List<ItemWindow> itemWindows;
    
    void Awake()
    {
        if(singleton == null) {
            singleton = this;
        } else {
            Destroy(this.gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        numItems = Random.Range(minItems, maxItems+1);
        playerInventory.LoadItems(Strings.playerItemsString);
        playerInventory.LoadCoins(Strings.playerCoinsString);
        itemWindows = new List<ItemWindow>();
        List<Item> items = new List<Item>();
        int i;
        for(i = 0; i < numItems; i++) {
            ItemWindow itemWindow = Instantiate(itemWindowPrefab, shopInventoryTransform).GetComponent<ItemWindow>();
            itemWindows.Add(itemWindow);
            
            int itemID = Random.Range(0, ItemManager.singleton.GetCount());
            itemWindow.SetItem(itemID);
            
        }
        
    }

    public int SellItem(Item item) {
        return playerInventory.BuyItem(item);
    }

    public void ExitShop() {
        playerInventory.SaveItems(Strings.playerItemsString);
        playerInventory.SaveCoins(Strings.playerCoinsString);
        shopScreenFader.FadeToColor();
        StartCoroutine(DelayLeaveMenuAfterFade());
    }

    IEnumerator DelayLeaveMenuAfterFade() {
        yield return new WaitUntil(()=>shopScreenFader.DoneFadingToColor());
        SceneManager.LoadScene(Strings.dungeonScene);
    }

    public Inventory GetInventory() {
        return playerInventory;
    }

}
