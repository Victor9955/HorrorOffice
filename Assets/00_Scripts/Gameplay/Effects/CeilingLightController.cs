using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CeilingLightController : MonoBehaviour
{
    [Serializable]
    public struct LightRow
    {
        public List<MeshRenderer> lightsInRowMeshes;
    }

    [SerializeField] private Light _topLight;
    [SerializeField] private Light _topLight2;
    [SerializeField] private Light _topLight3;
    [SerializeField] private List<LightRow> _lightRows;
    [Space(5)]
    [SerializeField] private Material _lightOffMat;
    [Space(5)]
    [Header("Delay Settings")]
    [SerializeField] private float _offStartDelay;
    [SerializeField] private float _lightOffDelay;

    private bool turnedOffLights;
    public void TurnOffLights()
    {
        if (!turnedOffLights) StartCoroutine(LightsOffRoutine());
    }

    private IEnumerator LightsOffRoutine()
    {
        turnedOffLights = true;
        int totalLights = 0;
        Debug.Log("Start lights off");
        foreach (var lightrow in _lightRows)
        {
            totalLights += lightrow.lightsInRowMeshes.Count;
        }

        yield return new WaitForSeconds(_offStartDelay);
        DOVirtual.Float(_topLight.intensity, 0, _lightRows.Count * _lightOffDelay, (intens) => _topLight.intensity = intens);
        DOVirtual.Float(_topLight2.intensity, 0, _lightRows.Count * _lightOffDelay, (intens) => _topLight2.intensity = intens);
        DOVirtual.Float(_topLight3.intensity, 0, _lightRows.Count * _lightOffDelay, (intens) => _topLight3.intensity = intens);
        for (int i = _lightRows.Count - 1; i > 0; i--)
        {
            Debug.Log("lights row n°" + i);
            int lightsInRowCount = _lightRows[i].lightsInRowMeshes.Count;
            for (int j = 0; j < lightsInRowCount; j++)
            {
                _lightRows[i].lightsInRowMeshes[j].material = _lightOffMat;
            }
            // play a sound
            yield return new WaitForSeconds(0.5f);
        }
        Utils.BigText("Lights all off");
    }

}
