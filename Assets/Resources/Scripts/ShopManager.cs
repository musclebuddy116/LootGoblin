using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Transform canvasTransform;
    [SerializeField] GameObject itemWindowPrefab;
    [SerializeField] List<Item> items;
    
    // Start is called before the first frame update
    void Start()
    {
        int items = 1;
        int row = 0, column = 0, colSide = -1;
        int i = 0;
        for(i = 0; i < items; i++) {
            Vector3 windowPos = new Vector3((162 * colSide) + (216 * column), 108 - (324 * row), 0);
            windowPos = new Vector3(-162,0,0);
            ItemWindow itemWindow = Instantiate(itemWindowPrefab, windowPos, Quaternion.identity, canvasTransform).GetComponent<ItemWindow>();
            row++;
            column++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
