using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class notka : MonoBehaviour
{
    public string notatka;
    public GameObject text;

    public void OnTriggerExit(Collider other)
    {
        text.GetComponent<Text>().text = "";
    }

    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        text.GetComponent<Text>().text = notatka;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}