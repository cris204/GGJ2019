using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region variables
    public PlayerScriptableObject player;
    [SerializeField]
    private int idPlayer;
    [SerializeField]
    private string moveHorizontal;
    [SerializeField]
    private string moveVertical;
    [SerializeField]
    private string fire1;
    [SerializeField]
    private string aimHorizontal;
    [SerializeField]
    private string aimVertical;
    private float speed;
    [SerializeField]
    private float bulletSpeed;
    private Rigidbody2D rb;
    private float horizontal;
    private float vertical;
    private float aimH;
    private float aimV;
    [SerializeField]
    private GameObject aim;
    private Vector2 startPosition;
    private bool canShoot;
    private Vector2 oldAim;
    private Vector2 inputDirection;
    private float timeWithPowerUp;
    #endregion
    #region unity functions
    private void OnEnable()
    {
        canShoot=true;
        player.attack = 10;

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        idPlayer = player.idPlayer;
        canShoot=true;
        inputDirection =new Vector2(0, 1);
        speed = player.movementSpeed;
    }

    void Update()
    {
        if (GameManager.Instance.StartGame)
        {
            if (Input.GetAxis(fire1) > 0.5f && canShoot)
            {
                Attack();
                canShoot = false;
                StartCoroutine(ShootDelay());
            }

            if (player.health <= 0)
            {
                Death();
            }
        }
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.StartGame)
        {
            Move();
            Aim();
        }
    }
    #endregion
    #region functions
    private void Move()
    {
        horizontal = Input.GetAxis(moveHorizontal);
        vertical = Input.GetAxis(moveVertical);
        rb.velocity = (new Vector2(horizontal,vertical) * speed * Time.deltaTime);
    }

    private void Aim()
    {
        //Vector2 inputDirection = Vector2.zero;
        inputDirection.x = Input.GetAxis(aimHorizontal);
        inputDirection.y = Input.GetAxis(aimVertical);
        startPosition=transform.position;
        if(inputDirection.x>-0.5f && inputDirection.x < 0.5f)
        {
            if(inputDirection.y>-0.5f && inputDirection.y < 0.5f)
            {
                inputDirection.y = 1;//oldAim.y;
            }
            inputDirection.x = 0;//oldAim.x;
        }
      /*  if(inputDirection.x>0.6f)
        {
            inputDirection.x = 1f;
            if(inputDirection.y > 0.6f)
            {
                inputDirection.y = 1f;
            }
        }
        if (inputDirection.x < -0.6f)
        {
            inputDirection.x = -1f;
            if (inputDirection.y < -0.6f)
            {
                inputDirection.y = -1f;
            }
        }
        oldAim*/ aim.transform.position = startPosition + inputDirection;
        //aim.transform.position = oldAim;
    }

    private void Attack()
    {
        Debug.Log("bullet");
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.GetComponent<BulletLifeTime>().whoShoot = player;
        bullet.transform.position = aim.transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity =(aim.transform.position-transform.position).normalized*bulletSpeed*Time.deltaTime;
    }

    private void Death()
    {
        GameManager.Instance.RespawnPlayer(idPlayer);
    } 
    #endregion
    #region collisions
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            player.health -= col.GetComponent<BulletLifeTime>().whoShoot.attack;
            CanvasManager.Instance.UpdateHealtBars(player.idPlayer, col.GetComponent<BulletLifeTime>().whoShoot.attack);
            BulletPool.Instance.ReleaseBullet(col.gameObject);
        }
        if (col.gameObject.CompareTag("Damage")|| col.gameObject.CompareTag("Health")|| col.gameObject.CompareTag("Speed"))
        {
            
            timeWithPowerUp= col.gameObject.GetComponent<PowerUps>().DurationTime;
            StartCoroutine(PowerUpTime(col.gameObject.GetComponent<PowerUps>().Value, col.gameObject.GetComponent<PowerUps>().Type));
            col.gameObject.SetActive(false);
           
        }
    }
    #endregion
    #region coroutines
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.3f);
        canShoot = true;
    }

    IEnumerator PowerUpTime(int value,string type)
    {
        switch (type)
        {
            case "Damage":
                player.attack *= value;
                break;
            case "Health":
                // player.health /= value;
                break;
            case "Speed":
                player.movementSpeed *= value;
                break;
        }

        Debug.Log(type);
        yield return new WaitForSeconds(timeWithPowerUp);
        
        switch (type)
        {
            case "Damage":
                player.attack /= value;
                break;
            case "Health":
                // player.health /= value;
                break;
            case "Speed":
                player.movementSpeed /= value;
                break;
        }
        
    }
    #endregion
}
