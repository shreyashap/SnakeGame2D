using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public float snakeSpeed = 1f;
    public BodyPart bodyPrefab = null;
    public Sprite tailSprite;
    public Sprite bodySprite;
    public SnakeHead snakeHaed = null;
    public GameObject rockPrefab;
    public GameObject eggPrefab;
    public GameObject goldenEggPrefab;
    private const float width = 3.7f;
    private const float height = 7f;
    public bool isAlive = true;
    public bool waitingToPlay = true;
    private List<Egg> eggs = new List<Egg>();

    private int level = 0;
    private int noOfEggsFoeNextLEvel = 0;

    private int score;
    private int hiScore;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text hiScoreText;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text tapToPlayText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Debug.Log("Starting Snake Game");
        CreateWalls();
        
    }
    private void Update()
    {
        if(waitingToPlay)
        {
            foreach(Touch touch in Input.touches)
            {
                if(touch.phase == TouchPhase.Ended)
                {
                    StartGamePlay();
                }
            }

            if(Input.GetMouseButtonUp(0))
                StartGamePlay();
            
        }
    }

    void StartGamePlay()
    {
        score = 0;
        hiScore = 0;
        level = 0;

        scoreText.text = "Score :" + score;
        hiScoreText.text = "HiScore :" + hiScore;
        waitingToPlay = false;
        isAlive = true;

        KillOldEggs();
        LevelUp();

        tapToPlayText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }

    private void LevelUp()
    {
        level++;

        noOfEggsFoeNextLEvel = 4 + (level *2);

        snakeSpeed = 1f + (level /4f);
        if (snakeSpeed >= 6f) snakeSpeed = 6f;

        snakeHaed.ResetSnake();
        CreateEgg();
    }
    void CreateWalls()
    {
        Vector3 start = new Vector3(-width, -height, -0.01f);
        Vector3 finish = new Vector3(-width, height, -0.01f);
        CreateSingleWall(start, finish);

        start = new Vector3(width, -height, -0.01f);
        finish = new Vector3(width, height, -0.01f);
        CreateSingleWall(start, finish);


        start = new Vector3(-width, height, -0.01f);
        finish = new Vector3(width, height, -0.01f);
        CreateSingleWall(start, finish);


        start = new Vector3(-width, -height, -0.01f);
        finish = new Vector3(width, -height, -0.01f);
        CreateSingleWall(start, finish);
    }

    void CreateSingleWall(Vector3 start, Vector3 finish)
    {
        float distance = Vector2.Distance(start, finish);
        int noOfRocks = (int)distance * 3;

        Vector3 delta = (finish - start)/noOfRocks;

        Vector3 position = start;
        for(int i = 0; i < noOfRocks; i++)
        {
            float rotation = Random.Range(0, 360);
            float scale = Random.Range(1f, 2f);
            CreateRock(position, rotation, scale);
            position += delta;
        }
    }

    void CreateRock(Vector3 position, float rotation, float scale)
    {
        GameObject rock = Instantiate(rockPrefab, position, Quaternion.Euler(0, 0, rotation));
        rock.transform.localScale = new Vector3(scale, scale, 1);
    }

    void CreateEgg(bool isGolden = false)
    {
        float posX = Random.Range(-3f, 3f);
        float posY = Random.Range(-6.2f, 6.2f);

        Vector3 randomPos = new Vector3(posX, posY, -0.01f);
        Egg egg;
        if (isGolden)
            egg = Instantiate(goldenEggPrefab, randomPos, Quaternion.identity).GetComponent<Egg>();
        else
            egg = Instantiate(eggPrefab, randomPos, Quaternion.identity).GetComponent<Egg>();

        
        eggs.Add(egg);
            
    }

    private void KillOldEggs()
    {
        foreach(Egg egg in eggs)
        {
            Destroy(egg.gameObject);
        }
        eggs.Clear();
    }
    public void GameOver()
    {
        isAlive = false;
        waitingToPlay = true;

        gameOverText.gameObject.SetActive(true);
        tapToPlayText.gameObject.SetActive(true);
    }
    public void EggEaten(Egg egg)
    {
        score++;

        noOfEggsFoeNextLEvel--;

        if (noOfEggsFoeNextLEvel == 0)
        {
            score += 10;
            LevelUp();     
        }
        else if(noOfEggsFoeNextLEvel == 1)
        {
            CreateEgg(true);
        }
        else
        {
            CreateEgg(false);
        }

        if(score > hiScore)
        {
            hiScore = score;
        }
        scoreText.text = "Score :" + score;
        hiScoreText.text = "HiScore :" + hiScore;
        eggs.Remove(egg);
        Destroy(egg.gameObject);
    }
}
