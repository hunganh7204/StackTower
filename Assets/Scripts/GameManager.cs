using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject blockPrefab;
    public BlockSpawner blockSpawner;
    public Transform groundObject;
    public Text scoreText;
    public GameObject gameOverPanel;

    private int score = 0;
    public Transform currentBlock;
    public bool isGameOver { get; private set; } = false;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    void Start()
    {
        score = 0;
        isGameOver = false;
        UpdateScore();
        if (blockSpawner != null)
        {
            blockSpawner.SpawnInitial();
        }
    }

    public void OnBlockLanded()
    {
        if (isGameOver) return;
        AddScore();
        if (blockSpawner != null) blockSpawner.SpawnNext();
    }

    public void AddScore()
    {
        score++;
        UpdateScore();
    }

    void UpdateScore()
    {
        if (scoreText != null) scoreText.text = score.ToString();
    }

    public void SetGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
