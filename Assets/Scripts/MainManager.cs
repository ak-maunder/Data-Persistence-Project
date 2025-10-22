using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    //Game variables
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    //UI variables
    public Text ScoreText;
    public GameObject GameOverText;

    public Text HighScoreText;
    public int highScore = 6;
    public string userName;


    //Game state variables
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;


    // Start is called before the first frame update
    private void Awake()
    {
        LoadUserData();
    }

    void Start()
    {
        //Display name and high score 
        

        
        

        //gameplay
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        HighScoreText.text = "Best Score: " + userName + ": " + highScore;

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";

        if (m_Points > highScore)
        {
            highScore = m_Points;
            userName = Menu.FirstInstance.userName;
            SaveUserData();
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }


    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    [System.Serializable]
    class SaveData
    {
        public string userName;
        public int highScore;
    }

    public void SaveUserData()
    {
        SaveData data = new SaveData();
        data.userName = userName;
        data.highScore = highScore;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadUserData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            userName = data.userName;
            highScore = data.highScore;
        }
        else
        {
            userName = Menu.FirstInstance.userName;
            highScore = 0;
        }
        
    }
}
