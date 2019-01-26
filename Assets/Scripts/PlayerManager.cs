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
    [SerializeField]
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
#endregion
    #region unity functions
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        idPlayer = player.idPlayer;
        canShoot=true;
    }

    void Update()
    {
        if(Input.GetAxis(fire1)>0.5f && canShoot)
        {
            Attack();
            canShoot=false;
            StartCoroutine(ShootDelay());
        }

        if (player.health <= 0)
        {
            Death();
        }
    }
    void FixedUpdate()
    {
        Move();
        Aim();
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
        Vector2 inputDirection = Vector2.zero;
        inputDirection.x = Input.GetAxis(aimHorizontal);
        inputDirection.y = Input.GetAxis(aimVertical);
        startPosition=transform.position;
        Debug.Log(inputDirection);
        if(inputDirection.x>-0.5f && inputDirection.x < 0.5f)
        {
            if(inputDirection.y>-0.5f && inputDirection.y < 0.5f)
            {
            inputDirection.y=1;
            }
            inputDirection.x=0;
        }
        aim.transform.position = startPosition + inputDirection;
    }

    private void Attack()
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = aim.transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity =(aim.transform.position-transform.position)*bulletSpeed*Time.deltaTime;
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
            player.health -= 10;
            BulletPool.Instance.ReleaseBullet(col.gameObject);

        }
    }
    #endregion
    #region coroutines
    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.3f);
        canShoot = true;
    }
    #endregion
}
