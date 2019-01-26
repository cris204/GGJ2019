using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
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
    }
    void FixedUpdate()
    {
        Move();
        Aim();
    }

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
        aim.transform.position = startPosition + inputDirection;
    }

    private void Attack()
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = aim.transform.position;
        bullet.GetComponent<Rigidbody2D>().velocity =(aim.transform.position-transform.position)*bulletSpeed*Time.deltaTime;
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.3f);
        canShoot = true;
    }
}
