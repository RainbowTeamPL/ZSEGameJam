using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public void GuzikMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    // Use this for initialization
    public void GuzikStart()
    {
        SceneManager.LoadSceneAsync("OverWorld01");
    }

    public void GuzikSterowanie()
    {
        SceneManager.LoadSceneAsync("Sterowanie");
    }
}