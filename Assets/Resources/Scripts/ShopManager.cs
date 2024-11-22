using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public static ShopManager singleton;

    [SerializeField] Transform canvasTransform;
    [SerializeField] GameObject itemWindowPrefab;
    [SerializeField] Inventory playerInventory;
    
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
        int numItems = 12;
        int row = 0, column = 0, colSide = -1;
        int i = 0;
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
        }
    }

    public void SellItem(Item item) {
        playerInventory.BuyItem(item);
    }

    public void ExitShop() {
        playerInventory.SaveInventory("playerInventory");
        SceneManager.LoadScene("Dungeon");
    }

}
