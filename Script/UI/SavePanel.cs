using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SavePanel : MonoBehaviour
{
    public Button openSavePanelButton;
    public Button closeButton;
    public Button selectLoadButton;
    public Button selectSaveButton;
    public GameObject loadPanel;
    public GameObject savePanel;
    public InputField loadPassWord;
    public InputField savePassWord;
    public Button decideButton;
    private bool isSelectLoad; //是否是在加载存档页面
    RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        isSelectLoad = true;
        openSavePanelButton.onClick.AddListener(OpenSavePanel);
        closeButton.onClick.AddListener(CloseSavePanel);
        selectLoadButton.onClick.AddListener(SelectLoadData);
        selectSaveButton.onClick.AddListener(SelectSaveData);
        decideButton.onClick.AddListener(ProcessDataControl);
    }
    private void OpenSavePanel()
    {
        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x -
        Setting.saveDataPanelMoveDistance, Setting.panelMoveDuration).SetEase(Ease.OutBack);
    }
    private void CloseSavePanel()
    {
        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x +
        Setting.saveDataPanelMoveDistance, Setting.panelMoveDuration).SetEase(Ease.OutBack);
    }
    private void SelectLoadData()
    {
        if (!isSelectLoad)
        {
            loadPanel.SetActive(true);
            savePanel.SetActive(false);
            isSelectLoad = true;
        }
    }
    private void SelectSaveData()
    {
        if (isSelectLoad)
        {
            savePanel.SetActive(true);
            loadPanel.SetActive(false);
            isSelectLoad = false;
        }
    }
    private void ProcessDataControl()
    {
        if (isSelectLoad && loadPassWord.text == "" || !isSelectLoad && savePassWord.text == "")
        {
            EventHolder.CallonShowAdmireEvent("请输入存档密码");
            return;
        }
        if (isSelectLoad)
            {
                int password = int.Parse(loadPassWord.text);
                SaveSystem.Instance.LoadGameData(password);
            }
            else
            {
                int password = int.Parse(savePassWord.text);
                SaveSystem.Instance.SaveGameData(password);
            }
    }
}
