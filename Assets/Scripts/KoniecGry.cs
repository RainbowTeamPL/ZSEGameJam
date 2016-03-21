using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KoniecGry : MonoBehaviour
{
    public float timer = 10f;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        timer = timer - Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0;
        }

        if (timer == 0)
        {
            SceneManager.LoadSceneAsync("Menu");
        }
    }
}