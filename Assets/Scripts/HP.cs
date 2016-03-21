using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HP : MonoBehaviour
{
    public float health = 100.0f;

    public void AdjustHP(int amount)
    {
        health = health - amount;
    }

    // Use this for initialization
    public void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        if (health >= 100f)
        {
            health = 100f;
        }
        if (health <= 0f)
        {
            health = 0f;
            SceneManager.LoadSceneAsync("Lamus");
        }
    }
}