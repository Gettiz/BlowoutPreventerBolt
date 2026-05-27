using System;
using System.Collections.Generic;
using UnityEngine;

public class BoltGameManager : MonoBehaviour
{
    public static event Action Bolt_GameStarted;
    public static event Action Bolt_GameOver;
    public static event Action Bolt_GameWon;

    [SerializeField] private List<GameObject> boltObjects = new List<GameObject>();

    private int currentSequenceIndex = 0;
    private bool isGameActive = false;

    private void Start()
    {
        isGameActive = true;
        Bolt_GameStarted?.Invoke();
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
        
        StartAgain(); // Testing
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