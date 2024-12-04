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
        
        /*int row = 0, column = 0, colSide = -1;
        for(i = 0; i < numItems; i++) {
            Vector3 windowPos = new Vector3((162 + (216 * column)) * colSide, 108 - (324 * row), 0);
            ItemWindow itemWindow = Instantiate(itemWindowPrefab, windowPos/108f, Quaternion.identity, canvasTransform).GetComponent<ItemWindow>();
            
            int itemID = Random.Range(0, ItemManager.singleton.GetCount());
            // Item item = ItemManager.singleton.GetItem(itemID);

            itemWindow.SetItem(itemID);
            
            if(colSide == 1) {
                column++;
            }
            colSide *= -1;
            if(column > 3) {
                row++;
                column = 0;
            }
        }*/
    }

    /*public void SellItem(Item item) {
        playerInventory.BuyItem(item);
    }*/

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
