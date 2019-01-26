using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }
    
    [Header("Players")]
    [SerializeField]
    private PlayerScriptableObject[] playersScriptableObj;
    [SerializeField]
    private GameObject[] players;
    private SpriteRenderer[] playerSprite=new SpriteRenderer[4];
    [SerializeField]
    private SpriteRenderer[] aimPlayerSprite = new SpriteRenderer[4];

    [Header("Items")]
    [SerializeField]
    private PowerUpsScriptableObject[] powerUpsScriptableObj;


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
    private Transform[] initialPos;
    [SerializeField]
    private float timeToRespawn;
    private WaitForSeconds waitForSecondsToRespawn;
    [SerializeField]
    private Color deathAlpha;
    [SerializeField]
    private Color aliveAlpha;
    private float lerpAlpha;
    private bool isDeath;

    [Header("Finish")]
    private float scoreMax;
    private int winner;

    private void Awake()
    {
        Time.timeScale = 1;
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
        for (int i = 0; i < playersScriptableObj.Length; i++)
        {
            playersScriptableObj[i].score = 0;
            playersScriptableObj[i].health = 100;
            CanvasManager.Instance.UpdateScore(i, 0);
            playerSprite[i] = players[i].GetComponent<SpriteRenderer>();
            aimPlayerSprite[i] = players[i].transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        SpawnPlayers();
        StartCoroutine(ReadyToStart());
        currentTime = maxTime;
        CanvasManager.Instance.TimeElapsed(currentTime);
        playersInZone = 0;
        waitForSecondsToRespawn = new WaitForSeconds(timeToRespawn);
    }

    // Update is called once per frame
    void Update()
    {
        TimeElapsed();
    }

    public void SpawnPlayers()
    {
        for (int i = 0; i < initialPos.Length; i++)
        {
            players[i].transform.localPosition = initialPos[i].localPosition;
        }
    }

    public void TimeElapsed()
    {
        if (StartGame)
        {
            currentTime -= Time.deltaTime;
            CanvasManager.Instance.TimeElapsed(currentTime);
            if (currentTime <= 0)
            {
                FinishStage();
            }
        }
    }

    private void FinishStage()
    {
        for (int i = 0; i < playersScriptableObj.Length; i++)
        {
            if (scoreMax <= playersScriptableObj[i].score)
            {
                scoreMax = playersScriptableObj[i].score;
                winner = i;
            }
        }
        CanvasManager.Instance.FinishGame(playersScriptableObj[winner]);
        StartCoroutine(RestartGame());
    }

    public PlayerScriptableObject GetPlayers(int i)
    {
        return playersScriptableObj[i];
    }

    public void RespawnPlayer(int idPlayer)
    {
        players[idPlayer].SetActive(false);
        playerSprite[idPlayer].color = deathAlpha;
        aimPlayerSprite[idPlayer].color = deathAlpha;
        StartCoroutine(Respawn(idPlayer));
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
            if (startGame)
            {
                playersScriptableObj[idPlayer].score += Time.deltaTime;
                CanvasManager.Instance.UpdateScore(idPlayer, playersScriptableObj[idPlayer].score);
                Debug.Log(playersScriptableObj[idPlayer].name);
            }
        }
    }

    IEnumerator RestartGame()
    {
        StartGame = false;
        yield return new WaitForSeconds(5f);
          
        SceneManager.LoadScene("TestUI");
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

    IEnumerator Respawn(int idPlayer)
    {
        yield return waitForSecondsToRespawn;
        playersScriptableObj[idPlayer].health = 100;
        players[idPlayer].transform.localPosition = initialPos[idPlayer].localPosition;
        players[idPlayer].SetActive(true);

        
        while (playerSprite[idPlayer].color.a <= 1)
        {
            yield return null;
            lerpAlpha += Time.deltaTime*2;
            aimPlayerSprite[idPlayer].color= Color.Lerp(deathAlpha, aliveAlpha, lerpAlpha);
            playerSprite[idPlayer].color = Color.Lerp(deathAlpha, aliveAlpha, lerpAlpha);
        }

        lerpAlpha = 0;
    }

    #endregion

    #region Get&Set
    public bool StartGame { get => startGame; set => startGame = value; }
    #endregion

}
