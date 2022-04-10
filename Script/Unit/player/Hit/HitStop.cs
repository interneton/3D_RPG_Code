using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    bool _stop;
    [SerializeField] float _stopTime;

    [SerializeField] Transform _shakeCam;
    [SerializeField] Vector3 _shake;

    // 히트 시 시간 정지시키기
    public void StopTime()
    {
        if (!_stop)
        {
            _stop = true;
            _shakeCam.localPosition += _shake;
            Time.timeScale = 0;

            StartCoroutine("ReturnTimeScale");
        }
    }


    // 다시 재개하기
    IEnumerator ReturnTimeScale()
    {
        yield return new WaitForSecondsRealtime(_stopTime);

        Time.timeScale = 1;

        _stop = false;
    }
}
