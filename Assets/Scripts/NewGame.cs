using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    // Use this for initialization
    public void GuzikStart()
    {
        SceneManager.LoadSceneAsync("OverWorld01Load");
    }
}