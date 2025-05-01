using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public void LoadLoginScene()
    {
        SceneManager.LoadScene("Login");
    }

    public void LoadRegisterScene()
    {
        SceneManager.LoadScene("Register");
    }
    
    public void LoadPlayerSelection()
    {
        SceneManager.LoadScene("PlayerSelection");
    }
    
    public void LoadStart()
    {
        SceneManager.LoadScene("StartingScene");
    }
}