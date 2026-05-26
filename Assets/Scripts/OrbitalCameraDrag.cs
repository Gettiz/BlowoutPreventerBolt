using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitalCameraDrag : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private InputManager inputManager;
    
    [SerializeField] private float maxDistancePerScroll = 0.5f;
    
    [SerializeField] private float defaultDistance = 2f;
    [SerializeField] private float minDistance = 1.5f;
    [SerializeField] private float maxDistance = 4.5f;
    [SerializeField] private float dragSpeed = 0.1f;
    
    [SerializeField] private float minPitch = 0f;
    [SerializeField] private float maxPitch = 90f;

    [SerializeField] private float currentDistance;
    
    private bool isDragging;
    
    private float currentYaw;
    private float currentPitch;

    private void OnEnable()
    {
        inputManager.MoveCameraEvent += OnCameraMove;
        inputManager.MouseLBEvent += OnMouseLB;
        inputManager.MouseScrollUpEvent += OnMouseScrollUp;
        inputManager.MouseScrollDownEvent += OnMouseScrollDown;
    }
    
    private void OnDisable()
    {
        inputManager.MoveCameraEvent -= OnCameraMove;
        inputManager.MouseLBEvent -= OnMouseLB;
        inputManager.MouseScrollUpEvent -= OnMouseScrollUp;
        inputManager.MouseScrollDownEvent -= OnMouseScrollDown;
    }
    
    private void Start()
    {
        if (cameraTarget == null) 
        {
            Debug.LogWarning("No camera target has been selected or been found");
            return;
        }
        
        Vector3 angles = transform.eulerAngles;
        currentYaw = angles.y;
        currentPitch = angles.x;

        currentDistance = defaultDistance;
    }

    private void OnCameraMove(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            currentYaw += context.ReadValue<Vector2>().x * dragSpeed;
            currentPitch -= context.ReadValue<Vector2>().y * dragSpeed;
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
        }
    }

    private void OnMouseLB(InputAction.CallbackContext context)
    {
        isDragging = context.ReadValueAsButton(); 
    }
    
    private void OnMouseScrollUp(InputAction.CallbackContext context)
    {
        if (context.performed) currentDistance -= maxDistancePerScroll;
    }
    
    private void OnMouseScrollDown(InputAction.CallbackContext context)
    {
        if (context.performed) currentDistance += maxDistancePerScroll;
    }
    
    private void LateUpdate()
    {
        if (cameraTarget == null) return;
        
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        Vector3 position = cameraTarget.position - (rotation * Vector3.forward * currentDistance);
        
        transform.position = position;
        transform.rotation = rotation;
    }
}