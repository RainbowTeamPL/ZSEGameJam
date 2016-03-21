using System.Collections;
using UnityEngine;

public class TorchScript : MonoBehaviour
{
    public bool isShown = false;
    public GameObject pochodnia;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            isShown = !isShown;
            if (isShown)
            {
                pochodnia.SetActive(true);
            }
            else
            {
                pochodnia.SetActive(false);
            }
        }
    }
}