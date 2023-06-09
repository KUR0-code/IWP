using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();

    public void AddItem(ItemObject _item, int _amount)
    {
        bool HasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                HasItem = true;
                break;
            }
        }
        if(!HasItem)
        {
            Container.Add(new InventorySlot(_item, _amount));
        }
    }
    public bool RemoveHeal()
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.type == ItemType.Food && Container[i].amount >= 1)
            {
                Container[i].amount -= 1;
                if (Container[i].amount <= 0)
                {
                    Container[i].amount = 0;
                }
                return true;
            }
            else
            {
            }
        }
        return false;
    }
      
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}
