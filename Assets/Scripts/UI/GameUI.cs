using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameUI : AbstractSingleton<GameUI>
{
    [SerializeField] TextMeshProUGUI KmCounterText;
    [SerializeField] TextMeshProUGUI SpeedText;
    [SerializeField] RectTransform SpeedometerNeedle;
    [SerializeField] RectTransform CarIcon;
    [SerializeField] TextMeshProUGUI CurrentLevelText;
    [SerializeField] TextMeshProUGUI NextLevelText;
    [SerializeField] RectTransform GasPedal;
    [SerializeField] UnityEvent GasDownEvent;
    [SerializeField] UnityEvent GasUpEvent;

    public GameObject GameOverDialog;
    public GameObject PauseDialog;
    public GameObject ReviveButton;

    float _lastMeter = 0;
    float _speedDeltaTime = 0;
    float _speedRefreshRate = 0.2f;
    float _speedKm = 0;
    float _needleZeroRotation = 112.5f;
    float _needleDecadeRotation = 22.5f;
    float _carIconZeroPoint = -100;
    float _lastLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _speedDeltaTime += Time.deltaTime;
        if (_speedDeltaTime > _speedRefreshRate)
        {
            float carIconPosition = _carIconZeroPoint + GameManager.Instance.LevelProcess * 200;
            CarIcon.anchoredPosition = new Vector2(Mathf.Lerp(CarIcon.anchoredPosition.x, carIconPosition, Time.deltaTime * 5), CarIcon.anchoredPosition.y);

            KmCounterText.text = GameManager.Instance.MeterCounter.ToString("N0");
            _speedKm = (GameManager.Instance.MeterCounter - _lastMeter) * (1 / _speedRefreshRate) * 60 * 60 / 1000;
            SpeedText.text = _speedKm.ToString("N0");
            float needleRotation = _needleZeroRotation - (Mathf.Round(_speedKm) / 10) * _needleDecadeRotation;
            SpeedometerNeedle.rotation = Quaternion.Euler(0, 0, needleRotation);

            _lastMeter = GameManager.Instance.MeterCounter;
            _speedDeltaTime -= _speedRefreshRate;
        }

        if (GameManager.Instance.Level == _lastLevel) return;
        CurrentLevelText.text = GameManager.Instance.Level.ToString();
        NextLevelText.text = (GameManager.Instance.Level + 1).ToString();
        _lastLevel = GameManager.Instance.Level;
    }

    public void GasDown()
    {
        GasPedal.anchoredPosition = new Vector3(15, -15, 0);
        GasDownEvent.Invoke();
    }

    public void GasUp()
    {
        GasPedal.anchoredPosition = new Vector3(0, 0, 0);
        GasUpEvent.Invoke();
    }
}
