using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public bool beenUsed = false;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += SceneChanger;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SceneChanger(Scene s, LoadSceneMode m)
    {
        if (beenUsed)
        {
            SceneManager.sceneLoaded -= SceneChanger;
            Destroy(this.gameObject);
        }
        if (s.name != SceneManager.GetSceneByBuildIndex(0).name)
        {
            beenUsed = true;
        }
    }

    public void switchScenes(int scene)
    {
        SceneManager.LoadScene(scene);
        Time.timeScale = 1;
    }
}
