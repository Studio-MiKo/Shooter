using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiePerWave = 5;
    public int currentZombiePerWave;

    public float spawnDelay = 0.5f; // Delay between spawning each zombie in a wave

    public int currentWave = 0;
    public float waveColdown = 10.0f; // Time in seconds between waves;

    public bool inCooldown; 
    public float cooldownCounter = 0; // We only use this for testing and the UI;

    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;

    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI cooldownCounterUI;

    public TextMeshProUGUI currentWaveUI;

    void Start()
    {
        currentZombiePerWave = initialZombiePerWave;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();

        currentWave++;
        currentWaveUI.text = "Wave: " + currentWave.ToString();

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for(int i = 0; i < currentZombiePerWave; i++)
        {
            //Generate a random offset within a specified range
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            // Instantiate the Zombie
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            // Get Enemy Script
            Enemy enemyScript = zombie.GetComponent<Enemy>();

            // Track this zombie
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);

        }
    }

    void Update()
    {
        // Get all dead zombies
        List<Enemy> zombieToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombiesAlive)
        {
            if(zombie.isDead)
            {
                zombieToRemove.Add(zombie);
            }
        }

        //Actually remove all dead zombies
        foreach (Enemy zombie in zombieToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }

        zombieToRemove.Clear();

        //Start Cooldown if all zombies are dead
        if(currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            //Start cooldown for next wave
            StartCoroutine(WaveColdown());
        }

        //Run the cooldown counter
        if(inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else{
            cooldownCounter = waveColdown;
        }

        cooldownCounterUI.text = cooldownCounter.ToString("F0");
    }

    private IEnumerator WaveColdown()
    {
        inCooldown = true;
        waveOverUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveColdown);

        inCooldown = false;
        waveOverUI.gameObject.SetActive(false);

        currentZombiePerWave *= 2;

        StartNextWave();
    }
}
