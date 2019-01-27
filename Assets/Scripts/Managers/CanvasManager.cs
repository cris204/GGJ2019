using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private static CanvasManager instance;

    public static CanvasManager Instance { get => instance;}

    [SerializeField]
    private Image[] playersSprites;
    [SerializeField]
    private Image[] playersHealthBars;
    [SerializeField]
    private Text[] playerScore;
    [SerializeField]
    private Text timeElapsed;
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private Text timeToStart;
    [SerializeField]
    private Image[] deathX; 

    [Header("Finish")]
    public GameObject containerFinishGame;
    public Text winner;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        //DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < playerScore.Length; i++)
        {
            playerScore[i].text = GameManager.Instance.GetPlayers(i).score.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int idPlayer,float newScore)
    {
        playerScore[idPlayer].text = newScore.ToString("F0");
    }

    public void AssignPlayerSprite(int idPlayer,Sprite playerSprite)
    {
        playersSprites[idPlayer].sprite = playerSprite;
    }

    public void UpdateHealtBars(int idPlayer, float damage)
    {
        playersHealthBars[idPlayer].fillAmount -= damage / 100;
        if (playersHealthBars[idPlayer].fillAmount <= 0)
        {
            deathX[idPlayer].enabled = true;
        }
        else
        {
            deathX[idPlayer].enabled = false;
        }
    }

    public void TimeElapsed(float time)
    {
        timeElapsed.text = time.ToString("F0");
    }

    public void TimeToStart(float timeLeftToStart)
    {
        timeToStart.text = timeLeftToStart.ToString("F0");
        if (timeLeftToStart<=0)
        {
            container.SetActive(false);
        }
    }
    public void FinishGame(PlayerScriptableObject player)
    {
        containerFinishGame.SetActive(true);
        winner.text = string.Format("the hobbo with the new house is {0} ", player.name);
    }
}
