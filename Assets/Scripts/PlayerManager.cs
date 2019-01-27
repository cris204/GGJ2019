using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region variables
    public PlayerScriptableObject player;
    private SpriteRenderer sR;
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
    private Animator anim;
    private AudioManager audioManager;
    [SerializeField]
    private Color colorDamage;
    [SerializeField]
    private Color colorNormal;

    #endregion
    #region unity functions

    private void OnEnable()
    {
        canShoot=true;
        player.attack = 10;
        player.movementSpeed = 100;
    }

    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        idPlayer = player.idPlayer;
        canShoot=true;
        inputDirection =new Vector2(0, 1);
        speed = player.movementSpeed;
        audioManager = GetComponent<AudioManager>();
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
        if (rb.velocity.x < 0)
        {
            sR.flipX = true;
        }
        else if(rb.velocity.x>0)
        {
            sR.flipX = false;
        }
        anim.SetFloat("SpeedY",rb.velocity.y);
        anim.SetFloat("SpeedX",rb.velocity.x);
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
        oldAim*/ aim.transform.position = startPosition + inputDirection/1.5f;
        //aim.transform.position = oldAim;
    }

    private void Attack()
    {
        anim.SetBool("Shoot", true);
        

        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.GetComponent<BulletLifeTime>().whoShoot = player;
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity =(aim.transform.position-transform.position).normalized*bulletSpeed*Time.deltaTime;
        audioManager.SetPlayAudio(4);
    }

    private void Death()
    {
        GameManager.Instance.RespawnPlayer(idPlayer);
        audioManager.SetPlayAudio(1);
    } 
    #endregion
    #region collisions
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bullet")&& player.name != col.GetComponent<BulletLifeTime>().whoShoot.name)
        {
            player.health -= col.GetComponent<BulletLifeTime>().whoShoot.attack;
            CanvasManager.Instance.UpdateHealtBars(player.idPlayer, col.GetComponent<BulletLifeTime>().whoShoot.attack);
            BulletPool.Instance.ReleaseBullet(col.gameObject);
            audioManager.SetPlayAudio(0);
            StartCoroutine(DamageRecived());
            if (player.health <= 0)
            {
                Death();
            }
        }
        if (col.gameObject.CompareTag("Damage")|| col.gameObject.CompareTag("Health")|| col.gameObject.CompareTag("Speed"))
        {
            
            timeWithPowerUp= col.gameObject.GetComponent<PowerUps>().DurationTime;
            StartCoroutine(PowerUpTime(col.gameObject.GetComponent<PowerUps>().Value, col.gameObject.GetComponent<PowerUps>().Type));
            col.gameObject.SetActive(false);
            audioManager.SetPlayAudio(2);
           
        }
    }
    #endregion
    #region coroutines
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.3f);
        canShoot = true;
        anim.SetBool("Shoot", false);
    }

    IEnumerator PowerUpTime(float value,string type)
    {
        switch (type)
        {
            case "Damage":
                player.attack *= value;
                break;
            case "Health":
                 player.health += -value;
                CanvasManager.Instance.UpdateHealtBars(player.idPlayer, value);
                break;
            case "Speed":
                player.movementSpeed *= value;
                speed = player.movementSpeed;
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
                speed = player.movementSpeed;
                break;
        }
        
    }

    IEnumerator DamageRecived()
    {
        sR.color = colorDamage;
        yield return new WaitForSeconds(0.1f);
        sR.color = colorNormal;
    }

    #endregion
}
