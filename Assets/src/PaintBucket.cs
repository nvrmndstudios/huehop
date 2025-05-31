using System.Collections.Generic;
using UnityEngine;

public class PaintBucket : MonoBehaviour
{
    [Header("References")]
    public Transform target;  // BaseMover's transform

    [Header("Visuals")]
    [SerializeField]
    private List<SpriteRenderer> visuals;
    public SerialisableColorMap CurrentColorMap => _currentColorMap;

    private SerialisableColorMap _currentColorMap;

    public float followSpeed = 5f;
    
    private bool _isGamePlaying = false;

    private void Update()
    {
        if (!_isGamePlaying) return;
        
        if (target != null)
        {
            Vector3 targetPos = new Vector3(target.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
    }

    public void Show()
    {
        _isGamePlaying = true;
        gameObject.SetActive(true);
        for (int i = 0; i < visuals.Count; i++)
        {
            visuals[i].gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        _isGamePlaying = false;
        gameObject.SetActive(false);
        for (int i = 0; i < visuals.Count; i++)
        {
            visuals[i].gameObject.SetActive(false);
        }
    }

    public void SetColor(SerialisableColorMap colorMap)
    {
        _currentColorMap = colorMap;
        UpdateColorVisual();
    }

    private void UpdateColorVisual()
    {
        if (visuals == null || visuals.Count == 0) return;

        foreach (var renderer in visuals)
        {
            if (renderer != null)
            {
                renderer.color = _currentColorMap.colorValue;
            }
        }
    }
}