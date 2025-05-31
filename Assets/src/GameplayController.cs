using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum ColorKeyType
{
    Violet,
    Indigo,
    Blue,
    Green,
    Yellow,
    Orange,
    Red
}

public class GameplayController : MonoBehaviour
{
    
    [Header("Color Map")]
    
    [SerializeField]
    private List<SerialisableColorMap> colorMap = new List<SerialisableColorMap>();
    
    
    [Header("References")]
    [SerializeField] private PaintBucket _paintBucket;
    [SerializeField] private CameraShaker _cameraShaker;
    [SerializeField] private Mover _mover;
    [SerializeField] private Transform _itemParent;                  // Parent to keep hierarchy clean
    [SerializeField] private GameObject _fallingItemPrefab;

    [Header("Game Config")]
    public float baseFallSpeed = 2f;
    public float speedIncreasePerScore = 0.1f;
    public float spawnInterval = 1.2f;

    private bool isPlaying = false;
    
    [Header("Spawner Settings")]
    public float spawnRangeX = 2.5f;      // left and right bounds
    public float spawnY = 6f;             // fixed Y value at top

    private void Awake()
    {
        _paintBucket.Hide();
        _mover.Hide();
    }


    public void StartGame()
    {
        GameData.CurrentScore = 0;
        GameData.CurrentLives = 3;
        
        var uiController = UiController.Instance;
        
        uiController.UpdateScore(0);
        uiController.UpdateLife(3);

        _paintBucket.SetColor(GetRandomColorMap());
        _paintBucket.Show();
        _mover.Show();
        
        isPlaying = true;
        
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (isPlaying)
        {
            SpawnRandomItem();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnRandomItem()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0);

        var item = Instantiate(_fallingItemPrefab, spawnPos, Quaternion.identity, _itemParent);
        var falling = item.GetComponent<FallingItem>();
        
        falling.Init(GetCurrentFallSpeed(), _paintBucket, GetRandomColorMap(), this);
        falling.gameObject.SetActive(true);
    }

    private float GetCurrentFallSpeed()
    {
        return baseFallSpeed + (GameData.CurrentScore * speedIncreasePerScore);
    }

    public void OnItemCollected(bool isCorrect)
    {
        if (isCorrect)
        {
            GameData.CurrentScore++;
            UiController.Instance.UpdateScore(GameData.CurrentScore);
            UiController.Instance.ShowPlusOne(_paintBucket.transform.position);
            _paintBucket.OnCorrectItemCollected();
            _paintBucket.SetColor(GetRandomColorMap()); // <=== Add this
        }
        else
        {
            _cameraShaker.Shake();
            UiController.Instance.ShowMinusOne(_paintBucket.transform.position);
            _paintBucket.OnWrongItemCollected();
            GameData.CurrentLives--;
            UiController.Instance.UpdateLife(GameData.CurrentLives);

            if (GameData.CurrentLives <= 0)
            {
                EndGame();
            }
        }
    }

    private void EndGame()
    {
        isPlaying = false;
        _paintBucket.Hide();
        _mover.Hide();
        GameManager.Instance.ChangeState(GameManager.GameState.Result);
    }

    private SerialisableColorMap GetRandomColorMap()
    {
        return colorMap[Random.Range(0, colorMap.Count)];
    }
}

[System.Serializable]
public class SerialisableColorMap
{
    public ColorKeyType colorKeyType;
    public Color colorValue;
}