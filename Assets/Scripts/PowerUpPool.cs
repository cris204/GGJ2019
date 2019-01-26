using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPool : MonoBehaviour
{
    private static PowerUpPool instance;

    public static PowerUpPool Instance
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
            PreparePowerUp();
        }
        else
            Destroy(gameObject);
    }

    private void PreparePowerUp()
    {
        obstacles = new List<GameObject>();
        for (int i = 0; i < size; i++)
            AddPowerUp();
    }

    public GameObject GetPowerUp()
    {
        if (obstacles.Count == 0)
            AddPowerUp();
        return AllocatePowerUp();
    }

    public void ReleasePowerUp(GameObject obstacle)
    {
        obstacle.gameObject.SetActive(false);
        obstacle.transform.position = initialPosition;

        obstacles.Add(obstacle);
    }

    private void AddPowerUp()
    {
        GameObject instance = Instantiate(obstaclePrefab);
		instance.transform.position=this.transform.position;
        instance.gameObject.SetActive(false);
        obstacles.Add(instance);
    }

    private GameObject AllocatePowerUp()
    {
        GameObject obstacle = obstacles[obstacles.Count - 1];
        obstacles.RemoveAt(obstacles.Count - 1);
        obstacle.gameObject.SetActive(true);
        return obstacle;
    }
}
