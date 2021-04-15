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
    // Weak, Weak Armored, Weak Flying, Medium Armor, Fast, Tank, Flying Tank, Boss
    public List<GameObject> L1Enemies = new List<GameObject>();
    public List<GameObject> L2Enemies = new List<GameObject>();
    public List<GameObject> L3Enemies = new List<GameObject>();
    public GameObject temp = null;

    public GameObject[] Spawns;
    int numSpawns = 0;

    public int level = 0;
    public int wave = 0;
    public bool inWave = false;
    bool canSpawn = true;
    int spawnpoint = 0;

    int numSpawned = 0;
    int num = 0;

    public List<List<Enemy>> WaveList = new List<List<Enemy>>();
    public List<GameObject> AliveEnemies = new List<GameObject>();

    public float NotifTime = 2f;
    public GameObject WaveText;

    public TMP_Text waveNumber;

    AudioSource Speaker;
    public AudioClip NextWaveSound;
    public AudioClip WaveBeginSound;
    public AudioClip WaveEndSound;

    // Start is called before the first frame update
    void Start()
    {
        Speaker = GetComponent<AudioSource>();

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
        // Weak, Weak Armored, Weak Flying, Medium Armor, Fast, Tank, Flying Tank, Boss
        WaveList[0].Add(new Enemy(0, 50, .5f));

        WaveList[1].Add(new Enemy(1, 50, .5f));

        WaveList[2].Add(new Enemy(0, 20, .5f));
        WaveList[2].Add(new Enemy(1, 20, .5f));
        WaveList[2].Add(new Enemy(0, 20, .5f));
        WaveList[2].Add(new Enemy(1, 20, .5f));

        WaveList[3].Add(new Enemy(2, 40, .5f));

        WaveList[4].Add(new Enemy(1, 10, .5f));
        WaveList[4].Add(new Enemy(2, 20, .5f));
        WaveList[4].Add(new Enemy(1, 10, .5f));
        WaveList[4].Add(new Enemy(2, 20, .5f));

        WaveList[5].Add(new Enemy(1, 10, .5f));
        WaveList[5].Add(new Enemy(2, 10, .5f));
        WaveList[5].Add(new Enemy(1, 10, .5f));
        WaveList[5].Add(new Enemy(2, 10, .5f));
        WaveList[5].Add(new Enemy(1, 20, .5f));
        WaveList[5].Add(new Enemy(2, 20, .5f));
        WaveList[5].Add(new Enemy(1, 20, .5f));
        WaveList[5].Add(new Enemy(2, 20, .5f));

        WaveList[6].Add(new Enemy(3, 20, .5f));
        WaveList[6].Add(new Enemy(2, 20, .5f));

        WaveList[7].Add(new Enemy(3, 50, .5f));

        WaveList[8].Add(new Enemy(3, 10, .5f));
        WaveList[8].Add(new Enemy(4, 10, .5f));

        WaveList[9].Add(new Enemy(4, 50, .3f));

        WaveList[10].Add(new Enemy(5, 10, 1f));
        WaveList[10].Add(new Enemy(4, 30, .3f));

        WaveList[11].Add(new Enemy(5, 30, 1f));

        WaveList[12].Add(new Enemy(6, 30, .5f));

        WaveList[13].Add(new Enemy(6, 10, .5f));
        WaveList[13].Add(new Enemy(4, 30, .3f));
        WaveList[13].Add(new Enemy(5, 10, 1f));
        WaveList[13].Add(new Enemy(2, 10, 1f));
        WaveList[13].Add(new Enemy(5, 10, 1f));
        WaveList[13].Add(new Enemy(4, 30, .3f));

        WaveList[14].Add(new Enemy(7, 1, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
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
    }

    public void StartNextWave()
    {
        if (!inWave)
        {
            Speaker.clip = NextWaveSound;
            Speaker.PlayOneShot(Speaker.clip);

            Speaker.clip = WaveBeginSound;
            Speaker.PlayOneShot(Speaker.clip);

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
            temp.GetComponent<EnemyController>().Gate = Spawns[spawnpoint].GetComponent<GateController>().ChooseGate();
            temp.GetComponent<EnemyController>().passGate = false;
            temp.GetComponent<EnemyController>().SetTargetGoal();

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
        Speaker.clip = WaveEndSound;
        Speaker.PlayOneShot(Speaker.clip);

        WaveText.SetActive(true);
        yield return new WaitForSeconds(NotifTime);
        WaveText.SetActive(false);
    }
}
