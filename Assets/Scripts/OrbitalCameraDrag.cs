using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitalCameraDrag : MonoBehaviour
{
    private bool canMoveCamera = false;
    
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private InputManager inputManager;
    
    [SerializeField] private float maxDistancePerScroll = 0.5f;
    
    [SerializeField] private float defaultDistance = 2f;
    [SerializeField] private float minDistance = 1.5f;
    [SerializeField] private float maxDistance = 4.5f;
    [SerializeField] private float dragSpeed = 5f;
    
    [SerializeField] private float minPitch = 0f;
    [SerializeField] private float maxPitch = 90f;

    [SerializeField] private float currentDistance;
    
    [SerializeField] private Transform visualizationPosition;
    [SerializeField] private float visualizationDistance = 1;
    
    private bool isDragging;
    
    private float currentYaw;
    private float currentPitch;
    
    private void Start()
    {
        if (cameraTarget == null) 
        {
            Debug.LogWarning("No camera target has been selected or been found");
            return;
        }

        UpdateCurrentCameraAngle();

        currentDistance = defaultDistance;
    }

    private void OnCameraMove(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            currentYaw += context.ReadValue<Vector2>().x * dragSpeed * Time.deltaTime;
            currentPitch -= context.ReadValue<Vector2>().y * dragSpeed * Time.deltaTime;
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
        }
    }

    private void UpdateCurrentCameraAngle()
    {
        Vector3 angles = transform.eulerAngles;
        currentYaw = angles.y;
        currentPitch = angles.x;
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

    public void SetCameraToVisualizationPosition()
    {
        transform.position = visualizationPosition.position;
        transform.rotation = visualizationPosition.rotation; 
        currentDistance = visualizationDistance;
        UpdateCurrentCameraAngle();
    }
    
    private void LateUpdate()
    {
        if (cameraTarget == null) return;
        if (!canMoveCamera || !isDragging) return;
        
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        Vector3 position = cameraTarget.position - (rotation * Vector3.forward * currentDistance);
        
        transform.position = position;
        transform.rotation = rotation;
    }
    
    private void EnableCameraMovement() { canMoveCamera = true; }
    
    private void DisableCameraMovement() { canMoveCamera = false; }
    
    private void OnEnable()
    {
        inputManager.MoveCameraEvent += OnCameraMove;
        inputManager.MouseLBEvent += OnMouseLB;
        inputManager.MouseScrollUpEvent += OnMouseScrollUp;
        inputManager.MouseScrollDownEvent += OnMouseScrollDown;
        BoltGameManager.Bolt_GameStarted += EnableCameraMovement;
        BoltGameManager.Bolt_GameOver += DisableCameraMovement;
        BoltGameManager.Bolt_StartVisualization += SetCameraToVisualizationPosition;
    }
    
    private void OnDisable()
    {
        inputManager.MoveCameraEvent -= OnCameraMove;
        inputManager.MouseLBEvent -= OnMouseLB;
        inputManager.MouseScrollUpEvent -= OnMouseScrollUp;
        inputManager.MouseScrollDownEvent -= OnMouseScrollDown;
        BoltGameManager.Bolt_GameStarted -= EnableCameraMovement;
        BoltGameManager.Bolt_GameOver -= DisableCameraMovement;
        BoltGameManager.Bolt_StartVisualization -= SetCameraToVisualizationPosition;
    }
}
