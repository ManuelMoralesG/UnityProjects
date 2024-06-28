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
    private float time = 0.0f;
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

    public GameObject GameOverMenuUI;
    public GameObject PauseMenuUI;
    [SerializeField] private TMP_Text ScoreDisplay;
    [SerializeField] private TMP_Text TimeDisplay;
    public bool isGameOver = false;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnBobOmb(spawnInterval, BobOmb));
        bobOmbControl = BobOmb.GetComponent<BobOmbControl>();

        GameOverMenuUI.SetActive(false);
        PauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        time += 1 * Time.deltaTime;

        TimeDisplay.text = Math.Round((decimal)time, 2).ToString();

        if (time >= 15) {
            bobOmbControl.publicBobOmbVelocity = 2.0f;
        }

        if (time >= 30) {
            bobOmbControl.publicBobOmbVelocity = 2.5f;
        }

        if (time >= 60) {
            bobOmbControl.publicBobOmbVelocity = 3.0f;
        }

        // Pausar el juego
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

    private IEnumerator spawnBobOmb(float Interval, GameObject BobOmb)
    {
        yield return new WaitForSeconds(Interval);

        if (time >= 40) {
            Interval = 4.1f;
        }

        if (time >= 15)
        {
            Instantiate(BobOmb, new Vector3(0.0462f, 4.03f, 0), Quaternion.identity);
        }

        Instantiate(BobOmb, new Vector3(0.0462f, -4.03f, 0), Quaternion.identity);

        if (time >= 50) {
            Instantiate(BobOmb, new Vector3(0.0462f, 4.03f, 0), Quaternion.identity);
            Instantiate(BobOmb, new Vector3(0.0462f, -4.03f, 0), Quaternion.identity);
        }

        if (time >= 90) {
            Instantiate(BobOmb, new Vector3(0.0462f, 4.03f, 0), Quaternion.identity);
            Instantiate(BobOmb, new Vector3(0.0462f, -4.03f, 0), Quaternion.identity);
        }
        
        StartCoroutine(spawnBobOmb(Interval, BobOmb));
    }

    public int GameOver() {
        if (isGameOver) return 0;

        isGameOver = true;

        GameObject[] BlackBobOmbs = GameObject.FindGameObjectsWithTag("BlackBobOmb");
        foreach(GameObject bomb in BlackBobOmbs) {
            GameObject.Destroy(bomb);
        }

        GameObject[] PinkBobOmbs = GameObject.FindGameObjectsWithTag("PinkBobOmb");
        foreach(GameObject bomb in PinkBobOmbs) {
            GameObject.Destroy(bomb);
        }

        StartCoroutine(ShowGameOverUI(1));
        
        Time.timeScale = 0f;
        return noBobOmbsInBlack + noBobOmbsInPink - 1;
    }

    private IEnumerator ShowGameOverUI(float time) {
        yield return new WaitForSecondsRealtime(time);

        ScoreDisplay.text = "Score: " + (noBobOmbsInBlack + noBobOmbsInPink).ToString();
        GameOverMenuUI.SetActive(true);
    }

    // Controla el menu de pausa y gameover
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
