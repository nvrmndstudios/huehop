using UnityEngine;

public class FallingItem : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer colorRenderer;
    
    public float destroyY = -6f;

    private float _fallSpeed;
    private PaintBucket _bucketRef;
    
    private SerialisableColorMap _currentColorMap;
    private GameplayController _gameplayControllerRef;

    public void Init(float speed, PaintBucket paintBucket,  SerialisableColorMap colorMap, GameplayController gameplayController)
    {
        _fallSpeed = speed;
        _bucketRef = paintBucket;
        _currentColorMap = colorMap;
        _gameplayControllerRef = gameplayController;
        UpdateColorVisual();
    }

    private void Update()
    {
        transform.position += Vector3.down * (_fallSpeed * Time.deltaTime);

        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }

        CheckCollision();
    }

    private void CheckCollision()
    {
        if (_bucketRef == null) return;

        float dist = Vector2.Distance(transform.position, _bucketRef.transform.position);
        if (dist < 0.5f)
        {
            bool correct = (_currentColorMap.colorKeyType == _bucketRef.CurrentColorMap.colorKeyType);
            _gameplayControllerRef.OnItemCollected(correct);
            Destroy(gameObject);
        }
    }

    private void UpdateColorVisual()
    {
        if (colorRenderer == null) return;
        colorRenderer.color = _currentColorMap.colorValue;
    }
}