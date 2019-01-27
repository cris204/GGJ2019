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
    #endregion
    #region unity functions
    private void OnEnable()
    {
        canShoot=true;
        player.attack = 10;

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

        aim.transform.position = startPosition + inputDirection/1.5f;
        
    }

    private void Attack()
    {
        anim.SetBool("Shoot", true);
        
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.GetComponent<BulletLifeTime>().whoShoot = player;
        bullet.transform.position = transform.position;
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
        if (col.gameObject.CompareTag("Bullet")&& player.name != col.GetComponent<BulletLifeTime>().whoShoot.name)
        {
            player.health -= col.GetComponent<BulletLifeTime>().whoShoot.attack;
            CanvasManager.Instance.UpdateHealtBars(player.idPlayer, col.GetComponent<BulletLifeTime>().whoShoot.attack);
            BulletPool.Instance.ReleaseBullet(col.gameObject);
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
    #endregion
}
