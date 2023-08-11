using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scavange : MonoBehaviour
{
    public GameObject player;
    public WeaponSway weaponSway;
    public Slider Scavange_Bar;
    public WeaponSwitching weaponHolder;
    public Bob bob;
    public TextMeshProUGUI textDisplay;
    float disappearTime;
    bool Archeck;
    bool PistolCheck;

    public GameObject ScavangeObject;

    private void Start()
    {
        textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, 0);
        disappearTime = 0;
        Archeck = false;
        PistolCheck = false; 
        Scavange_Bar.value = 5;
    }
    public void Scavanging()
    {

        // reduce the ui bar based on dt
        Scavange_Bar.value -= Time.deltaTime;

        // lock player movement and rotation
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerLook>().enabled = false;
        player.GetComponent<InputManager>().enabled = false;
        bob.enabled = false;
        weaponSway.enabled = false;  


        if (Scavange_Bar.value <= 0)
        {
            if (player.GetComponent<PlayerInteract>().Scavange_Rifle_Ammo && !weaponHolder.GetWeapon().GetComponent<Gun>().rapidFire) // Ar + pistol
            {
                Debug.Log("1");

                weaponHolder.GetPreviousWeapon().GetComponent<Gun>().totalAmmo += player.GetComponent<PlayerInteract>().RandomArNum;
                player.GetComponent<PlayerInteract>().Scavange_Rifle_Ammo = false;
                Archeck = true;
            }
            else if (player.GetComponent<PlayerInteract>().Scavange_Rifle_Ammo && weaponHolder.GetWeapon().GetComponent<Gun>().rapidFire) // Ar + Ar
            {
                Debug.Log(player.GetComponent<PlayerInteract>().Scavange_Rifle_Ammo);
                weaponHolder.GetWeapon().GetComponent<Gun>().totalAmmo += player.GetComponent<PlayerInteract>().RandomArNum;
                player.GetComponent<PlayerInteract>().Scavange_Rifle_Ammo = false;
                Archeck = true;
            }   
            else if (player.GetComponent<PlayerInteract>().Scavange_Pistol_Ammo && !weaponHolder.GetWeapon().GetComponent<Gun>().rapidFire) // Ar + pistol
            {
                Debug.Log("3");
                weaponHolder.GetPreviousWeapon().GetComponent<Gun>().totalAmmo += player.GetComponent<PlayerInteract>().RandomPistolNum;
                player.GetComponent<PlayerInteract>().Scavange_Pistol_Ammo = false;
                PistolCheck = true;
            }
            else
            {
                Debug.Log("4");
                weaponHolder.GetNextWeapon().GetComponent<Gun>().totalAmmo += player.GetComponent<PlayerInteract>().RandomPistolNum;
                player.GetComponent<PlayerInteract>().Scavange_Pistol_Ammo = false;
                PistolCheck = true;
            }
            Scavange_Bar.gameObject.SetActive(false);
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<PlayerLook>().enabled = true;
            player.GetComponent<InputManager>().enabled = true;
            bob.enabled = true;
            weaponSway.enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<PlayerInteract>().Scavange_Rifle_Ammo || player.GetComponent<PlayerInteract>().Scavange_Pistol_Ammo)
        {
            Scavanging();
        }

        Collected_Ar();
        Collected_Pistol();
    }


    void Collected_Ar()
    {
        if (Archeck)
        {
            Debug.Log("for ar");

            disappearTime += Time.deltaTime;
            if (disappearTime <= 0.1f)
            {
                textDisplay.text = "+" + player.GetComponent<PlayerInteract>().RandomArNum.ToString() + " Rifle Ammo";
                Archeck = false;
            }
            FadeOut();
        }


    }
    void Collected_Pistol()
    {
        if (PistolCheck)
        {
            Debug.Log("for pistol");
            disappearTime += Time.deltaTime;
            if (disappearTime <= 0.1f)
            {
                textDisplay.text = "+" + player.GetComponent<PlayerInteract>().RandomPistolNum.ToString() + " Pistol Ammo";
                PistolCheck = false;
            }
            FadeOut();
        }
    }
    
    public void FadeOut()
    {
        StartCoroutine(FadeOutCR());
    }


    private IEnumerator FadeOutCR()
    {
        float duration = 2.5f;
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            textDisplay.color = new Color(textDisplay.color.r, textDisplay.color.g, textDisplay.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        Destroy(ScavangeObject);
        yield break;
    }
}
