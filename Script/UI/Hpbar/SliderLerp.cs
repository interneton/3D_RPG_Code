using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderLerp : MonoBehaviour
{
    public Slider _Front_Slider;
    public Slider _Back_Slider;

    [SerializeField] float _lerpSpeed = 2.0f;
    

    // 코루틴 실행
    public void LerpSlider()
   {
        StartCoroutine("_LerpStart");
    }

    // 코루틴 중지
    private void OnDisable()
    {
        StopCoroutine("_LerpStart");
    }

    // 현재 Front 슬라이더랑 Back 슬라이더 Value 같게 만들어 주기
    IEnumerator _LerpStart()
    {
        while (_Back_Slider.value >= _Front_Slider.value)
        {
            _Back_Slider.value = Mathf.Lerp(_Back_Slider.value, _Front_Slider.value, _lerpSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
