using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform[] weapons;

    [Header("Keys")]
    [SerializeField] private KeyCode[] keys;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedWeapon = 0;
    private float timeSinceLastSwitch;

    private void Awake()
    {
        SetWeapons();
        Select(selectedWeapon);

        timeSinceLastSwitch = 0f;
    }

    private void SetWeapons()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]) && selectedWeapon != i && timeSinceLastSwitch >= switchTime)
            {
                selectedWeapon = i;
                Select(selectedWeapon); 
            }
        }
    }
    public Transform GetWeapon()
    {
        return transform.GetChild(selectedWeapon);
    }

    public Transform GetNextWeapon()
    {
        return transform.GetChild(selectedWeapon + 1);
    }
    public Transform GetPreviousWeapon()
    {
        return transform.GetChild(selectedWeapon - 1);
    }

    private void Update()
    {
        timeSinceLastSwitch += Time.deltaTime;
        SetWeapons();
    }

    private void Select(int weaponIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (i == weaponIndex)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);

        }
        timeSinceLastSwitch = 0f;

        OnWeaponSelected();
    }

    private void OnWeaponSelected() { }
}