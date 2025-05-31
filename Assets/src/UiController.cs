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
        highScoreText.text = $"{highScore}";
    }

    // ==============================
    // Gameplay UI Updates
    // ==============================

    public void UpdateScore(int score)
    {
        scoreText.text = $"{score}";
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
        finalScoreText.text = $"{score}";
    }

    public void OnClickRestart()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
    }

    public void OnClickHome()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Menu);
    }
    
    [SerializeField] private GameObject plusOnePrefab;
    [SerializeField] private GameObject minusOnePrefab;// Assign your prefab
    [SerializeField] private Canvas canvas; // Assign the main UI canvas

    public void ShowPlusOne(Vector3 worldPosition)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        GameObject instance = Instantiate(plusOnePrefab, canvas.transform);
        instance.transform.position = screenPos;

        StartCoroutine(AnimateFloatUp(instance));
    }
    public void ShowMinusOne(Vector3 worldPosition)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        GameObject instance = Instantiate(minusOnePrefab, canvas.transform);
        instance.transform.position = screenPos;
        StartCoroutine(AnimateFloatUp(instance));
    }
    private IEnumerator AnimateFloatUp(GameObject instance)
    {
        RectTransform rect = instance.GetComponent<RectTransform>();
        Vector3 start = rect.anchoredPosition;
        Vector3 end = start + new Vector3(0, 100f, 0); // move upward 100 units

        float duration = 0.8f;
        float time = 0;

        CanvasGroup canvasGroup = instance.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = instance.AddComponent<CanvasGroup>();
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Curve upward with ease-out
            float curve = Mathf.Sin(t * Mathf.PI * 0.5f); // ease-out
            rect.anchoredPosition = Vector3.Lerp(start, end, curve);

            // Fade out
            canvasGroup.alpha = 1 - t;

            yield return null;
        }

        Destroy(instance);
    }
}