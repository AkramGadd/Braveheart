using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectedSkinPlay : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public static GameObject selectedPlayerPrefab;


    // Start is called before the first frame update
    void Start()
    {
        int selectedSkin = PlayerPrefs.GetInt("SelectedSkinIndex", 0);
        if(selectedSkin == 0)
        {
            selectedPlayerPrefab = player1;
            player1.SetActive(true);
            player2.SetActive(false);
        }
        else
        {
            selectedPlayerPrefab = player2;
            player1.SetActive(false); player2.SetActive(true);
        }
        selectedPlayerPrefab.name = "Player";
        DontDestroyOnLoad(selectedPlayerPrefab);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
