using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BoltSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static event Action<GameObject> OnBoltClicked;
    
    [SerializeField] private Material[] hoverMaterials;

    private Renderer objRenderer;
    private Material[] originalMaterials;
    private Material[] combinedMaterialsList;

    public GameObject boltNutObj;
    [SerializeField] private float boltMoveDuration = 0.5f;
    private bool hasBeenMoved;
    
    private Coroutine moveBoltNutNumerator;
    
    [SerializeField] private Transform boltNutStartPoint;
    [SerializeField] private Transform boltNutEndPoint;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        originalMaterials = objRenderer.sharedMaterials; 
        
        int originalCount = originalMaterials.Length;
        int hoverCount = hoverMaterials.Length;
      
        combinedMaterialsList = new Material[originalCount + hoverCount];
        
        for (int i = 0; i < originalCount; i++) combinedMaterialsList[i] = originalMaterials[i];
        
        for (int i = 0; i < hoverCount; i++) combinedMaterialsList[originalCount + i] = hoverMaterials[i];

        hasBeenMoved = false;
        boltNutObj.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    { if (!hasBeenMoved) EnableOutline(); }

    public void OnPointerExit(PointerEventData eventData)
    { if (!hasBeenMoved) DisableOutline(); }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!hasBeenMoved)
        {
            moveBoltNutNumerator = StartCoroutine(MoveBoltNut());
            OnBoltClicked?.Invoke(gameObject);
        } 
    }

    public void EnableOutline()
    { if (hoverMaterials != null && hoverMaterials.Length > 0) objRenderer.sharedMaterials = combinedMaterialsList; }
    
    public void DisableOutline() 
    { objRenderer.sharedMaterials = originalMaterials; }
    
    private IEnumerator MoveBoltNut()
    {
        hasBeenMoved = true;
        DisableOutline();
        
        float elapsedTime = 0f;

        boltNutObj.SetActive(true);
        
        Vector3 startPosition = new Vector3(boltNutObj.transform.position.x, boltNutStartPoint.position.y, boltNutObj.transform.position.z);
        Vector3 endPosition = new Vector3(boltNutObj.transform.position.x, boltNutEndPoint.position.y, boltNutObj.transform.position.z);
        
        Quaternion startRotation = boltNutObj.transform.rotation;
        Quaternion endRotation = boltNutObj.transform.rotation * Quaternion.Euler(0, 0, 180);

        while (elapsedTime < boltMoveDuration)
        {
            // Time Normalization
            elapsedTime += Time.deltaTime;
            boltNutObj.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / boltMoveDuration));
            boltNutObj.transform.rotation = Quaternion.Lerp(startRotation, endRotation, (elapsedTime / boltMoveDuration));
            yield return null;
        }
        
        boltNutObj.transform.position = endPosition;
        boltNutObj.transform.rotation = endRotation;
    }

    private void ResetBolt()
    {
        if (moveBoltNutNumerator != null)
        {
            StopCoroutine(moveBoltNutNumerator);
            moveBoltNutNumerator = null;
        }
        
        hasBeenMoved = false;
        boltNutObj.SetActive(false);
        boltNutObj.transform.position = new Vector3(boltNutObj.transform.position.x, boltNutStartPoint.position.y, boltNutObj.transform.position.z);
    }

    private void OnEnable()
    {
        BoltGameManager.Bolt_GameOver += ResetBolt;
    }

    private void OnDisable()
    {
        BoltGameManager.Bolt_GameOver -= ResetBolt;
    }
}