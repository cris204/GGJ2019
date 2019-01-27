using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstruccionesManager : MonoBehaviour
{
    public GameObject[] slides;
    public int id;

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (id == 0)
            {
                slides[0].SetActive(false);
                slides[1].SetActive(true);
                id++;
            }
            else
            {
                SceneManager.LoadScene("Game");
            }
        }
    }

}
