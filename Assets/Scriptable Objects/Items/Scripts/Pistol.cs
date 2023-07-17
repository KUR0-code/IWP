using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Pistol object", menuName = "Inventory System/Items/Pistol")]
public class Pistol : ItemObject
{
    public int ammoCount;
    // Start is called before the first frame update
    public void Awake()
    {
        type = ItemType.Pistol;
    }
}
