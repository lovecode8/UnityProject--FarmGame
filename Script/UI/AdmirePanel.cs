using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AdmirePanel : MonoBehaviour
{
    public Text admireContextText;
    RectTransform rectTransform;
    private bool isAdmirePanelShowing;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void OnEnable()
    {
        EventHolder.onShowAdmireEvent += ShowAdmirePanel;
    }
    void OnDisable()
    {
        EventHolder.onShowAdmireEvent -= ShowAdmirePanel;
    }
    private void ShowAdmirePanel(string context)
    {
        if (isAdmirePanelShowing)
        {
            return;
        }

        isAdmirePanelShowing = true;
        admireContextText.text = context;
        AudioManager.Instance.PlaySound("admire");
        StartCoroutine(ShowAndHideAdmirePanel());
    }
    IEnumerator ShowAndHideAdmirePanel()
    {
        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x +
        Setting.admirePanelMoveDistance, Setting.panelMoveDuration).SetEase(Ease.OutBack);

        yield return Setting.admirePanelShowTime;

        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x -
        Setting.admirePanelMoveDistance, Setting.panelMoveDuration).SetEase(Ease.OutBack);

        isAdmirePanelShowing = false;
    }
}
