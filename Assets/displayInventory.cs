using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class displayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public int xStart;
    public int yStart;
    public int X_SpaceBetweenitems;
    public int Column;
    public int Y_SpaceBetweenitems;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
         UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        for(int i = 0; i< inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            }

            else
            {
                var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
                itemsDisplayed.Add(inventory.Container[i], obj);
            }
        }
    }
    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (X_SpaceBetweenitems * (i % Column)), yStart + (-Y_SpaceBetweenitems * (i / Column)), 0f);
    }
}
