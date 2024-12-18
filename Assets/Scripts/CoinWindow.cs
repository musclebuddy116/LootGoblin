using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinWindow : MonoBehaviour
{

    [SerializeField] PlayerInputHandler playerInputHandler;
    [SerializeField] TextMeshProUGUI coinText;

    // Update is called once per frame
    void Update()
    {
        int coinAmt;
        if(playerInputHandler == null) {
            coinAmt = ShopManager.singleton.GetInventory().GetCoins();
        } else {
            coinAmt = playerInputHandler.GetPlayerCharacter().GetInventory().GetCoins();
        }
        coinText.text = coinAmt.ToString() + " G";
    }
}
