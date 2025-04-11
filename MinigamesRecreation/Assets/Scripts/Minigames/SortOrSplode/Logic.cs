using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Logic : MonoBehaviour
{
    [SerializeField] private GameObject BobOmb;
    private BobOmbControl bobOmbControl;

    private float spawnInterval = 4.5f;
    private float inGameTime = 0.0f;

    private int noBobOmbsInBlack = 0;
    public int publicNoBobOmbsInBlack
    {
        get { return noBobOmbsInBlack; }
        set { noBobOmbsInBlack = value; }
    }
    private int noBobOmbsInPink = 0;
    public int publicNoBobOmbsInPink
    {
        get { return noBobOmbsInPink; }
        set { noBobOmbsInPink = value; }
    }

    [SerializeField] private TMP_Text ScoreDisplay;
    [SerializeField] private TMP_Text TimeDisplay;
    public GameObject GameOverMenuUI;
    public GameObject PauseMenuUI;
    public bool isGameOver = false;
    private bool isPaused = false;

    void Start()
    {
        StartCoroutine(spawnBobOmb(spawnInterval, BobOmb));
        bobOmbControl = BobOmb.GetComponent<BobOmbControl>();

        GameOverMenuUI.SetActive(false);
        PauseMenuUI.SetActive(false);
    }

    void Update()
    {
        inGameTime += 1 * Time.deltaTime;

        TimeDisplay.text = Math.Round((decimal)inGameTime, 2).ToString();

        // Sube la velocidad de las bobas mientras mas tiempo pase
        if (inGameTime >= 15) {
            bobOmbControl.publicBobOmbVelocity = 2.0f;
        }

        if (inGameTime >= 30) {
            bobOmbControl.publicBobOmbVelocity = 2.5f;
        }

        if (inGameTime >= 60) {
            bobOmbControl.publicBobOmbVelocity = 3.0f;
        }

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
    }

    // "Spawn" de las bombas
    private IEnumerator spawnBobOmb(float Interval, GameObject BobOmb)
    {
        yield return new WaitForSeconds(Interval);

        // Baja el intervalo de "spawn" despues de x segundos
        if (inGameTime >= 40) {
            Interval = 4.1f;
        }

        Instantiate(BobOmb, new Vector3(0.0462f, -4.03f, 0), Quaternion.identity);

        // Empieza a "spawnear" bombas arriba
        if (inGameTime >= 15)
        {
            Instantiate(BobOmb, new Vector3(0.0462f, 4.03f, 0), Quaternion.identity);
        }

        // Comienza a spawnear multiples bombas
        if (inGameTime >= 50) {
            Instantiate(BobOmb, new Vector3(0.0462f, 4.03f, 0), Quaternion.identity);
            Instantiate(BobOmb, new Vector3(0.0462f, -4.03f, 0), Quaternion.identity);
        }

        if (inGameTime >= 90) {
            Instantiate(BobOmb, new Vector3(0.0462f, 4.03f, 0), Quaternion.identity);
            Instantiate(BobOmb, new Vector3(0.0462f, -4.03f, 0), Quaternion.identity);
        }
        
        StartCoroutine(spawnBobOmb(Interval, BobOmb));
    }

    // Metodo para cuando el jugador pierda
    public int GameOver() {
        if (isGameOver) return 0;

        isGameOver = true;

        // Destruyen todos los objetos con etiquetas BlackBobOmb o Pink BobOmb
        GameObject[] BlackBobOmbs = GameObject.FindGameObjectsWithTag("BlackBobOmb");
        foreach(GameObject bomb in BlackBobOmbs) {
            GameObject.Destroy(bomb);
        }

        GameObject[] PinkBobOmbs = GameObject.FindGameObjectsWithTag("PinkBobOmb");
        foreach(GameObject bomb in PinkBobOmbs) {
            GameObject.Destroy(bomb);
        }

        // Hace que la pantalla de GameOver se muestre
        StartCoroutine(ShowGameOverUI(1));
        
        Time.timeScale = 0f;
        return 0;
    }

    private IEnumerator ShowGameOverUI(float time) {
        yield return new WaitForSecondsRealtime(time);

        ScoreDisplay.text = "Score: " + (noBobOmbsInBlack + noBobOmbsInPink).ToString();
        GameOverMenuUI.SetActive(true);
    }

    // Controles para el menu de pausa y gameover
    public void Retry() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SortOrSplode");
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
