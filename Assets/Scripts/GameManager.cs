using UnityEngine;

public class GameManager : AbstractSingleton<GameManager>
{
    public GameObject HomeUI;
    public GameUI GameUI;

    public static float MaxScore = 0;
    public float MeterCounter => PlayerController.Instance.MyVehicle.transform.position.z;
    public static float LevelLimit = 500;
    public float Level => Mathf.Floor(MeterCounter / LevelLimit) + 1;
    public float LevelProcess => (MeterCounter % LevelLimit) / LevelLimit;
    int _reviveCount = 0;
    int _maxRevive = 1;

    public enum Statuses
    {
        HOME,
        GAME,
        PAUSE,
        GAMEOVER
    }
    public Statuses Status = Statuses.HOME;

    void Start()
    {
        Application.targetFrameRate = 60;
        SetHomeStatus();
    }

    public void SetHomeStatus()
    {
        Status = Statuses.HOME;
        HomeUI.SetActive(true);
        GameUI.gameObject.SetActive(false);
        PlayerController.Instance.ResetMyVehicle();
        LevelManager.Instance.ResetLevel();
        CameraFollow.Instance.SetState(new CameraHomeState());
    }

    public void GameStart()
    {
        Status = Statuses.GAME;
        HomeUI.SetActive(false);
        GameUI.gameObject.SetActive(true);
        CameraFollow.Instance.SetState(new CameraGameState());
        LevelManager.Instance.Traffic.SpawnForDistance(200);
        PlayerController.Instance.StartDrive();
        _reviveCount = 0;
    }

    public void GameOver()
    {
        Status = Statuses.GAMEOVER;
        SoundManager.Instance.PlayCrashSound();
        Time.timeScale = 0;
        GameUI.GameOverDialog.SetActive(true);
        GameUI.ReviveButton.SetActive(_reviveCount < _maxRevive);
        SaveMyScore();
    }

    void SaveMyScore()
    {
        if (MeterCounter <= MaxScore) return;
        AuthManager.Instance.SaveMyScore(MeterCounter);
    }

    public void PauseGame()
    {
        Status = Statuses.PAUSE;
        Time.timeScale = 0;
        GameUI.PauseDialog.SetActive(true);
    }

    public void ResumeGame()
    {
        Status = Statuses.GAME;
        Time.timeScale = 1;
        GameUI.PauseDialog.SetActive(false);
    }

    public void ReviveButton()
    {
#if UNITY_EDITOR
        ReviveGame();
#else
        AdsManager.Instance.ShowRewardedAd(ReviveGame);
#endif
    }

    public void ReviveGame()
    {
        Status = Statuses.GAME;
        Time.timeScale = 1;
        GameUI.GameOverDialog.SetActive(false);
        PlayerController.Instance.Revive();
        _reviveCount++;
    }

    public void RestartGame()
    {
        Status = Statuses.GAME;
        LevelManager.Instance.ResetLevel();
        LevelManager.Instance.Traffic.SpawnForDistance(200);
        PlayerController.Instance.ResetMyVehicle();
        PlayerController.Instance.StartDrive();
        _reviveCount = 0;

        Time.timeScale = 1;
    }

    public void GoHome()
    {
        Time.timeScale = 1;
        SaveMyScore();
        SetHomeStatus();
    }
}
