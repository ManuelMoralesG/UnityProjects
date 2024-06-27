using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Logic : MonoBehaviour
{
    [SerializeField] private GameObject BobOmb;
    private BobOmbControl bobOmbControl;
    private float spawnInterval = 4f;
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
    [SerializeField]
    private TMP_Text ScoreDisplay;
    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnBobOmb(spawnInterval, BobOmb));
        bobOmbControl = BobOmb.GetComponent<BobOmbControl>();
        GameOverMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        time += 1 * Time.deltaTime;

        if (time >= 25) {
            bobOmbControl.publicBobOmbVelocity = 4.0f;
        }

        if (time >= 60) {
            bobOmbControl.publicBobOmbVelocity = 4.5f;
        }
    }

    private IEnumerator spawnBobOmb(float Interval, GameObject BobOmb)
    {
        yield return new WaitForSeconds(Interval);

        if (time >= 40) {
            Interval = 3.5f;
        }

        if (time >= 90) {
            Interval = 3.2f;
        }

        if (time >= 15.0f)
        {
            Instantiate(BobOmb, new Vector3(0.0462f, 4.03f, 0), Quaternion.identity);
        }

        Instantiate(BobOmb, new Vector3(0.0462f, -4.03f, 0), Quaternion.identity);
        
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

        Debug.Log("Game Over");

        StartCoroutine(ShowGameOverUI(1));
        

        Time.timeScale = 0f;
        return noBobOmbsInBlack + noBobOmbsInPink - 1;
    }

    private IEnumerator ShowGameOverUI(float time) {
        yield return new WaitForSecondsRealtime(time);

        ScoreDisplay.text = "Score: " + (noBobOmbsInBlack + noBobOmbsInPink).ToString();
        GameOverMenuUI.SetActive(true);
    }
}
