using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class LogicPicturePoker : MonoBehaviour
{

    [SerializeField] private TMP_Text ScoreDisplay;
    [SerializeField] private TMP_Text MoneyDisplayInGame;
    [SerializeField] private TMP_Text MoneyWinDisplayInGame;
    [SerializeField] public GameObject holdButton;
    [SerializeField] public GameObject drawButton;
    [SerializeField] public TMP_Text PlayerResultsDisplay;
    [SerializeField] public TMP_Text HouseResultsDisplay;
    public GameObject GameOverMenuUI;
    public GameObject PauseMenuUI;
    private int moneyLeft = 20;
    private int roundWins = 0;
    public int moneyBetInRound = 0;
    public int selectedCards = 0;
    public bool isGameOver = false;
    private bool isPaused = false;
    private List<HouseCardAction> houseCards = new List<HouseCardAction>();
    private List<PlayerCardAction> playerCards = new List<PlayerCardAction>();

    // Start is called before the first frame update
    void Start()
    {   
        GameOverMenuUI.SetActive(false);
        PauseMenuUI.SetActive(false);
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
    }

    public void Bet() {
        if (moneyLeft > 0) {
            moneyLeft -= 1;
            moneyBetInRound += 1;
        }

        MoneyDisplayInGame.text = moneyLeft.ToString();
        MoneyWinDisplayInGame.text = moneyBetInRound.ToString() + " x 1.5";

        if (selectedCards == 0) {
            holdButton.SetActive(true);
        }
    }

    public void AddPlayerCard(PlayerCardAction playerCard)
    {
        playerCards.Add(playerCard);
    }

    public void AddHouseCard(HouseCardAction houseCard)
    {
        houseCards.Add(houseCard);
    }

    public void DrawOrHold(bool isDrawing) {
        foreach (var playerCard in playerCards)
        {
            if (playerCard.isSelected && isDrawing) {
                playerCard.AssignNewValue();
                playerCard.isSelected = false;
                playerCard.transform.position = new Vector2(playerCard.transform.position.x, playerCard.transform.position.y - 0.5f);
                selectedCards = 0;

                drawButton.SetActive(false);
                holdButton.SetActive(true);
            }
        }

        StartCoroutine(ShowHouseCards());
    }

    public IEnumerator ShowHouseCards() {
        yield return new WaitForSeconds(1);

        foreach (var houseCard in houseCards)
        {
            houseCard.ShowCard();
        }

        drawButton.SetActive(false);
        holdButton.SetActive(false);

        StartCoroutine(CheckForResults());
    }

    private IEnumerator CheckForResults() {
        yield return new WaitForSeconds(1);

        OnePairPlayerCheck();
        TwoPairPlayerCheck();
        ThreeOAKPlayerCheck();
        FullHousePlayerCheck();
        FourOAKPlayerCheck();
        FiveOAKPlayerCheck();

        OnePairHouseCheck();
        TwoPairHouseCheck();
        ThreeOAKHouseCheck();
        FullHouseHouseCheck();
        FourOAKHouseCheck();
        FiveOAKHouseCheck();

        DisplayPlayerResults();
        DisplayHouseResults();
    }

    private bool OnePairPlayerCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();

        foreach (var playerCard in playerCards) {
            if (cardCount.ContainsKey(playerCard.cardValue)) {
                cardCount[playerCard.cardValue]++;
            } else {
                cardCount[playerCard.cardValue] = 1;
            }
        }

        foreach (var count in cardCount.Values) {
            if (count == 2) {
                return true;
            }
        }

        return false;
    }

    private bool TwoPairPlayerCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        int pairCount = 0;

        foreach (var playerCard in playerCards) {
            if (cardCount.ContainsKey(playerCard.cardValue)) {
                cardCount[playerCard.cardValue]++;
            } else {
                cardCount[playerCard.cardValue] = 1;
            }
        }

        foreach (var count in cardCount.Values) {
            if (count == 2) {
                pairCount++;
            }
        }

        return pairCount == 2;
    }

    private bool ThreeOAKPlayerCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();

        foreach (var playerCard in playerCards) {
            if (cardCount.ContainsKey(playerCard.cardValue)) {
                cardCount[playerCard.cardValue]++;
            } else {
                cardCount[playerCard.cardValue] = 1;
            }
        }

        foreach (var count in cardCount.Values) {
            if (count == 3) {
                return true;
            }
        }

        return false;
    }

    private bool FullHousePlayerCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        bool hasThreeOfAKind = false;
        bool hasPair = false;

        foreach (var playerCard in playerCards) {
            if (cardCount.ContainsKey(playerCard.cardValue)) {
                cardCount[playerCard.cardValue]++;
            } else {
                cardCount[playerCard.cardValue] = 1;
            }
        }

        foreach (var count in cardCount.Values) {
            if (count == 3) {
                hasThreeOfAKind = true;
            } else if (count == 2) {
                hasPair = true;
            }
        }

        return hasThreeOfAKind && hasPair;
    }

    private bool FourOAKPlayerCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();

        foreach (var playerCard in playerCards) {
            if (cardCount.ContainsKey(playerCard.cardValue)) {
                cardCount[playerCard.cardValue]++;
            } else {
                cardCount[playerCard.cardValue] = 1;
            }
        }

        foreach (var count in cardCount.Values) {
            if (count == 4) {
                return true;
            }
        }

        return false;
    }

    private bool FiveOAKPlayerCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();

        foreach (var playerCard in playerCards) {
            if (cardCount.ContainsKey(playerCard.cardValue)) {
                cardCount[playerCard.cardValue]++;
            } else {
                cardCount[playerCard.cardValue] = 1;
            }
        }

        foreach (var count in cardCount.Values) {
            if (count == 5) {
                return true;
            }
        }

        return false;
    }

    private void DisplayPlayerResults() {
        if (FiveOAKPlayerCheck()) {
            PlayerResultsDisplay.text = "5 OF A KIND";
        } else if (FourOAKPlayerCheck()) {
            PlayerResultsDisplay.text = "4 OF A KIND";
        } else if (FullHousePlayerCheck()) {
            PlayerResultsDisplay.text = "FULL HOUSE";
        } else if (ThreeOAKPlayerCheck()) {
            PlayerResultsDisplay.text = "3 OF A KIND";
        } else if (TwoPairPlayerCheck()) {
            PlayerResultsDisplay.text = "2 PAIRS";
        } else if (OnePairPlayerCheck()) {
            PlayerResultsDisplay.text = "1 PAIR";
        } else {
            PlayerResultsDisplay.text = "Junk";
        }
    }

    ////////////////////////////////////////////
    
    private bool OnePairHouseCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();

        foreach (var houseCard in houseCards) {
            if (cardCount.ContainsKey(houseCard.cardValue)) {
                cardCount[houseCard.cardValue]++;
            } else {
                cardCount[houseCard.cardValue] = 1;
            }
        }

        foreach (var count in cardCount.Values) {
            if (count == 2) {
                return true;
            }
        }

        return false;
    }

    private bool TwoPairHouseCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        int pairCount = 0;

        foreach (var houseCard in houseCards) {
            if (cardCount.ContainsKey(houseCard.cardValue)) {
                cardCount[houseCard.cardValue]++;
            } else {
                cardCount[houseCard.cardValue] = 1;
            }
        }

        foreach (var count in cardCount.Values) {
            if (count == 2) {
                pairCount++;
            }
        }

        return pairCount == 2;
    }

    private bool ThreeOAKHouseCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();

        foreach (var houseCard in houseCards) {
            if (cardCount.ContainsKey(houseCard.cardValue)) {
                cardCount[houseCard.cardValue]++;
            } else {
                cardCount[houseCard.cardValue] = 1;
            }
        }

        foreach (var count in cardCount.Values) {
            if (count == 3) {
                return true;
            }
        }

        return false;
    }

    private bool FullHouseHouseCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        bool hasThreeOfAKind = false;
        bool hasPair = false;

        foreach (var houseCard in houseCards) {
            if (cardCount.ContainsKey(houseCard.cardValue)) {
                cardCount[houseCard.cardValue]++;
            } else {
                cardCount[houseCard.cardValue] = 1;
            }
        }

        foreach (var count in cardCount.Values) {
            if (count == 3) {
                hasThreeOfAKind = true;
            } else if (count == 2) {
                hasPair = true;
            }
        }

        return hasThreeOfAKind && hasPair;
    }

    private bool FourOAKHouseCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();

        foreach (var houseCard in houseCards) {
            if (cardCount.ContainsKey(houseCard.cardValue)) {
                cardCount[houseCard.cardValue]++;
            } else {
                cardCount[houseCard.cardValue] = 1;
            }
        }

        foreach (var count in cardCount.Values) {
            if (count == 4) {
                return true;
            }
        }

        return false;
    }

    private bool FiveOAKHouseCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();

        foreach (var houseCard in houseCards) {
            if (cardCount.ContainsKey(houseCard.cardValue)) {
                cardCount[houseCard.cardValue]++;
            } else {
                cardCount[houseCard.cardValue] = 1;
            }
        }

        foreach (var count in cardCount.Values) {
            if (count == 5) {
                return true;
            }
        }

        return false;
    }

    private void DisplayHouseResults() {
        if (FiveOAKHouseCheck()) {
            HouseResultsDisplay.text = "5 OF A KIND";
        } else if (FourOAKHouseCheck()) {
            HouseResultsDisplay.text = "4 OF A KIND";
        } else if (FullHouseHouseCheck()) {
            HouseResultsDisplay.text = "FULL HOUSE";
        } else if (ThreeOAKHouseCheck()) {
            HouseResultsDisplay.text = "3 OF A KIND";
        } else if (TwoPairHouseCheck()) {
            HouseResultsDisplay.text = "2 PAIRS";
        } else if (OnePairHouseCheck()) {
            HouseResultsDisplay.text = "1 PAIR";
        } else {
            HouseResultsDisplay.text = "Junk";
        }
    }

    // Metodo para cuando el jugador pierda
    public int GameOver() {
        if (isGameOver) return 0;

        isGameOver = true;


        // Hace que la pantalla de GameOver se muestre
        StartCoroutine(ShowGameOverUI(1));
        
        Time.timeScale = 0f;
        return 0;
    }

    private IEnumerator ShowGameOverUI(float inGameTime) {
        yield return new WaitForSecondsRealtime(inGameTime);

        ScoreDisplay.text = "Score: " + roundWins.ToString();
        GameOverMenuUI.SetActive(true);
    }

    // Controles para el menu de pausa y gameover
    public void Retry() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("PicturePoker");
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
