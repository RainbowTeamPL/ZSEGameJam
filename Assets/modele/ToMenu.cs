using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenu : MonoBehaviour
{
    // Use this for initialization
    private void OnTriggerExit(Collider other)
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    // Update is called once per frame
    private void Update()
    {
    }
}