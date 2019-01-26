using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    [Header("Time")]
    [SerializeField]
    private float maxTime;
    private float currentTime;


    [Header("Detect Players in zone"),Space]
    [SerializeField]
    private int playersInZone;
    [SerializeField]
    private bool isAlone;



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
       
        currentTime = maxTime;
        playersInZone = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TimeElapsed();
    }

    public void TimeElapsed()
    {
        currentTime -= Time.deltaTime;
        CanvasManager.Instance.TimeElapsed(currentTime);
        if (currentTime >= maxTime)
        {
            FinishStage();
        }
    }

    private void FinishStage()
    {
        //Next stage or finish game
    }






    #region Colliders

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playersInZone++;
            if (playersInZone==1)
            {
                isAlone = true;
               // StartCoroutine(UpgradeScore(collision.GetComponent<PlayerManager>().idPlayer));
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
            if (playersInZone == 1)
            {
                isAlone = true;
               // StartCoroutine(UpgradeScore());
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

            Debug.Log("update Score");
        }
        Debug.Log("finish");
    }

    #endregion

}
