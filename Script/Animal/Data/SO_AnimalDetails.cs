using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalDetailsList", menuName = "ScriptableObjects/AnimalDetailsList")]
public class SO_AnimalDetails : ScriptableObject
{
    [SerializeField]
    public List<AnimalDetails> animalDetailsList;

    public AnimalDetails GetAnimalDetails(int id)
    {
        return animalDetailsList.Find(x => x.animalId == id);
    }
}
