using System.Collections;
using UnityEngine;

public class kompas : MonoBehaviour
{
    public Texture blipTex;
    public Texture compBg;
    public Transform player;

    private Rect CreateBlip()
    {
        float angDeg = player.eulerAngles.y - 90;
        float angRed = angDeg * Mathf.Deg2Rad;

        float blipX = 50 * Mathf.Cos(angRed);
        float blipY = 50 * Mathf.Sin(angRed);

        blipX += 110;
        blipY += 110;

        return new Rect(blipX, blipY, 15, 15);
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 240, 240), compBg);
        GUI.DrawTexture(CreateBlip(), blipTex);
    }
}