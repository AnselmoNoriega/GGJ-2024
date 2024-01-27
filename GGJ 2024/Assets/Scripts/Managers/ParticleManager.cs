using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private GameObject _gasEffectHolder;
    
    public IEnumerator ActivateGasEffect(float effectDuration)
    {
        _gasEffectHolder.SetActive(true);
        yield return new WaitForSeconds(effectDuration);
        _gasEffectHolder.SetActive(false);
    }
}
