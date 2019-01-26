using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }
    

    [SerializeField]
    private PlayerScriptableObject[] players;

    [Header("Time")]
    [SerializeField]
    private float maxTime;
    private float currentTime;
    [SerializeField]
    private float timeToStart;
    [SerializeField]
    private bool startGame=false;


    [Header("Detect Players in zone"),Space]
    [SerializeField]
    private int playersInZone;
    [SerializeField]
    private bool isAlone;
    [SerializeField]
    private List<int> playersIdInZone=new List<int>();

    [Header("Respawn")]
    [SerializeField]
    private GameObject[] initialPos;


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
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].score = 0;
            CanvasManager.Instance.UpdateScore(i, 0);
        }
        StartCoroutine(ReadyToStart());
        currentTime = maxTime;
        CanvasManager.Instance.TimeElapsed(currentTime);
        playersInZone = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TimeElapsed();
    }

    public void TimeElapsed()
    {
        if (StartGame)
        {
            currentTime -= Time.deltaTime;
            CanvasManager.Instance.TimeElapsed(currentTime);
            if (currentTime >= maxTime)
            {
                FinishStage(true);
            }
        }
    }

    private void FinishStage(bool continueToNextStage, string stageName = "")
    {
        if (continueToNextStage)
        {
            SceneManager.LoadScene(stageName);
        }
        else
        {
            SceneManager.LoadScene("");//FinishGame
        }
    }

    public PlayerScriptableObject GetPlayers(int i)
    {
        return players[i];
    }


    #region Colliders

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playersInZone++;
            playersIdInZone.Add(collision.GetComponent<PlayerManager>().player.idPlayer);
            if (playersInZone==1)
            {
                isAlone = true;
                StartCoroutine(UpgradeScore(playersIdInZone[0]));
            }
            else
            {
                isAlone = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playersInZone--;
            playersIdInZone.Remove(collision.GetComponent<PlayerManager>().player.idPlayer);
            if (playersInZone == 1)
            {
                isAlone = true;
                StartCoroutine(UpgradeScore(playersIdInZone[0]));
            }
            else
            {
                isAlone = false;
            }
        }
    }

    #endregion

    #region Coroutine
    
    IEnumerator UpgradeScore(int idPlayer)
    {
        while (isAlone)
        {
            yield return null;
            players[idPlayer].score+=Time.deltaTime;
            CanvasManager.Instance.UpdateScore(idPlayer, players[idPlayer].score);
            Debug.Log( players[idPlayer].name);
        }
    }

    IEnumerator ReadyToStart()
    {
        while (timeToStart > 0)
        {
            yield return null;
            CanvasManager.Instance.TimeToStart(timeToStart);
            timeToStart -= Time.deltaTime;

        }
        CanvasManager.Instance.TimeToStart(timeToStart);
        StartGame = true;
    }

    #endregion

    #region Get&Set
    public bool StartGame { get => startGame; set => startGame = value; }
    #endregion

}
