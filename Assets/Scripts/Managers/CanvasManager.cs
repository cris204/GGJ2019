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

    public void AssignPlayerSprite(int idPlayer,Sprite playerSprite)
    {
        playersSprites[idPlayer].sprite = playerSprite;
    }

    public void UpdateHealtBars(int idPlayer, int damage)
    {
        playersHealthBars[idPlayer].fillAmount = -damage / 100;
    }

    public void TimeElapsed(float time)
    {
        timeElapsed.text = time.ToString("F0");
    }

}
