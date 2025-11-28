using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject blockPrefab;
    public BlockSpawner blockSpawner;
    public Transform groundObject;
    public TextMeshProUGUI scoreText;
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

    public void AnimateScore()
    {
        StartCoroutine(ScorePop());
    }

    IEnumerator ScorePop()
    {
        Vector3 original = scoreText.transform.localScale;
        Vector3 big = original * 1.2f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            scoreText.transform.localScale = Vector3.Lerp(original, big, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            scoreText.transform.localScale = Vector3.Lerp(big, original, t);
            yield return null;
        }

        scoreText.transform.localScale = original;
    }

    public void AddScore()
    {
        score++;
        UpdateScore();
        AnimateScore();
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
