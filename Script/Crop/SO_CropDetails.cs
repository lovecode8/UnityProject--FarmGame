using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropDetailsList", menuName = "ScriptableObjects/CropDetailsList")]
public class SO_CropDetails : ScriptableObject
{
    [SerializeField]
    public List<CropDetails> cropDetails;
    
    public CropDetails GetCropDetails(int id)
    {
        return cropDetails.Find(x => x.seedId == id);
    }
}
