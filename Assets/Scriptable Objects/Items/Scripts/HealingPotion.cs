using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Healing object", menuName = "Inventory System/Items/Healing")]

public class HealingPotion : ItemObject
{
    public int restoreHealthValue;
    // Start is called before the first frame update
    public void Awake()
    {
        type = ItemType.Food;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
