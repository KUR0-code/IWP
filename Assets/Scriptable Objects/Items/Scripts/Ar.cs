using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Ar object", menuName = "Inventory System/Items/Ar")]
public class Ar : ItemObject
{
    public int ammoCount;
    // Start is called before the first frame update
    public void Awake()
    {
        type = ItemType.Ar;
    }
}
