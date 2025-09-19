using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundList", menuName = "ScriptableObjects/SoundList")]
public class SO_Sound : ScriptableObject
{
    [SerializeField]
    public List<Sound> soundList;
}
