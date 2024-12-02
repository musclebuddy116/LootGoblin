using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemWindow : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] Item item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Purchase() {
        ShopManager.singleton.SellItem(item);
        item.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    // public void SetItem(Item item) {
    //     // this.item = item;
    //     Vector3 pos = transform.position + new Vector3(0,5/108f,0);
    //     this.item = Instantiate(item, pos, Quaternion.identity).GetComponent<Item>();
    //     this.item.gameObject.SetActive(true);
    //     SetInformation();
    // }

    public void SetItem(Item item, int itemID) {
        // this.item = item;
        Vector3 pos = transform.position + new Vector3(0,5/108f,0);
        this.item = Instantiate(item, pos, Quaternion.identity).GetComponent<Item>();
        this.item.SetId(itemID);
        this.item.gameObject.SetActive(true);
        SetInformation();
    }

    public void SetItem(int itemID) {
        // this.item = item;
        Vector3 pos = transform.position + new Vector3(0,5/108f,0);
        this.item = Instantiate(ItemManager.singleton.GetItem(itemID), pos, Quaternion.identity).GetComponent<Item>();
        this.item.SetId(itemID);
        this.item.gameObject.SetActive(true);
        SetInformation();
    }

    public void SetInformation() {
        // SetImage(item.GetComponent<SpriteRenderer>().sprite);
        SetItemText(item.GetName(), item.GetValue());
    }

    public void SetImage(Sprite image) {
        this.image.GetComponent<Image>().sprite = image;
    }

    public void SetItemText(String itemName, float value) {
        itemText.SetText(itemName + "\n" + value.ToString() + " G");
    }
}
