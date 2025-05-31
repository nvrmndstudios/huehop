using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float boundaryX = 2.5f; // limit movement to left/right

    private Vector2 touchStart;
    
    private bool _isGamePlaying = false;

    private void Update()
    {
        if (!_isGamePlaying) { return; }
        
#if UNITY_EDITOR
        // Mouse control for testing
        if (Input.GetMouseButtonDown(0))
            touchStart = Input.mousePosition;
        else if (Input.GetMouseButton(0))
            HandleSwipe(Input.mousePosition);
#else
        // Mobile swipe
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                touchStart = touch.position;
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                HandleSwipe(touch.position);
        }
#endif
    }
    
    public void Show()
    {
        _isGamePlaying = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        _isGamePlaying = false;
        gameObject.SetActive(false);
    }

    private void HandleSwipe(Vector2 current)
    {
        float direction = current.x - touchStart.x;
        Vector3 newPos = transform.position + Vector3.right * direction * moveSpeed * Time.deltaTime * 0.01f;
        newPos.x = Mathf.Clamp(newPos.x, -boundaryX, boundaryX);
        transform.position = newPos;
    }
}