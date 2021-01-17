using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    #region GameManager Singleton
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }


    private void Awake()
    {
        scoreText = scoreObject.GetComponent<TextMeshPro>();
        highScoreText = highScoreObject.GetComponent<TextMeshPro>();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion
    int level = 1;
    int score;
    TextMeshPro scoreText;
    [SerializeField]GameObject levelIndicator;
    TextMeshProUGUI levelIndicatorText;
    int highScore;
    TextMeshPro highScoreText;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject scoreObject;
    [SerializeField] GameObject highScoreObject;
    public int enemiesLeft;
    [SerializeField] GameObject[] powerUps;
    public bool isMovingRight = true;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int waves;
    public GameObject player;
    int spawnProbability = 3;
    [SerializeField] bool gameFinished = false;
    private void Start()
    {
        
        levelIndicatorText = levelIndicator.GetComponent<TextMeshProUGUI>();
        StartCoroutine("LevelIndicator");
        GameLoop();
        HighScoreCheck();
    }

    void GameLoop()
    {
        if (enemiesLeft < 0) enemiesLeft = 0;
        //check if number of enemiesLeft is 0 if it is, try to launch next wave of enemies
       if (enemiesLeft == 0)
        {
            switch (waves)
            {

                case 0:
                    StartCoroutine("LevelIndicator");
                    StartCoroutine(Spawn(enemies[0], 3, 3f, Random.Range(-6, 6)));
                    waves++;
                    break;
                case 1:
                StartCoroutine(Spawn(enemies[1], 4, 2f, Random.Range(-6, 6)));
                    waves++;
                    break;
                case 2:
                    level++;
                    StartCoroutine("LevelIndicator");
                    StartCoroutine(Spawn(enemies[0], 15, 0.5f, -5));
                    waves++;
                    break;
                case 3:
                StartCoroutine(Spawn(enemies[2], 3, 1f, -5));
                StartCoroutine(Spawn(enemies[1], 4, 7f, Random.Range(-6, 6)));
                    waves++;
                    break;
                case 4:
                StartCoroutine(Spawn(enemies[2], 5, 3f, -5));
                    waves++;
                    break;
                case 5:
                StartCoroutine(Spawn(enemies[2], 4, 2f, -5));
                    waves++;
                    break;

                case 6:
                    level++;
                    StartCoroutine("LevelIndicator");
                    StartCoroutine(Spawn(enemies[0], 6, 0.5f, -5));
                    StartCoroutine(Spawn(enemies[2], 3, 4f, -5));
                    StartCoroutine(Spawn(enemies[1], 4, 7f, Random.Range(-6, 6)));
                    waves++;
                    break;
                case 7:
                    StartCoroutine(Spawn(enemies[2], 5, 2f, -5));
                    waves++;
                    break;
                case 8:
                    StartCoroutine(Spawn(enemies[1], 2, 1f, -5));
                    waves++;
                    break;
                case 9:

                    //If it is a boss fight powerup spawn probability is 100%
                    spawnProbability = 100;
                    level++;
                    StartCoroutine("LevelIndicator");
                    StartCoroutine(Spawn(enemies[3], 1, 1f, -5));
                    StartCoroutine(Spawn(enemies[1], 3, 5f, -5));
                    StartCoroutine(Spawn(enemies[0], 3, 5f, -5));
                    waves++;
                    break;
                case 10:
                    gameFinished = true;
                    GameOver();
                        break;

            }
            
        }
        else
        {
            //if player didnt kill all enemies wait for wave end
            StartCoroutine("waitForWaveEnd");
        }


    }

  
    IEnumerator waitForWaveEnd()
    {
        //wait 5 second for Player to kill all enemies and try to launch next wave
        yield return new WaitForSeconds(5);
        GameLoop();
    }
    public IEnumerator Spawn (GameObject enemyType, int numberOfEnemies, float periodBetweenSpawns,float xPosition)
    {
        enemiesLeft += numberOfEnemies;
        for (int i=0; numberOfEnemies>i;i++)
        {
            Instantiate(enemyType, new Vector3(Random.Range(-6, 6), 3.2f, 0), Quaternion.identity);
            yield return new WaitForSeconds(periodBetweenSpawns);

        }
        GameLoop();
    }
   
    IEnumerator LevelIndicator()
    {
        
        levelIndicator.SetActive(true);
        levelIndicatorText.text = "Level " + level.ToString();
        if (level > 3)
        {
            levelIndicatorText.text = "BOSS";
        }
        yield return new WaitForSeconds(3);
        levelIndicator.SetActive(false);
    }
    private void HighScoreCheck()
    {
            highScore = PlayerPrefs.GetInt("highScore");
            highScoreText.text = highScore.ToString();
            
    }    
    public void OnTryAgainButtonPressed()
    {
        Time.timeScale = 1;
        //If Try Again is pressed load 1st level
        SceneManager.LoadScene(1);
    }
    public void OnExitButtonPressed()
    {
        //If exit button is pressed quit app
        Application.Quit();
    }
    public void OnMainMenuButtonPressed()
    {
        //If Main Menu Button is Pressed LoadScene with Main Menu
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }    


    public void AddPointsToScore(int points)
    {
        //Adds points to score and updates score in the UI
        score += points;
        scoreText.text = score.ToString();
        if(score>highScore)
        {
            highScore = score;
            highScoreText.text = highScore.ToString();
            PlayerPrefs.SetInt("highScore", score);
        }
        //call a function to try spawn a powerup
        SpawnPowerUp();
        //reduce number of enemies left
        enemiesLeft--;
    }
    public void SpawnPowerUp()
    {

        //There is spawnProbability probality to spawn a powerUp
        if (Random.Range(0,100)<spawnProbability)
        {
            int i = Random.Range(0, 3);
            Instantiate(powerUps[i], new Vector3(Random.Range(-6, 6), -4.7f, 0), Quaternion.identity) ;
        }
    }

    public void GameOver()
    {


        //if score is more than highscore save it to the playerprefs
        if (score>highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
        }
        //Show game over menu
        gameOverMenu.SetActive(true);
        
        
        if (gameFinished)
        {
            //If game is finished change game over text in text menu to 'you won'
            gameOverText.GetComponent<TextMeshProUGUI>().text = "YOU WON!";
        }
        //Pause physics
        Time.timeScale = 0;
    }


}
