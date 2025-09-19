using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//动物圈管理面板
public class AnimalPanel : MonoBehaviour
{
    public TextMeshProUGUI animalTitleText;
    public TextMeshProUGUI animalCountText;
    public TextMeshProUGUI animalFeedCountText;
    public TextMeshProUGUI harvestCountText;
    public TextMeshProUGUI canKillAnimalsCountText;
    public Button buyAnimalButton;
    public Button buyFeedButton;
    public Button harvestButton;
    public Button killButton;
    public Image feedSprite;
    public Image harvestSprite;
    public Image killSprite;

    void Start()
    {
        buyAnimalButton.onClick.AddListener(BuyAnimal);
        buyFeedButton.onClick.AddListener(BuyAnimalFeed);        
        harvestButton.onClick.AddListener(HarvestAnimal);        
        killButton.onClick.AddListener(KillAnimal);        
    }

    private void BuyAnimal()
    {
        EventHolder.CallonBuyAnimalEvent();
    }

    private void BuyAnimalFeed()
    {
        EventHolder.CallonBuyAnimalFeedEvent();
    }

    private void HarvestAnimal()
    {
        EventHolder.CallonHarvestAnimalEvent();
    }

    private void KillAnimal()
    {
        EventHolder.CallonKillAnimalEvent();
    }

    public void ShowAnimalPanel(AnimalSlotDetails animalSlotDetails)
    {

        animalTitleText.SetText(animalSlotDetails.animalDetails.animalName);

        animalCountText.SetText(Setting.countText + animalSlotDetails.animalCount.ToString() +
        "/" + Setting.animalSlotMaxCount);

        animalFeedCountText.SetText(Setting.feedText + animalSlotDetails.feedCount.ToString());

        harvestCountText.SetText(Setting.harvestCountText +
        animalSlotDetails.harvestCount.ToString());

        canKillAnimalsCountText.SetText(Setting.rawText + animalSlotDetails.
        canKillAnimalsCount.ToString() + "/" + animalSlotDetails.animalCount.ToString());

        feedSprite.sprite = InventoryManager.Instance.
        GetItemDetails(animalSlotDetails.animalDetails.feedItemId).itemSprite;
        harvestSprite.sprite = InventoryManager.Instance.
        GetItemDetails(animalSlotDetails.animalDetails.harvestItemId).itemSprite;
        killSprite.sprite = InventoryManager.Instance.
        GetItemDetails(animalSlotDetails.animalDetails.beKillItemId).itemSprite;
    }
}
