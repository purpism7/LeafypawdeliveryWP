using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

using TMPro;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light2D light2d = null;
    [SerializeField] private TextMeshProUGUI timeTMP = null;

    private float _dayLenght = 60f * 1f; // 하루의 길이(초).
    private float _dayIntensity = 1f;
    private float _nightIntensity = 0.4f;

    private Color _dayColor = Color.white;
    private Color _nightColor = new Color(30 / 255f, 130 / 255f, 255 / 255f);

    private float _timeOfDay = 0.55f; // 현재 시간 (0 - 1 범위로, 0은 낮 시작, 1은 다시 밤 시작)
    private int _hours = 0;

    public bool IsNight
    {
        get { return _hours >= 18 && _hours < 24 || _hours >= 0 && _hours < 6; }
    }

    public void Update()
    {
        if (light2d == null)
            return;

        _timeOfDay += Time.deltaTime / _dayLenght;
        if (_timeOfDay > 1f)
            _timeOfDay = 0;

        UpdateLighting();
        UpdateTime();
    }

    private void UpdateLighting()
    {
        if (light2d == null)
            return;

        // 1. 밤 -> 낮 전환 구간 (0.45 ~ 0.55)
        if (_timeOfDay >= 0.45f && _timeOfDay <= 0.55f)
        {
            ChangeToDay();
        }
        // 2. 낮 -> 밤 전환 구간 (0.95 ~ 1.0 또는 0.0 ~ 0.05)
        else if (_timeOfDay >= 0.95f || _timeOfDay <= 0.05f)
        {
            ChangeToNight();
        }
        // 3. 완전한 낮 유지 구간 (0.55 ~ 0.95)
        else if (_timeOfDay > 0.55f && _timeOfDay < 0.95f)
        {
            SetLight(_dayIntensity, _dayColor);
        }
        // 4. 완전한 밤 유지 구간 (0.05 ~ 0.45)
        else
        {
            SetLight(_nightIntensity, _nightColor);
        }
    }

    // 💡 새로운 헬퍼 함수: 전환 중이 아닐 때 빛을 단단히 고정해 줍니다.
    private void SetLight(float intensity, Color color)
    {
        light2d.intensity = intensity;
        light2d.color = color;
    }

    private void ChangeToDay()
    {
        float start = 0.45f;
        float transitionRange = 0.1f; // 0.45 ~ 0.55 (총 0.1의 시간 동안 전환)

        // UpdateLighting에서 이미 구간 검사를 했으므로 if문을 뺄 수 있어 코드가 깔끔해집니다.
        float time = (_timeOfDay - start) / transitionRange;
        LerpLight(_nightIntensity, _dayIntensity, _nightColor, _dayColor, time);
    }

    private void ChangeToNight()
    {
        float start = 0.95f;
        float transitionRange = 0.05f; // 0.95~1.0(0.05) + 0.0~0.05(0.05) = 총 0.1의 시간
        float time = 0;

        if (_timeOfDay >= start)
        {
            // 0.95 ~ 1.0 구간 (진행률의 절반인 0.0 ~ 0.5 비율 적용)
            time = (_timeOfDay - start) / transitionRange * 0.5f;
        }
        else
        {
            // 0.0 ~ 0.05 구간 (나머지 절반인 0.5 ~ 1.0 비율 적용)
            time = 0.5f + (_timeOfDay / transitionRange * 0.5f);
        }

        LerpLight(_dayIntensity, _nightIntensity, _dayColor, _nightColor, time);
    }

    private void LerpLight(float startIntensity, float endIntensity, Color startColor, Color endColor, float time)
    {
        light2d.intensity = Mathf.Lerp(startIntensity, endIntensity, time);
        light2d.color = Color.Lerp(startColor, endColor, time);
    }

    private void UpdateTime()
    {
        string dayNight = IsNight ? "Night" : "Day";

        float time = _timeOfDay * 24f;
        _hours = Mathf.FloorToInt(time);
        string hoursText = _hours < 10 ? $"0{_hours}" : $"{_hours}";

        int minutes = (int)((time - _hours) * 60);
        string minutesText = minutes < 10 ? $"0{minutes}" : $"{minutes}";

        int seconds = (int)(((time - _hours) * 60 - minutes) * 60f);
        string secondsText = seconds < 10 ? $"0{seconds}" : $"{seconds}";

        timeTMP?.SetText($"{dayNight} {hoursText}:{minutesText}:{secondsText}");
    }
}

