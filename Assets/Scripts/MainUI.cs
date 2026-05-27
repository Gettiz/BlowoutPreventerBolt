using System;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    // Since there no much going on all the UI will be in this script

    [SerializeField] BoltGameManager boltGameManager;
    [SerializeField] private GameObject startCanvas, gameOverCanvas, visualizeCanvas;
    
    public enum UiStates
    {
        UiNone,
        UiMenu,
        UiInGame,
    }

    public UiStates uiState;
    
    public void StartGame()
    {
        uiState = UiStates.UiNone;
        VisualizeOrder();
    }

    private void SwitchUiState(UiStates state)
    {
        switch (state)
        {
                   case UiStates.UiNone:
                   default:
                   {
                       startCanvas.SetActive(false);
                       gameOverCanvas.SetActive(false);
                       visualizeCanvas.SetActive(false);  
                   }
                       break;
                   case UiStates.UiMenu:
                   {
                       startCanvas.SetActive(true);
                       gameOverCanvas.SetActive(true);
                       visualizeCanvas.SetActive(false);
                   }
                       break;
                   case UiStates.UiInGame:
                   {
                       startCanvas.SetActive(false);
                       gameOverCanvas.SetActive(false);
                       visualizeCanvas.SetActive(true);
                   }
                       break;
        }

    }
    
    private void EnableMenu() {SwitchUiState(UiStates.UiMenu);}

    public void VisualizeOrder()
    {
        SwitchUiState(UiStates.UiNone);
        boltGameManager.StartBoltGame();
    }

    private void OnEnable()
    {
        BoltGameManager.Bolt_GameOver += EnableMenu;
    }

    private void OnDisable()
    {
        BoltGameManager.Bolt_GameOver -= EnableMenu;
    }
}
