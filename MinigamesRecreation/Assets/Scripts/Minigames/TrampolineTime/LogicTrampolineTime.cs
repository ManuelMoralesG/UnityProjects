using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicTrampolineTime : MonoBehaviour
{
    public GameObject GameOverMenuUI;
    public GameObject PauseMenuUI;
    public bool isGameOver = false;
    private bool isPaused = false;
    [SerializeField] private TMP_Text ScoreDisplay;
    [SerializeField] private TMP_Text ScoreDisplayInGame;
    [SerializeField] public TMP_Text LivesLeftDisplay;
    public int livesLeft = 3;
    public int successfullJumps = 0;

    [SerializeField] private GameObject Jumper;
    private float spawnInterval = 4.3f;
    public float noJumpers = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameOverMenuUI.SetActive(false);
        PauseMenuUI.SetActive(false);

        StartCoroutine(spawnJumper(spawnInterval, Jumper));
    }

    private IEnumerator spawnJumper(float Interval, GameObject Jumper) {
        yield return new WaitForSeconds(Interval);

        if (noJumpers < 4) {
            Instantiate(Jumper, new Vector2(-7.45f, 3.2f), Quaternion.identity);
            noJumpers++;
        }

        StartCoroutine(spawnJumper(Interval, Jumper));
    }

    // Update is called once per frame
    void Update()
    {
        // Pausa el juego
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (!isPaused) {
                Time.timeScale = 0f;
                PauseMenuUI.SetActive(true);
                isPaused = true;
            } else if  (isPaused) {
                Resume();
            }
        }

        if (livesLeft <= 0) {
            GameOver();
        }
    }

    public int GameOver() {
        if (isGameOver) return 0;
        Debug.Log("GO");

        isGameOver = true;

        // Hace que la pantalla de GameOver se muestre
        StartCoroutine(ShowGameOverUI(0));
        
        Time.timeScale = 0f;
        return 0;
    }

    private IEnumerator ShowGameOverUI(float inGameTime) {
        yield return new WaitForSecondsRealtime(inGameTime);

        ScoreDisplay.text = "Score: " + successfullJumps.ToString();
        GameOverMenuUI.SetActive(true);
    }

    // Controles para el menu de pausa y gameover
    public void Retry() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TrampolineTime");
    }

    public void GoToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MinigameSelection"); 
    }

    public void Resume() {
        Time.timeScale = 1f;
        PauseMenuUI.SetActive(false);
        isPaused = false;
    }
}
