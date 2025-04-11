using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using JetBrains.Annotations;
using System;

public class LogicPicturePoker : MonoBehaviour
{

    [SerializeField] private TMP_Text ScoreDisplay;
    [SerializeField] private TMP_Text ScoreDisplayInGame;
    [SerializeField] private TMP_Text MoneyDisplayInGame;
    [SerializeField] private TMP_Text MoneyWinDisplayInGame;
    [SerializeField] private GameObject betButton;
    private Button buttonComponent;
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
    private List<HouseCardAction> houseCardsInUse = new List<HouseCardAction>();
    private List<PlayerCardAction> playerCardsInUse = new List<PlayerCardAction>();

    // Start is called before the first frame update
    void Start()
    {   
        GameOverMenuUI.SetActive(false);
        PauseMenuUI.SetActive(false);

        buttonComponent = betButton.GetComponent<Button>();
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
        MoneyWinDisplayInGame.text = moneyBetInRound.ToString() + " x 2";

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

    public void AddPlayerCardInUse(PlayerCardAction playerCard) {
        playerCardsInUse.Add(playerCard);
    }

    public void AddHouseCardInUse(HouseCardAction houseCard) {
        houseCardsInUse.Add(houseCard);
    }

    public void DrawOrHold(bool isDrawing) {
        buttonComponent.interactable = false;

        foreach (var playerCard in playerCards)
        {
            if (playerCard.isSelected && isDrawing) {
                playerCard.AssignNewValue();
                playerCard.isSelected = false;
                playerCard.transform.position = new Vector2(playerCard.transform.position.x, playerCard.transform.position.y - 0.5f);
                selectedCards = 0;

                drawButton.SetActive(false);
                holdButton.SetActive(false);
            }
        }

        drawButton.SetActive(false);
        holdButton.SetActive(false);

        StartCoroutine(ShowHouseCards());
    }

    public IEnumerator ShowHouseCards() {
        yield return new WaitForSeconds(1);

        foreach (var houseCard in houseCards)
        {
            houseCard.ShowCard();
        }

        StartCoroutine(CheckForResults());
    }

    public int levelOfPlayerResults = 0;
    private int highestPlayerCardValue = 0;

    public int levelOfHouseResults = 0;
    private int highestHouseCardValue = 0;


    private IEnumerator CheckForResults() {
        yield return new WaitForSeconds(1);

        if (FiveOAKPlayerCheck()) {
            PlayerResultsDisplay.text = "5 OF A KIND";
            levelOfPlayerResults = 6;
        } else if (FourOAKPlayerCheck()) {
            PlayerResultsDisplay.text = "4 OF A KIND";
            levelOfPlayerResults = 5;
        } else if (FullHousePlayerCheck()) {
            PlayerResultsDisplay.text = "FULL HOUSE";
            levelOfPlayerResults = 4;
        } else if (ThreeOAKPlayerCheck()) {
            PlayerResultsDisplay.text = "3 OF A KIND";
            levelOfPlayerResults = 3;
        } else if (TwoPairPlayerCheck()) {
            PlayerResultsDisplay.text = "2 PAIRS";
            levelOfPlayerResults = 2;
        } else if (OnePairPlayerCheck()) {
            PlayerResultsDisplay.text = "1 PAIR";
            levelOfPlayerResults = 1;
        } else {
            PlayerResultsDisplay.text = "Junk";
            levelOfPlayerResults = 0;
        }

        if (FiveOAKHouseCheck()) {
            HouseResultsDisplay.text = "5 OF A KIND";
            levelOfHouseResults = 6;
        } else if (FourOAKHouseCheck()) {
            HouseResultsDisplay.text = "4 OF A KIND";
            levelOfHouseResults = 5;
        } else if (FullHouseHouseCheck()) {
            HouseResultsDisplay.text = "FULL HOUSE";
            levelOfHouseResults = 4;
        } else if (ThreeOAKHouseCheck()) {
            HouseResultsDisplay.text = "3 OF A KIND";
            levelOfHouseResults = 3;
        } else if (TwoPairHouseCheck()) {
            HouseResultsDisplay.text = "2 PAIRS";
            levelOfHouseResults = 2;
        } else if (OnePairHouseCheck()) {
            HouseResultsDisplay.text = "1 PAIR";
            levelOfHouseResults = 1;
        } else {
            HouseResultsDisplay.text = "Junk";
            levelOfHouseResults = 0;
        }

        StartCoroutine(ApplyResultsAndReset(3));
    }

  private bool OnePairPlayerCheck() {
    Dictionary<float, int> cardCount = new Dictionary<float, int>();
    float pairValue = -1;

    foreach (var playerCard in playerCards) {
        if (cardCount.ContainsKey(playerCard.cardValue)) {
            cardCount[playerCard.cardValue]++;
        } else {
            cardCount[playerCard.cardValue] = 1;
        }
    }

    foreach (var item in cardCount) {
        if (item.Value == 2) {
            pairValue = item.Key;
            break;
        }
    }

    if (pairValue != -1) {
        foreach (var playerCard in playerCards) {
            if (playerCard.cardValue == pairValue) {
                AddPlayerCardInUse(playerCard);
            }
        }
        return true;
    }

    return false;
}


    private bool TwoPairPlayerCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        List<float> pairValues = new List<float>();

        foreach (var playerCard in playerCards) {
            if (cardCount.ContainsKey(playerCard.cardValue)) {
                cardCount[playerCard.cardValue]++;
            } else {
                cardCount[playerCard.cardValue] = 1;
            }
        }

        foreach (var item in cardCount) {
            if (item.Value == 2) {
                pairValues.Add(item.Key);
            }
        }

        if (pairValues.Count == 2) {
            foreach (var playerCard in playerCards) {
                if (pairValues.Contains(playerCard.cardValue)) {
                    AddPlayerCardInUse(playerCard);
                }
            }
            return true;
        }

        return false;
    }


