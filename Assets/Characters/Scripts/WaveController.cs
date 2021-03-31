using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct Enemy
{
    public int id;
    public int count;
    public float delay;

    public Enemy(int id, int count, float delay)
    {
        this.id = id;
        this.count = count;
        this.delay = delay;
    }
}

public class WaveController : MonoBehaviour
{
    // Enemies
    public List<GameObject> L1Enemies = new List<GameObject>();
    public List<GameObject> L2Enemies = new List<GameObject>();
    public List<GameObject> L3Enemies = new List<GameObject>();
    public GameObject temp = null;

    public GameObject[] Spawns;
    public int numSpawns = 0;

    public int level = 0;
    public int wave = 0;
    public bool inWave = false;
    public bool canSpawn = true;
    public int spawnpoint = 0;

    public int numSpawned = 0;
    public int num = 0;

    public List<List<Enemy>> WaveList = new List<List<Enemy>>();
    public List<GameObject> AliveEnemies = new List<GameObject>();

    public float NotifTime = 2f;
    public GameObject WaveText;

    public TMP_Text waveNumber;

    // Start is called before the first frame update
    void Start()
    {
        // Get level
        level = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        // Get number of spawns
        Spawns = GameObject.FindGameObjectsWithTag("Spawner");
        numSpawns = Spawns.Length;

        // Add j waves to the wave list
        for(int j = 0; j < 15; j++)
        {
            WaveList.Add(new List<Enemy>());
        }

        // Populate Waves
    }

    // Update is called once per frame
    void Update()
    {
        // Continue wave
        if (inWave && canSpawn)
        {
            WaveCreator();
        }
        else if (!inWave && wave == WaveList.Count)
        {
            GameObject.Find("Canvas").GetComponent<Menu>().Victory();
        }
    }

    public void StartNextWave()
    {
        if (!inWave)
        {
            wave++;
            string wavenum = "" + wave;
            waveNumber.text = wavenum;
            inWave = true;
        }
    }

    public void WaveCreator()
    {
        if (num >= WaveList[wave - 1].Count)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("Enemy");
            if (temp != null)
            {
                return;
            }

            inWave = false;
            num = 0;
            StartCoroutine(WaveComplete());
            return;
        }

        canSpawn = false;

        if (WaveList[wave - 1][num].count > numSpawned)
        {
            temp = null;

            // Instantiate enemy from current levels enemy list
            if(level == 1)
            {
                temp = Instantiate(L1Enemies[WaveList[wave - 1][num].id], Spawns[spawnpoint].transform.position, Quaternion.identity);
            }
            else if(level == 2)
            {
                temp = Instantiate(L2Enemies[WaveList[wave - 1][num].id], Spawns[spawnpoint].transform.position, Quaternion.identity);
            }
            else
            {
                temp = Instantiate(L3Enemies[WaveList[wave - 1][num].id], Spawns[spawnpoint].transform.position, Quaternion.identity);
            }

            AliveEnemies.Add(temp);
            spawnpoint = SpawnCounter(spawnpoint);
            StartCoroutine(EnemyDelay(WaveList[wave - 1][num].delay));
            numSpawned++;
        }
        else
        {
            numSpawned = 0;
            num++;
            canSpawn = true;
        }
    }

    public int SpawnCounter(int n)
    {
        n++;
        if (n < numSpawns)
        {
            return n;
        }
        else
        {
            return 0;
        }
    }

    IEnumerator EnemyDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canSpawn = true;
    }

    IEnumerator WaveComplete()
    {
        WaveText.SetActive(true);
        yield return new WaitForSeconds(NotifTime);
        WaveText.SetActive(false);
    }
}
