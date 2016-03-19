using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MarbleEngine.Loading
{
    /// <summary>
    /// Skrypt używany podczas ładowania pomiędzy Menu głównym a światem gry. 
    /// </summary>
    public class MenuToOverWorld01 : MonoBehaviour
    {
        public Color full = new Vector4(1, 1, 1, 0);
        public bool load = false;
        public Color notfull = new Vector4(1, 1, 1, 1);

        [SerializeField]
        public string scene;

        public float timer /*{ get; set; }*/ = 1.2f;
        private bool loadScene = false;

        public void Full2NotFull()
        {
            timer = timer - Time.deltaTime;

            if (timer >= 0)
            {
                if (scene != "-1")
                {
                    this.GetComponent<Image>().material.color = Color.Lerp(notfull, full, timer);
                }

                if (scene == "-1")
                {
                    this.GetComponent<Image>().material.color = Color.Lerp(full, notfull, timer);
                }
            }

            if (timer <= 0)
            {
                timer = 0;
                //GameObject.Find("First Person Controller").GetComponent(FPSInputController).enabled = true;
                //GameObject.Find("First Person Controller").GetComponent(MouseLook).enabled = true;
                //GameObject.Find("Main Camera").GetComponent(MouseLook).enabled = true;
                //Destroy(gameObject);
            }
        }

        public void NotFull2Full()
        {
            timer = timer - Time.deltaTime;

            if (timer >= 0)
            {
                if (scene != "-1")
                {
                    this.GetComponent<Image>().material.color = Color.Lerp(full, notfull, timer);
                }
                if (scene == "-1")
                {
                    this.GetComponent<Image>().material.color = Color.Lerp(notfull, full, timer);
                }
            }

            if (timer <= 0)
            {
                timer = 0;
                //GameObject.Find("First Person Controller").GetComponent(FPSInputController).enabled = true;
                //GameObject.Find("First Person Controller").GetComponent(MouseLook).enabled = true;
                //GameObject.Find("Main Camera").GetComponent(MouseLook).enabled = true;
                //Destroy(gameObject);
            }
        }

        public void Start()
        {
            this.GetComponent<Image>().material.color = notfull;
            load = true;

            //GameObject.Find("First Person Controller").GetComponent(FPSInputController).enabled = false;
            //GameObject.Find("First Person Controller").GetComponent(MouseLook).enabled = false;
            //GameObject.Find("Main Camera").GetComponent(MouseLook).enabled = false;
        }

        public void Update()
        {
            if (load && loadScene == false)
            {
                Full2NotFull();

                // ...set the loadScene boolean to true to prevent loading a new scene more than once...
                loadScene = true;
                // yield return new WaitForSeconds(3);
                StartCoroutine(LoadNewScene());
            }
        }

        private IEnumerator LoadNewScene()
        {
            // This line waits for 3 seconds before executing the next line in the coroutine. This
            // line is only necessary for this demo. The scenes are so simple that they load too fast
            // to read the "Loading..." text.
            yield return new WaitForSeconds(3);

            // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
            AsyncOperation async = SceneManager.LoadSceneAsync(scene);

            // While the asynchronous operation to load the new scene is not yet complete, continue
            // waiting until it's done.
            while (!async.isDone)
            {
                yield return null;
            }
        }
    }
}