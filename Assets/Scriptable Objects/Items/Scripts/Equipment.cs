using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment object", menuName = "Inventory System/Items/Equipment")]
public class Equipment : ItemObject
{
    public float atkBonus;
    public float defenceBonus;
    // Start is called before the first frame update
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
