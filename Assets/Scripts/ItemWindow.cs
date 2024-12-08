using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] Item item;
    [SerializeField] GameObject imagePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Purchase() {
        if(ShopManager.singleton.SellItem(item) != -1) {
            item.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
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
        Vector3 pos = transform.position + new Vector3(0,30/108f,0);
        this.item = Instantiate(item, pos, Quaternion.identity).GetComponent<Item>();
        this.item.SetId(itemID);
        this.item.gameObject.SetActive(false);
        SetInformation();
    }

    public void SetItem(int itemID) {
        // this.item = item;
        Vector3 pos = transform.position + new Vector3(0,30/108f,0);
        this.item = Instantiate(ItemManager.singleton.GetItem(itemID), pos, Quaternion.identity).GetComponent<Item>();
        this.item.SetId(itemID);
        this.item.gameObject.SetActive(false);
        SetInformation();
    }

    public void SetItem(Item item) {
        Vector3 pos = transform.position + new Vector3(0,30/108f,0);
        this.item = item;
        this.item.gameObject.SetActive(false);
        SetInformation();
    }

    public void SetInformation() {
        Image image = Instantiate(imagePrefab, transform.position + new Vector3(0,30/108f,0), Quaternion.identity, this.transform).GetComponent<Image>();
        image.sprite = item.GetComponent<SpriteRenderer>().sprite;
        image.gameObject.SetActive(true);
        SetItemText(item.GetName(), item.GetValue());
    }

    /*public void SetImage(Sprite image) {
        this.image.GetComponent<Image>().sprite = image;
    }*/

    public void SetItemText(String itemName, float value) {
        itemText.SetText(itemName + "\n" + value.ToString() + " G");
    }
}
