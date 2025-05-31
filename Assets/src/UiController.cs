using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public static UiController Instance { get; private set; }

    [Header("Screens")]
    public GameObject splashScreen;
    public GameObject menuScreen;
    public GameObject gameplayScreen;
    public GameObject resultScreen;

    [Header("Menu Screen UI")]
    public TMP_Text highScoreText;

    [Header("Gameplay UI")]
    public TMP_Text scoreText;

    [Tooltip("Drag 3 life icon GameObjects here")]
    public GameObject[] lifeIcons; // Should have 3 icons

    [Header("Result Screen UI")]
    public TMP_Text finalScoreText;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // ==============================
    // Screen Visibility Handlers
    // ==============================

    public void ShowOnlySplash()
    {
        splashScreen.SetActive(true);
        menuScreen.SetActive(false);
        gameplayScreen.SetActive(false);
        resultScreen.SetActive(false);
    }

    public void ShowOnlyMenu()
    {
        splashScreen.SetActive(false);
        menuScreen.SetActive(true);
        gameplayScreen.SetActive(false);
        resultScreen.SetActive(false);
    }

    public void ShowOnlyGameplay()
    {
        splashScreen.SetActive(false);
        menuScreen.SetActive(false);
        gameplayScreen.SetActive(true);
        resultScreen.SetActive(false);
    }

    public void ShowOnlyResult()
    {
        splashScreen.SetActive(false);
        menuScreen.SetActive(false);
        gameplayScreen.SetActive(false);
        resultScreen.SetActive(true);
    }

    // ==============================
    // Menu UI Updates
    // ==============================

    public void UpdateHighScore(int highScore)
    {
        highScoreText.text = $"High Score: {highScore}";
    }

    // ==============================
    // Gameplay UI Updates
    // ==============================

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void UpdateLife(int life)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].SetActive(i < life); // Only show icons for remaining lives
        }
    }

    // ==============================
    // Result Screen Actions
    // ==============================

    public void SetFinalScore(int score)
    {
        finalScoreText.text = $"Your Score: {score}";
    }

    public void OnClickRestart()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
    }

    public void OnClickHome()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Menu);
    }
}