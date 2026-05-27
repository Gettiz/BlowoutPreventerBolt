using System;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    // Since there no much going on all the UI will be in this script

    [SerializeField] BoltGameManager boltGameManager;
    [SerializeField] private GameObject startCanvas, visualizeCanvas, winCanvas;
    
    public enum UiStates
    {
        UiNone,
        UiMenu,
        UiVisualizeMenu,
        UiWinMenu
    }

    public UiStates uiState;
    
    public void StartGame()
    {
        SwitchUiState(UiStates.UiNone);
        boltGameManager.StartBoltGame();
    }
    
    public void VisualizeOrder()
    {
        SwitchUiState(UiStates.UiNone);
        boltGameManager.StartVisualization();
    }

    private void SwitchUiState(UiStates state)
    {
        switch (state)
        {
                   case UiStates.UiNone:
                   default:
                   {
                       startCanvas.SetActive(false);
                       visualizeCanvas.SetActive(false);
                       winCanvas.SetActive(false);
                   }
                       break;
                   case UiStates.UiMenu:
                   {
                       startCanvas.SetActive(true);
                       visualizeCanvas.SetActive(false);
                       winCanvas.SetActive(false);
                   }
                       break;
                   case UiStates.UiVisualizeMenu:
                   {
                       startCanvas.SetActive(false);
                       visualizeCanvas.SetActive(true);
                       winCanvas.SetActive(false);
                   }
                       break;
                   case UiStates.UiWinMenu:
                   {
                       startCanvas.SetActive(false);
                       visualizeCanvas.SetActive(false);
                       winCanvas.SetActive(true);
                   }
                       break;
        }

    }
    
    private void EnableVisualizationMenu() { SwitchUiState(UiStates.UiVisualizeMenu); }
    private void EnableWinMenu() { SwitchUiState(UiStates.UiWinMenu); }

    private void OnEnable()
    {
        BoltGameManager.Bolt_GameOver += EnableVisualizationMenu;
        BoltGameManager.Bolt_EndVisualization += EnableVisualizationMenu;
        BoltGameManager.Bolt_GameWon += EnableWinMenu;
    }

    private void OnDisable()
    {
        BoltGameManager.Bolt_GameOver -= EnableVisualizationMenu;
        BoltGameManager.Bolt_EndVisualization -= EnableVisualizationMenu;
        BoltGameManager.Bolt_GameWon -= EnableWinMenu;
    }
}
