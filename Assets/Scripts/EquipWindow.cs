using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipWindow : MonoBehaviour
{
    [SerializeField] PlayerInputHandler playerInputHandler;
    [SerializeField] TextMeshProUGUI equipText;
    [SerializeField] Image equipImage;
    [SerializeField] Transform durabilityTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Weapon weapon = playerInputHandler.GetPlayerCharacter().GetInventory().GetCurrWeapon();
        if(weapon == null) {
            equipImage.sprite = null;
            equipText.text = "You have no weapons left!";
            durabilityTransform.localScale = new Vector3(0, durabilityTransform.localScale.y, 1);
            return;
        }
        equipImage.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        equipText.text = "Equipped weapon:\n  " + weapon.GetName() + "\nDamage: " + weapon.GetDamage().ToString("F2");
        float durability = weapon.GetDurability();
        float maxDurability = weapon.GetMaxDurability();
        float fraction = durability/maxDurability;
        durabilityTransform.GetComponent<Image>().color = Color.Lerp(Color.red,Color.green, fraction);
        durabilityTransform.localScale = new Vector3(fraction, durabilityTransform.localScale.y, 1);        
    }
}
