using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour
{
    public void QuitToMainMenu()
    {
        // DontDestroyOnLoad(Player);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
