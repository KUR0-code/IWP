using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour
{
    GameObject Player;
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void QuitToMainMenu()
    {
        DontDestroyOnLoad(Player);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
