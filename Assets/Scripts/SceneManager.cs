using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    public Player Player;
    public List<Enemy> Enemies;

    [SerializeField] private GameObject Lose;
    [SerializeField] private GameObject Win;

    private int currWave = 0;
    [SerializeField] private LevelConfig Config;
    [SerializeField] private Text wawesText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnWave();
    }

    public void AddEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
        if(Enemies.Count == 0)
        {
            SpawnWave();
        }
    }

    public void GameOver()
    {
        Lose.SetActive(true);
    }

    private void SpawnWave()
    {
        if (currWave >= Config.Waves.Length)
        {
            Win.SetActive(true);
            return;
        }

        var wave = Config.Waves[currWave];
        foreach (var character in wave.Characters)
        {
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(character, pos, Quaternion.identity);
        }
        currWave++;
        wawesText.text = $"Wave: {currWave}/{Config.Waves.Length}";
    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
