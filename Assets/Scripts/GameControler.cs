using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    private Collecter playerCollecter;
    public int TotalCollectedItems = 6;
    public Text scoreText;
    public List<Image> InfectionImages;

    public string LoseSceneName = "LoseScene";
    public string WinSceneName = "WinScene";
    private string scoreFormat = "{0}/{1}";
    private Infection playerInfection = null;
    
    void Start()
    {
        playerCollecter = player.GetComponent<Collecter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            SceneManager.LoadScene(LoseSceneName, LoadSceneMode.Single);
        }
        else if (playerCollecter.CurrentCollected == TotalCollectedItems)
        {
            SceneManager.LoadScene(WinSceneName, LoadSceneMode.Single);
        }

        UpdateText(string.Format(scoreFormat, playerCollecter.CurrentCollected, TotalCollectedItems));

        if(playerInfection == null)
        {
            playerInfection = player.GetComponent<Infection>();
        }
        else
        {
            UpdateInfectionMarkers();
        }
    }

    public void UpdateText(string textValue)
    {
        scoreText.text = textValue; 
    }

    public  void UpdateInfectionMarkers()
    {
        int step = (int)playerInfection.DeathTreshHold / 10 -1;
        int imagesNumbers = (int)playerInfection.CurrentLevel / step;

        for (int i = 0; i < imagesNumbers; i++)
        {
            InfectionImages[i].gameObject.SetActive(true);
        }

    }
}
