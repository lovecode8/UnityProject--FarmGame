using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Light dayLight;
    public Light nightLight;
    public Light moon;
    private StateInDay stateInDay = StateInDay.day;
    private int countForG = 0;
    private int countForB = 0;
    private Color dayLightColor;
    private Quaternion dayLightRotation;
    private Vector3 moonLightPosition;
    void Start()
    {
        dayLightColor = dayLight.color;
        dayLightRotation = dayLight.transform.rotation;
        moonLightPosition = moon.transform.position;
    }
    void OnEnable()
    {
        EventHolder.onGameMintueEvent += MoveLight;
    }
    void OnDisable()
    {
        EventHolder.onGameMintueEvent -= MoveLight;
    }
    private void MoveLight(int year, Season season, int day, int hour, int mintue)
    {
        if (stateInDay == StateInDay.day)
        {
            MoveDayLight(hour, mintue);
        }
        else if (stateInDay == StateInDay.dayToNight)
        {
            SwitchDayNight(true, hour, mintue);
        }
        else if (stateInDay == StateInDay.nightToDay)
        {
            SwitchDayNight(false, hour, mintue);
        }
        else if (stateInDay == StateInDay.night)
        {
            MoveMoonLight(hour, mintue);
        }
    }
    private void MoveDayLight(int hour, int mintue)
    {
        if (hour == 17 && mintue >= 30)
        {
            //切换黑夜
            stateInDay = StateInDay.dayToNight;
            return;
        }
        countForB++;
        countForG++;
        if (countForB >= Setting.dayLightChangeMintueCountB)
        {
            countForB = 0;
            dayLightColor.b -= Setting.dayLightChangeMintueColor;
            dayLight.color = dayLightColor;
        }
        if (countForG >= Setting.dayLightChangeMintueCountG)
        {
            countForG = 0;
            dayLightColor.g -= Setting.dayLightChangeMintueColor;
            dayLight.color = dayLightColor;
        }
        dayLightRotation = Quaternion.Euler(dayLightRotation.eulerAngles.x,
        dayLightRotation.eulerAngles.y - Setting.dayLightRotatePerMintue,
        dayLightRotation.eulerAngles.z);
        dayLight.transform.rotation = dayLightRotation;
        // Debug.Log(dayLightColor);
    }
    private void MoveMoonLight(int hour, int mintue)
    {
        Debug.Log("NIGHT");
        if (hour == 5 && mintue >= 30)
        {
            //切换白天
            stateInDay = StateInDay.nightToDay;
            return;
        }
        moonLightPosition.z -= Setting.moonLightChangeMintuePosZ;
        moon.transform.position = moonLightPosition;
    }
    private void SwitchDayNight(bool toNight, int hour, int mintue)
    {
        if (toNight)
        {
            dayLight.intensity -= Setting.SwitchDayLightPerNum;
            nightLight.intensity += Setting.SwitchNightLightPerNum;
            moon.intensity += Setting.SwitchMoonLightPerNum;
            if (hour == 18 && mintue >= 30)
            {
                nightLight.intensity = 0.8f;
                moon.intensity = 0.3f;
                dayLight.intensity = 0;
                dayLightRotation = Setting.dayLightOriginalRotation;
                dayLightColor.g = Setting.dayLightOriginalColorG;
                dayLightColor.b = Setting.dayLightOriginalColorB;
                stateInDay = StateInDay.night;
            }
        }
        else
        {
            dayLight.intensity += Setting.SwitchDayLightPerNum;
            nightLight.intensity -= Setting.SwitchNightLightPerNum;
            moon.intensity -= Setting.SwitchMoonLightPerNum;
            if (hour == 6 && mintue >= 30)
            {
                nightLight.intensity = 0;
                moon.intensity = 0;
                dayLight.intensity = 1;
                moonLightPosition.z = Setting.moonLightOriginalPosZ;
                stateInDay = StateInDay.day;
            }
        }
    }
}
