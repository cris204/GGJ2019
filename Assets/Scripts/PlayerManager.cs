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
    private bool canShoot;
    private Vector2 inputDirection;
    private float timeWithPowerUp;
    private Animator anim;
    private AudioManager audioManager;
    [SerializeField]
    private Color colorDamage;
    [SerializeField]
    private Color colorNormal;
    public bool isAlive;
    #endregion
    #region unity functions

    private void OnEnable()
    {
        canShoot=true;
        player.attack = 10;
        player.movementSpeed = 100;
        isAlive = true;
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
        if (GameManager.Instance.StartGame && isAlive)
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
        if (GameManager.Instance.StartGame && isAlive)
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
        inputDirection = new Vector2(Input.GetAxis(aimHorizontal), Input.GetAxis(aimVertical));

        if (inputDirection.magnitude > 0.0f)
        {
            inputDirection.Normalize();
            inputDirection *= 0.7f;
            aim.transform.localPosition = inputDirection;
            //aim.SetActive(true);
        }
        else
        {
            //aim.SetActive(false);
        }
    }

    private void Attack()
    {
        anim.SetBool("Shoot", true);
        

        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.GetComponent<BulletLifeTime>().whoShoot = player;
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity =(aim.transform.position-transform.position).normalized*bulletSpeed*Time.deltaTime;
        audioManager.SetPlayAudio(5);
    }

    private void Death()
    {
        GameManager.Instance.RespawnPlayer(idPlayer);
       
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
            
            if (player.health <= 0)
            {
                Death();
            }
            else
            {
                StartCoroutine(DamageRecived());
            }
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
        anim.SetBool("Shoot", false);
    }

    IEnumerator PowerUpTime(float value,string type)
    {
        switch (type)
        {
            case "Damage":
                player.attack *= value;
                audioManager.SetPlayAudio(2);
                break;
            case "Health":

                player.health += -value;
                CanvasManager.Instance.UpdateHealtBars(player.idPlayer, value);
                audioManager.SetPlayAudio(4);

                if (player.health > 100)
                {
                    player.health = 100;
                }

                break;
            case "Speed":
                player.movementSpeed *= value;
                speed = player.movementSpeed;
                audioManager.SetPlayAudio(3);
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
