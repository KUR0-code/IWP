using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoRemaining : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    [SerializeField]
    Gun gun;
    [SerializeField]
    private GameObject weaponholder;

    string currentAmmo;
    string totalAmmo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentAmmo = weaponholder.GetComponent<WeaponSwitching>().GetWeapon().GetComponent<Gun>().currentAmmo.ToString();
        totalAmmo = weaponholder.GetComponent<WeaponSwitching>().GetWeapon().GetComponent<Gun>().totalAmmo.ToString();

        gun = weaponholder.GetComponent<WeaponSwitching>().GetWeapon().GetComponent<Gun>();
        textDisplay.text = currentAmmo + " / " + totalAmmo;
    }
}
