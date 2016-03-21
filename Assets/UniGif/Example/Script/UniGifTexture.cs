/*
UniGif
Copyright (c) 2015 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Texture Animation from GIF image 
/// </summary>
public class UniGifTexture : MonoBehaviour
{
    public string gifpic;

    //public string gifpic2;

    //public string gifpic3;

    private string actgif;

    // Textures filter mode
    [SerializeField]
    private FilterMode filterMode = FilterMode.Point;

    // Decoded GIF texture list
    private List<UniGif.GifTexture> gifTexList = new List<UniGif.GifTexture>();

    // Loading flag
    private bool loading;

    // Load from url on start
    [SerializeField]
    private bool loadOnStart;

    // GIF image url (WEB or StreamingAssets path)
    [SerializeField]
    private string loadOnStartUrl;

    // Debug log flag
    [SerializeField]
    private bool outputDebugLog;

    // Animationa pause flag
    [SerializeField]
    private bool pauseAnimation;

    // private int r;

    // Rotating on loading
    [SerializeField]
    private bool rotateOnLoading;

    // Renderer of target
    [SerializeField]
    private GameObject targetRenderer;

    // Use coroutine flag to GetTextureList
    [SerializeField]
    private bool useCoroutineGetTexture;

    // Textures wrap mode
    [SerializeField]
    private TextureWrapMode wrapMode = TextureWrapMode.Clamp;

    /// <summary>
    /// This component state 
    /// </summary>
    public enum STATE
    {
        NONE,
        LOADING,
        READY,
        PLAYING,
        PAUSE,
    }

    /// <summary>
    /// Texture height (px) 
    /// </summary>
    public int height
    {
        get;
        private set;
    }

    /// <summary>
    /// Animation loop count (0 is infinite) 
    /// </summary>
    public int loopCount
    {
        get;
        private set;
    }

    /// <summary>
    /// Now state 
    /// </summary>
    public STATE state
    {
        get;
        private set;
    }

    /// <summary>
    /// Texture width (px) 
    /// </summary>
    public int width
    {
        get;
        private set;
    }

    /// <summary>
    /// Pause animation 
    /// </summary>
    public void Pause()
    {
        if (outputDebugLog && state != STATE.PLAYING)
        {
            Debug.Log("State is not PLAYING.");
            return;
        }
        pauseAnimation = true;
        state = STATE.PAUSE;
    }

    /// <summary>
    /// Play animation 
    /// </summary>
    public void Play()
    {
        if (state != STATE.READY)
        {
            Debug.LogWarning("State is not READY.");
            return;
        }
        StopAllCoroutines();
        StartCoroutine(GifLoopCoroutine());
    }

    /// <summary>
    /// Resume animation 
    /// </summary>
    public void Resume()
    {
        if (outputDebugLog && state != STATE.PAUSE)
        {
            Debug.Log("State is not PAUSE.");
            return;
        }
        pauseAnimation = false;
        state = STATE.PLAYING;
    }

    /// <summary>
    /// Set GIF texture from url 
    /// </summary>
    /// <param name="url">
    /// GIF image url (WEB or StreamingAssets path) 
    /// </param>
    /// <param name="autoPlay">
    /// Auto play after decode 
    /// </param>
    /// <returns>
    /// IEnumerator 
    /// </returns>
    public IEnumerator SetGifFromUrlCoroutine(string url, bool autoPlay = true)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("URL is nothing.");
            yield break;
        }

        loading = true;

        string path;
        if (url.StartsWith("http"))
        {
            // from WEB
            path = url;
        }
        else
        {
            // from StreamingAssets
            path = System.IO.Path.Combine("file:///" + Application.streamingAssetsPath, url);
        }

        // Load file
        using (WWW www = new WWW(path))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error) == false)
            {
                Debug.LogError("File load error.\n" + www.error);
            }
            else
            {
                gifTexList.Clear();

                // Get GIF textures
                if (useCoroutineGetTexture)
                {
                    // use coroutine (avoid lock up but more slow)
                    yield return StartCoroutine(UniGif.GetTextureListCoroutine(this, www.bytes, (gtList, loop, w, h) =>
                    {
                        gifTexList = gtList;
                        FinishedGetTextureList(loop, w, h, autoPlay);
                    }, filterMode, wrapMode, outputDebugLog));
                }
                else
                {
                    // dont use coroutine (there is a possibility of lock up)
                    int loop, w, h;
                    gifTexList = UniGif.GetTextureList(www.bytes, out loop, out w, out h, filterMode, wrapMode, outputDebugLog);
                    FinishedGetTextureList(loop, w, h, autoPlay);
                }
            }
        }
    }

    /// <summary>
    /// Stop animation 
    /// </summary>
    public void Stop()
    {
        if (outputDebugLog && state != STATE.PLAYING && state != STATE.PAUSE)
        {
            Debug.Log("State is not PLAYING and PAUSE.");
            return;
        }
        StopAllCoroutines();
        state = STATE.READY;
    }

    /// <summary>
    /// Finished UniGif.GetTextureList or UniGif.GetTextureListCoroutine 
    /// </summary>
    private void FinishedGetTextureList(int loop, int w, int h, bool autoPlay)
    {
        loading = false;
        loopCount = loop;
        width = w;
        height = h;
        state = STATE.READY;
        if (rotateOnLoading)
        {
            transform.localEulerAngles = Vector3.zero;
        }
        if (autoPlay)
        {
            // Start GIF animation
            StartCoroutine(GifLoopCoroutine());
        }
    }

    /// <summary>
    /// GIF Animation loop 
    /// </summary>
    /// <returns>
    /// IEnumerator 
    /// </returns>
    private IEnumerator GifLoopCoroutine()
    {
        if (state != STATE.READY)
        {
            Debug.LogWarning("State is not READY.");
            yield break;
        }
        if (targetRenderer == null || gifTexList == null || gifTexList.Count <= 0)
        {
            Debug.LogError("TargetRenderer or GIF texture is nothing.");
            //yield break;
        }
        // play start
        state = STATE.PLAYING;
        int nowLoopCount = 0;
        do
        {
            foreach (var gifTex in gifTexList)
            {
                // Change texture
                targetRenderer.GetComponent<Image>().sprite = Sprite.Create(gifTex.texture2d, new Rect(0, 0, gifTex.texture2d.width, gifTex.texture2d.height), new Vector2(0.5f, 0.5f));
                // Delay
                float delayedTime = Time.time + gifTex.delaySec * 0.3f;
                while (delayedTime > Time.time)
                {
                    yield return 0;
                }
                // Pause
                while (pauseAnimation)
                {
                    yield return 0;
                }
            }
            if (loopCount > 0)
            {
                nowLoopCount++;
            }
        } while (loopCount <= 0 || nowLoopCount < loopCount);
    }

    private void Start()
    {
        if (targetRenderer == null)
        {
            //targetRenderer = GetComponent<RawImage>();
        }
        if (loadOnStart)
        {
            //if (SceneManager.GetActiveScene().name == "OverWorld01")
            //{
            //    r = PlayerPrefs.GetInt("r");
            //    PlayerPrefs.DeleteKey("r");
            //}
            //else
            //{
            //    r = Mathf.FloorToInt(Random.Range(1, 3));
            //    if (r == 3)
            //    {
            //        r = 2;
            //    }
            //    PlayerPrefs.SetInt("r", r);
            //}
            //r = 3;

            //switch (r)
            //{
            //   case 1:
            actgif = gifpic;
            //        break;
            //
            //    case 2:
            //        actgif = gifpic2;
            //        break;
            //        /*case 3:
            //           actgif = gifpic3;
            //            break;*/
            //}
            //Debug.Log(actgif);
            StartCoroutine(SetGifFromUrlCoroutine("AnimTextures/" + actgif));
        }
    }

    private void Update()
    {
        if (rotateOnLoading && loading)
        {
            transform.Rotate(0f, 0f, 30f * Time.deltaTime, Space.Self);
        }
    }
}