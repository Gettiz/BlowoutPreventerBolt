using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltGameManager : MonoBehaviour
{
    public static event Action Bolt_GameStarted;
    public static event Action Bolt_GameOver;
    public static event Action Bolt_GameWon;
    public static event Action Bolt_StartVisualization;
    public static event Action Bolt_EndVisualization;

    [Header("Main Settings")]
    // The execution order is based in the list order which can be changed in the unity inspector 
    [SerializeField] private List<GameObject> boltObjects = new List<GameObject>();

    private int currentSequenceIndex = 0;
    private bool isGameActive = false;

    [Header("Visualization Settings")] 
    [SerializeField] private float timePerBoltExample;
    
    public void StartBoltGame()
    {
        isGameActive = true;
        currentSequenceIndex = 0;
        Bolt_GameStarted?.Invoke();
    }

    public void StartVisualization()
    {
        StartCoroutine(HighlightExecutionOrder());
        Bolt_StartVisualization?.Invoke();
    }

    public void DisableInteraction()
    {
        isGameActive = false;
    }

    public void EnableInteraction()
    {
        isGameActive = true;
    }

    private IEnumerator HighlightExecutionOrder()
    {
        DisableInteraction();
        
        for (int i = 0; i < boltObjects.Count; i++)
        {
            BoltSelection boltSelection = boltObjects[i].GetComponent<BoltSelection>();
            
            boltSelection.EnableOutline();
            yield return new WaitForSeconds(timePerBoltExample);
            boltSelection.DisableOutline();
            
            yield return new WaitForSeconds(0.2f);
        }

        EnableInteraction();
        Bolt_EndVisualization?.Invoke();
    }

    private void HandleBoltClicked(GameObject clickedBolt)
    {
        if (!isGameActive) return;

        if (clickedBolt == boltObjects[currentSequenceIndex])
        {
            Debug.Log("Correct Bolt");
            currentSequenceIndex++;

            if (currentSequenceIndex >= boltObjects.Count)
            {
                Debug.Log("All Bolts Correct");
                isGameActive = false;
                Bolt_GameWon?.Invoke();
            }
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Wrong Bolt Starting Again");
        isGameActive = false;
        currentSequenceIndex = 0;
        Bolt_GameOver?.Invoke();
    }

    private void StartAgain()
    {
        isGameActive = true;
        Bolt_GameStarted?.Invoke();
    }
    
    private void OnEnable()
    {
        BoltSelection.OnBoltClicked += HandleBoltClicked;
    }

    private void OnDisable()
    {
        BoltSelection.OnBoltClicked -= HandleBoltClicked;
    }
}