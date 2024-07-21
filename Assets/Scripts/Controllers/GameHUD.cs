using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WTF.Configs;
using WTF.Events;
using WTF.Models;
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
        EventDispatcher<SCreepsExplodeInfo>.Register(CustomEvents.CreepsExploded, OnCreepsExploded);
        EventDispatcher<bool>.Register(CustomEvents.GameWin, ShowWinScreen);
        EventDispatcher<bool>.Register(CustomEvents.GameLose, ShowLoseScreen);
    }

    private void OnDisable()
    {
        EventDispatcher<Creep>.Unregister(CustomEvents.CreepSelected, OnCreepSelected);
        EventDispatcher<Creep>.Unregister(CustomEvents.CreepUnselected, OnCreepUnselected);
        EventDispatcher<SCreepsExplodeInfo>.Unregister(CustomEvents.CreepsExploded, OnCreepsExploded);
        EventDispatcher<bool>.Unregister(CustomEvents.GameWin, ShowWinScreen);
        EventDispatcher<bool>.Unregister(CustomEvents.GameLose, ShowLoseScreen);
    }

    private void ShowWinScreen()
    {
        innerLoopScreenController.ChangeScreen(ScreenController.SCREEN_TYPE.GAME_OVER_VICTORY);
    }

    private void ShowLoseScreen()
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

    private void OnCreepsExploded(SCreepsExplodeInfo creepInfo)
    {
        if (creepInfo.creepType == CreepTypes.Player)
        {
            leftProgressWidget.UpdateProgress(creepInfo.creepCount);
        }
        else
        {
            rightProgressWidget.UpdateProgress(creepInfo.creepCount);
        }
    }
}
