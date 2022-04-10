using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderLerp : MonoBehaviour
{
    public Slider _Front_Slider;
    public Slider _Back_Slider;

    [SerializeField] float _lerpSpeed = 2.0f;
    

    // �ڷ�ƾ ����
    public void LerpSlider()
   {
        StartCoroutine("_LerpStart");
    }

    // �ڷ�ƾ ����
    private void OnDisable()
    {
        StopCoroutine("_LerpStart");
    }

    // ���� Front �����̴��� Back �����̴� Value ���� ����� �ֱ�
    IEnumerator _LerpStart()
    {
        while (_Back_Slider.value >= _Front_Slider.value)
        {
            _Back_Slider.value = Mathf.Lerp(_Back_Slider.value, _Front_Slider.value, _lerpSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