    private bool ThreeOAKPlayerCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        float threeOfAKindValue = -1;

        foreach (var playerCard in playerCards) {
            if (cardCount.ContainsKey(playerCard.cardValue)) {
                cardCount[playerCard.cardValue]++;
            } else {
                cardCount[playerCard.cardValue] = 1;
            }
        }

        foreach (var item in cardCount) {
            if (item.Value == 3) {
                threeOfAKindValue = item.Key;
                break;
            }
        }

        if (threeOfAKindValue != -1) {
            foreach (var playerCard in playerCards) {
                if (playerCard.cardValue == threeOfAKindValue) {
                    AddPlayerCardInUse(playerCard);
                }
            }
            return true;
        }

        return false;
    }


    private bool FullHousePlayerCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        float threeOfAKindValue = -1;
        float pairValue = -1;

        foreach (var playerCard in playerCards) {
            if (cardCount.ContainsKey(playerCard.cardValue)) {
                cardCount[playerCard.cardValue]++;
            } else {
                cardCount[playerCard.cardValue] = 1;
            }
        }

        foreach (var item in cardCount) {
            if (item.Value == 3) {
                threeOfAKindValue = item.Key;
            } else if (item.Value == 2) {
                pairValue = item.Key;
            }
        }

        if (threeOfAKindValue != -1 && pairValue != -1) {
            foreach (var playerCard in playerCards) {
                if (playerCard.cardValue == threeOfAKindValue || playerCard.cardValue == pairValue) {
                    AddPlayerCardInUse(playerCard);
                }
            }
            return true;
        }

        return false;
    }


    private bool FourOAKPlayerCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        float fourOfAKindValue = -1;

        foreach (var playerCard in playerCards) {
            if (cardCount.ContainsKey(playerCard.cardValue)) {
                cardCount[playerCard.cardValue]++;
            } else {
                cardCount[playerCard.cardValue] = 1;
            }
        }

        foreach (var item in cardCount) {
            if (item.Value == 4) {
                fourOfAKindValue = item.Key;
                break;
            }
        }

        if (fourOfAKindValue != -1) {
            foreach (var playerCard in playerCards) {
                if (playerCard.cardValue == fourOfAKindValue) {
                    AddPlayerCardInUse(playerCard);
                }
            }
            return true;
        }

        return false;
    }


    private bool FiveOAKPlayerCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        float fiveOfAKindValue = -1;

        foreach (var playerCard in playerCards) {
            if (cardCount.ContainsKey(playerCard.cardValue)) {
                cardCount[playerCard.cardValue]++;
            } else {
                cardCount[playerCard.cardValue] = 1;
            }
        }

        foreach (var item in cardCount) {
            if (item.Value == 5) {
                fiveOfAKindValue = item.Key;
                break;
            }
        }

        if (fiveOfAKindValue != -1) {
            foreach (var playerCard in playerCards) {
                if (playerCard.cardValue == fiveOfAKindValue) {
                    AddPlayerCardInUse(playerCard);
                }
            }
            return true;
        }

        return false;
    }

    ////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////
    
    private bool OnePairHouseCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        float pairValue = -1;

        foreach (var houseCard in houseCards) {
            if (cardCount.ContainsKey(houseCard.cardValue)) {
                cardCount[houseCard.cardValue]++;
            } else {
                cardCount[houseCard.cardValue] = 1;
            }
        }

        foreach (var item in cardCount) {
            if (item.Value == 2) {
                pairValue = item.Key;
                break;
            }
        }

        if (pairValue != -1) {
            foreach (var houseCard in houseCards) {
                if (houseCard.cardValue == pairValue) {
                    AddHouseCardInUse(houseCard);
                }
            }
            return true;
        }

        return false;
    }


    private bool TwoPairHouseCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        List<float> pairValues = new List<float>();

        foreach (var houseCard in houseCards) {
            if (cardCount.ContainsKey(houseCard.cardValue)) {
                cardCount[houseCard.cardValue]++;
            } else {
                cardCount[houseCard.cardValue] = 1;
            }
        }

        foreach (var item in cardCount) {
            if (item.Value == 2) {
                pairValues.Add(item.Key);
            }
        }

        if (pairValues.Count == 2) {
            foreach (var houseCard in houseCards) {
                if (pairValues.Contains(houseCard.cardValue)) {
                    AddHouseCardInUse(houseCard);
                }
            }
            return true;
        }

        return false;
    }


    private bool ThreeOAKHouseCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        float threeOfAKindValue = -1;

        foreach (var houseCard in houseCards) {
            if (cardCount.ContainsKey(houseCard.cardValue)) {
                cardCount[houseCard.cardValue]++;
            } else {
                cardCount[houseCard.cardValue] = 1;
            }
        }

        foreach (var item in cardCount) {
            if (item.Value == 3) {
                threeOfAKindValue = item.Key;
                break;
            }
        }

        if (threeOfAKindValue != -1) {
            foreach (var houseCard in houseCards) {
                if (houseCard.cardValue == threeOfAKindValue) {
                    AddHouseCardInUse(houseCard);
                }
            }
            return true;
        }

        return false;
    }


    private bool FullHouseHouseCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        float threeOfAKindValue = -1;
        float pairValue = -1;

        foreach (var houseCard in houseCards) {
            if (cardCount.ContainsKey(houseCard.cardValue)) {
                cardCount[houseCard.cardValue]++;
            } else {
                cardCount[houseCard.cardValue] = 1;
            }
        }

        foreach (var item in cardCount) {
            if (item.Value == 3) {
                threeOfAKindValue = item.Key;
            } else if (item.Value == 2) {
                pairValue = item.Key;
            }
        }

        if (threeOfAKindValue != -1 && pairValue != -1) {
            foreach (var houseCard in houseCards) {
                if (houseCard.cardValue == threeOfAKindValue || houseCard.cardValue == pairValue) {
                    AddHouseCardInUse(houseCard);
                }
            }
            return true;
        }

        return false;
    }


    private bool FourOAKHouseCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        float fourOfAKindValue = -1;

        foreach (var houseCard in houseCards) {
            if (cardCount.ContainsKey(houseCard.cardValue)) {
                cardCount[houseCard.cardValue]++;
            } else {
                cardCount[houseCard.cardValue] = 1;
            }
        }

        foreach (var item in cardCount) {
            if (item.Value == 4) {
                fourOfAKindValue = item.Key;
                break;
            }
        }

        if (fourOfAKindValue != -1) {
            foreach (var houseCard in houseCards) {
                if (houseCard.cardValue == fourOfAKindValue) {
                    AddHouseCardInUse(houseCard);
                }
            }
            return true;
        }

        return false;
    }


    private bool FiveOAKHouseCheck() {
        Dictionary<float, int> cardCount = new Dictionary<float, int>();
        float fiveOfAKindValue = -1;

        foreach (var houseCard in houseCards) {
            if (cardCount.ContainsKey(houseCard.cardValue)) {
                cardCount[houseCard.cardValue]++;
            } else {
                cardCount[houseCard.cardValue] = 1;
            }
        }

        foreach (var item in cardCount) {
            if (item.Value == 5) {
                fiveOfAKindValue = item.Key;
                break;
            }
        }

        if (fiveOfAKindValue != -1) {
            foreach (var houseCard in houseCards) {
                if (houseCard.cardValue == fiveOfAKindValue) {
                    AddHouseCardInUse(houseCard);
                }
            }
            return true;
        }

        return false;
    }


    private IEnumerator ApplyResultsAndReset(int interval) {
        yield return new WaitForSeconds(interval);

        if (levelOfPlayerResults > levelOfHouseResults) {
            moneyBetInRound *= 2;
            moneyLeft += moneyBetInRound;
            MoneyDisplayInGame.text = moneyLeft.ToString();

            roundWins += 1;
            
            ScoreDisplayInGame.text = roundWins.ToString();
        } 
        
        if (levelOfPlayerResults == levelOfHouseResults) {
            foreach (var playerCard in playerCardsInUse)
            {
                if (Math.Abs(playerCard.cardValue - 6) > highestPlayerCardValue) {
                    highestPlayerCardValue = Math.Abs(playerCard.cardValue - 6);
                }
            }

            foreach (var houseCard in houseCardsInUse)
            {
                if (Math.Abs(houseCard.cardValue - 6) > highestHouseCardValue) {
                    highestHouseCardValue = Math.Abs(houseCard.cardValue - 6);
                }
            }

            levelOfPlayerResults *= highestPlayerCardValue;
            levelOfHouseResults *= highestHouseCardValue;

            if (levelOfPlayerResults != levelOfHouseResults)
            {
                if (levelOfPlayerResults > levelOfHouseResults) {
                    moneyBetInRound *= 2;
                    moneyLeft += moneyBetInRound;
                    MoneyDisplayInGame.text = moneyLeft.ToString();

                    roundWins += 1;
                    
                    ScoreDisplayInGame.text = roundWins.ToString();
                } 
            }
        }

        moneyBetInRound = 0;
        MoneyWinDisplayInGame.text = "0 x 2";

        drawButton.SetActive(false);
        holdButton.SetActive(false);

        PlayerResultsDisplay.text = "";
        HouseResultsDisplay.text = "";

        houseCardsInUse.Clear();
        playerCardsInUse.Clear();

        foreach (var playerCard in playerCards)
        {
            playerCard.AssignNewValue();
        }

        foreach (var houseCard in houseCards)
        {
            houseCard.AssignNewValue();
            houseCard.HideCard();
        }

        if (moneyLeft <= 0) {
            GameOver();
        }

        buttonComponent.interactable = true;

        highestPlayerCardValue = 0;
        highestHouseCardValue = 0;
    }

    // houseCardMetodo para cuando el jugador pierda
    public int GameOver() {
        Debug.Log("GO");
        if (isGameOver) return 0;

        isGameOver = true;

        // Hace que la pantalla de GameOver se muestre
        StartCoroutine(ShowGameOverUI(0));
        
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
