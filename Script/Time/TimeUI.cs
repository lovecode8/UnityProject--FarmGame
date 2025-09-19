using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeText;
    void OnEnable()
    {
        EventHolder.onGameMintueEvent += UpdateTimeUI;
    }
    void OnDisable()
    {
        EventHolder.onGameMintueEvent -= UpdateTimeUI;
    }
    private void UpdateTimeUI(int year, Season season, int day, int hour, int mintue)
    {
        string hourZero = "";
        string mintueZero = "";
        if (hour < 10) hourZero = "0";
        if (mintue < 10) mintueZero = "0";
        string time = hourZero + hour + ":" + mintueZero + mintue;
        dayText.SetText(day + "天");
        timeText.SetText(time);
    }
}
