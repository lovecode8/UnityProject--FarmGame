using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VFXManager : MonoBehaviour
{
    void OnEnable()
    {
        EventHolder.onDisplayEffect += DisplayEffect;
    }
    void OnDisable()
    {
        EventHolder.onDisplayEffect -= DisplayEffect;
    }

    private void DisplayEffect(Effect effect, Vector3 pos)
    {
        StartCoroutine(Display(effect, pos));
    }

    IEnumerator Display(Effect effect, Vector3 pos)
    {
        GameObject effectPrefab = PoolManager.Instance.GetGameObject((int)effect);
        effectPrefab.SetActive(true);
        effectPrefab.transform.position = pos;
        yield return Setting.effectExistTime;
        PoolManager.Instance.ReturnToPool((int)effect, effectPrefab);
    }
}
