using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SkinManager : MonoBehaviour
{
    //public SpriteRenderer sr;
    public List<GameObject> skins = new List<GameObject>();
    private int selectedSkin = 0;
    public GameObject playerSkin;

    public void NextOption()
    {
        playerSkin.SetActive(false);
        selectedSkin = selectedSkin + 1;
        if (selectedSkin == skins.Count)
        {
            selectedSkin = 0;
        }
       /* sr.sprite = skins[selectedSkin];*/
       playerSkin = Instantiate(skins[selectedSkin]);
       playerSkin.SetActive(true);
    }
    
    public void BacktOption()
    {
        playerSkin.SetActive(false);
        selectedSkin = selectedSkin - 1;
        if (selectedSkin < 0)
        {
            selectedSkin = skins.Count - 1;
        }
        playerSkin = Instantiate(skins[selectedSkin]);  
        playerSkin.SetActive(true);
        //sr.sprite = skins[selectedSkin];
    }

    public void PlayGame()
    {

        PlayerPrefs.SetInt("SelectedSkinIndex", selectedSkin);

        SceneManager.LoadScene("GameplayScene");
    }
}
