using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WTF.Configs;
using WTF.Events;
using WTF.Players;

public class GameHUD : MonoBehaviour
{
    public HUDPlayerProgressWidget leftProgressWidget;
    public HUDPlayerProgressWidget rightProgressWidget;
    public Button pauseButton;

    public ScreenController innerLoopScreenController;

    private void OnEnable()
    {
        EventDispatcher<Creep>.Register(CustomEvents.CreepSelected, OnCreepSelected);
        EventDispatcher<Creep>.Register(CustomEvents.CreepUnselected, OnCreepUnselected);
    }

    private void OnDisable()
    {
        EventDispatcher<Creep>.Unregister(CustomEvents.CreepSelected, OnCreepSelected);
        EventDispatcher<Creep>.Unregister(CustomEvents.CreepUnselected, OnCreepUnselected);
    }

    [ContextMenu("Show Win Screen")]
    public void ShowWinScreen()
    {
        innerLoopScreenController.ChangeScreen(ScreenController.SCREEN_TYPE.GAME_OVER_VICTORY);
    }
    [ContextMenu("Show lose Screen")]
    public void ShowLoseScreen()
    {
        innerLoopScreenController.ChangeScreen(ScreenController.SCREEN_TYPE.GAME_OVER_LOSE);
    }

    private void OnCreepSelected(Creep creep)
    {
        if (creep.creepType == CreepTypes.Player)
        {
            leftProgressWidget.UpdateProgress(-1);
        }
        else
        {
            rightProgressWidget.UpdateProgress(-1);
        }
    }

    private void OnCreepUnselected(Creep creep)
    {
        if (creep.creepType == CreepTypes.Player)
        {
            leftProgressWidget.UpdateProgress(1);
        }
        else
        {
            rightProgressWidget.UpdateProgress(1);
        }
    }
}
