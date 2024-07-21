using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public enum SCREEN_TYPE
    {
        MAIN_MENU = 0,
        HOW_TO_PLAY = 1,
        CREDITS = 2,
        GAMEPLAY = 3,
        PAUSE = 4,
        GAME_OVER_VICTORY = 5,
        GAME_OVER_LOSE = 6,
        EMPTY = 7
    }
    [System.Serializable]
    public struct ScreenRef
    {
        public SCREEN_TYPE screenType;
        public GameObject screenPrefab;
    }
    public SCREEN_TYPE currentScreen;
    public List<ScreenRef> screenRefs;
    public void ChangeScreen(SCREEN_TYPE screen)
    {
        var previousScreen = currentScreen;
        currentScreen = screen;
        CloseScreen(previousScreen);
        OpenScreen(currentScreen);
    }

    private void Start()
    {
        CloseAllScreens();
        ChangeScreen(currentScreen);
    }
    public void CloseScreen(SCREEN_TYPE screen)
    {
        foreach (var item in screenRefs)
        {
            if (item.screenType == screen)
            {
                item.screenPrefab.SetActive(false);
            }
        }
    }
    public void OpenScreen(SCREEN_TYPE screen)
    {
        foreach (var item in screenRefs)
        {
            if (item.screenType == screen)
            {
                item.screenPrefab.SetActive(true);
            }
        }
    }
    public void CloseAllScreens()
    {
        foreach (var item in screenRefs)
        {
            item.screenPrefab.SetActive(false);
        }
    }

    public void ChangeScreen(int index)
    {
        ChangeScreen((SCREEN_TYPE)index);
    }

}