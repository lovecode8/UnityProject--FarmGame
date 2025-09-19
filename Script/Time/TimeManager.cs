using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

//定义规则--0.012秒为游戏1秒，1个季节固定30天，
public class TimeManager : SingletonMonoBehavior<TimeManager>
{
    private int gameSecond = 0;
    private int gameMinute = 30;
    private int gameHour = 6;
    private int gameDay = 1;
    private Season gameSeason = Season.spring;
    private int gameYear = 1;
    private float timer = 0f;
    private bool timePurse = false;
    void OnEnable()
    {
        EventHolder.onLoadDataEvent += LoadGameDay;
    }
    void OnDisable()
    {
        EventHolder.onLoadDataEvent -= LoadGameDay;
    }
    void Update()
    {
        if (!timePurse)
        {
            TimeGoing();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestTimeOneHour(5);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            TestTimeMintue(30);
        }
    }
    private void TestTimeOneHour(int hour)
    {
        gameHour += hour;
    }
    private void TestTimeMintue(int min)
    {
        gameMinute += min;
    }
    private void TimeGoing()
    {
        timer += Time.deltaTime;
        if (timer >= Setting.gameSecond)
        {
            TimeTick();
            timer -= Setting.gameSecond;
        }
    }
    private void TimeTick()
    {
        gameSecond++;
        if (gameSecond > 59)
        {
            gameSecond = 0;
            gameMinute++;
            if (gameMinute > 59)
            {
                gameMinute = 0;
                gameHour++;
                if (gameHour > 23f)
                {
                    gameHour = 0;
                    gameDay++;
                    if (gameDay > 29f)
                    {
                        gameDay = 1;
                        int gs = (int)gameSeason;
                        gs++;
                        gameSeason = (Season)gs;
                        if (gs > 3)
                        {
                            gs = 0;
                            gameSeason = (Season)gs;
                            gameYear++;
                            if (gameYear > 9999)
                            {
                                gameYear = 1;
                            }
                        }
                    }
                    EventHolder.CallonGameDayEvent(gameYear, gameSeason, gameDay, gameHour, gameMinute);
                }
            }
            EventHolder.CallonGameMintueEvent(gameYear, gameSeason, gameDay, gameHour, gameMinute);
        }
    }
    public int GetCurrentDay()
    {
        return gameDay;
    }
    private void LoadGameDay(SaveData data)
    {
        gameDay = data.gameDay;
    }
}