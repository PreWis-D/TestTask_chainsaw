using System.Collections;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private SlicedFilledImage _progressLine;
    [SerializeField] private float _maxSpeedFill = 0.5f;
    [SerializeField] private float _minSpeedFill = 0.03f;

    private float _currentValue;
    private float _initialValue;

    public void ChangeValue(float value)
    {
        _initialValue = _currentValue;
        _currentValue = value;

        _progressLine.fillAmount = value;
    }

    public void ChangeSmoothValue(float value)
    {
        _initialValue = _currentValue;
        _currentValue = value;
    }

    private void Update()
    {
        if (_progressLine.fillAmount != _currentValue)
        {
            var t = Mathf.InverseLerp(_initialValue, _currentValue, _progressLine.fillAmount);
            var step = Time.smoothDeltaTime * Mathf.Lerp(_maxSpeedFill, _minSpeedFill, t);

            _progressLine.fillAmount =
                Mathf.MoveTowards(_progressLine.fillAmount, _currentValue, step);
        }
    }
}