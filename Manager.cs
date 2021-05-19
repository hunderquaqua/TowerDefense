using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum gameStatus
{
    next,
    play, 
    gameover,
    win
}

public class Manager : Loader<Manager>
{
    [SerializeField]
    int totalWaves = 10;
    [SerializeField]
    Text totalMoneyLabel;
    [SerializeField]
    Text currentWave;
    [SerializeField]
    Text totalEscapedLabel;
    [SerializeField]
    Text playBtnLabel;
    [SerializeField]
    Button playBtn;

    int waveNumber = 0;
    int totalMoney = 10;
    int totalEscaped = 0;
    int roundEscaped = 0;
    int totalKilled = 0;
    int enemiesToSpawn = 0;
    gameStatus currentState = gameStatus.play;



    [SerializeField]
    GameObject spawnPoint;
    [SerializeField]
    Enemy[] enemies;
    [SerializeField]
    int totalEnemies = 1;
    [SerializeField]
    int enemiesPerSpawn;

    AudioSource audioSource;

    public List<Enemy> EnemyList = new List<Enemy>();


    const float spawnDelay = 0.5F; //Переменная, содержащая в себе длительность паузы после каждого спауна
    // Геттеры для переменных

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }
    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }
    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }
    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = totalMoney.ToString();
        }
    }
    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }
    }

    
    
    IEnumerator Spawn () // Отвечает за спаун врагов (какие враги и сколько их)
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    Enemy newEnemy = Instantiate(enemies[Random.Range(0, enemiesToSpawn)]) as Enemy;
                    newEnemy.transform.position = spawnPoint.transform.position; 
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy)         // Регистрирует врагов в List
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)         // Удаляет врагов из List
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyEnemies()                    // Уничтожение обхекта Enemy
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }

        EnemyList.Clear();
    }



   
    void Start()
    {
        playBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        ShowMenu();
;    }

    private void Update()
    {
        HandleEscape();
        

    }

    public void addMoney (int amount)       // Пополняет счет при убийстве
    {
        TotalMoney += amount;
    }
   
    public void subtractMoney (int amount)  // Снимает очки за побег
    {
        TotalMoney -= amount;
    }

    public void IsWaveOver()        // Проверка на то, закончилась волна, переход на следущую волну
    {
        totalEscapedLabel.text = "Escaped " + TotalEscaped + "/" + totalEnemies;

        if ((RoundEscaped + totalKilled) == totalEnemies)
        {
            if (waveNumber <= enemies.Length)
            {
                enemiesToSpawn += waveNumber;
            }
            SetCurrentGameState();
            ShowMenu();
        }
    }
    
    public void SetCurrentGameState()  // Состояние игры (победа, конец, начать игру)
    {
        if (totalEscaped >= totalEnemies)
        {
            currentState = gameStatus.gameover;
        }
        else if (waveNumber == 0 && (RoundEscaped + TotalKilled) == 0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber >= totalWaves)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void PlayButtonPressed ()  // Вызывается после нажатия Play, устанавливает все начальные значения
    {
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber++;
                totalEnemies += waveNumber;
                break;

            default:
                totalEnemies = 1;
                TotalEscaped = 0;
                TotalMoney = 10;
                enemiesToSpawn = 0;
                totalMoneyLabel.text = TotalMoney.ToString();
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagBuildSite();
                totalEscapedLabel.text = "Escaped " + totalEscaped + "/" + totalEnemies;
                audioSource.PlayOneShot(SoundManager.Instance.NewGame);

                break;
        }
        DestroyEnemies();
        RoundEscaped = 0;
        TotalKilled = 0;
        
        currentWave.text = "Wave " + (waveNumber);
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
    }

     public void ShowMenu ()        // Меню
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLabel.text = "Try again";
                AudioSource.PlayOneShot(SoundManager.Instance.GameOver);
                break;
            case gameStatus.next:
                playBtnLabel.text = "Next wave";
                break;
            case gameStatus.play:
                playBtnLabel.text = "Play";
                break;
            case gameStatus.win:
                playBtnLabel.text = "Play again";
                break;

        }
        playBtn.gameObject.SetActive(true);

    }

    private void HandleEscape ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDrag();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }
    
}
