using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private static BulletPool instance;

    public static BulletPool Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private GameObject obstaclePrefab;
    private Vector2 initialPosition = new Vector2(0, 14.3f);
    [SerializeField]
    private int size;
    private List<GameObject> obstacles;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            PrepareBullet();
        }
        else
            Destroy(gameObject);
    }

    private void PrepareBullet()
    {
        obstacles = new List<GameObject>();
        for (int i = 0; i < size; i++)
            AddBullet();
    }

    public GameObject GetBullet()
    {
        if (obstacles.Count == 0)
            AddBullet();
        return AllocateBullet();
    }

    public void ReleaseBullet(GameObject obstacle)
    {
        obstacle.gameObject.SetActive(false);
        obstacle.transform.position = initialPosition;

        obstacles.Add(obstacle);
    }

    private void AddBullet()
    {
        GameObject instance = Instantiate(obstaclePrefab);
		instance.transform.position=this.transform.position;
        instance.gameObject.SetActive(false);
        obstacles.Add(instance);
    }

    private GameObject AllocateBullet()
    {
        GameObject obstacle = obstacles[obstacles.Count - 1];
        obstacles.RemoveAt(obstacles.Count - 1);
        obstacle.gameObject.SetActive(true);
        return obstacle;
    }
}
