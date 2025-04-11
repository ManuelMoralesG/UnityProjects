using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameMenu : MonoBehaviour
{
    public void GoToMainMenu() {
        SceneManager.LoadScene("Main");
    }

    public void GoToMinigame(String minigameName) {
        SceneManager.LoadScene(minigameName);
    }
}
