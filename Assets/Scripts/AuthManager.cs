using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using UnityEngine.SocialPlatforms;

public class AuthManager : AbstractSingleton<AuthManager>
{
    string _leaderboardID = "CgkIm8iF1LgCEAIQAQ";

    void Start()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate(AuthenticationCompleted);
    }

    void AuthenticationCompleted(bool success)
    {
        if (!success) return;
        GetScore();
    }

    void GetScore()
    {
        PlayGamesPlatform.Instance.LoadScores(
            _leaderboardID,
            LeaderboardStart.PlayerCentered,
            1,
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,(data)=> {
                if (data.PlayerScore == null) return;
                GameManager.MaxScore = data.PlayerScore.value;
            });
    }

    public void SaveMyScore(float score)
    {
        Social.ReportScore((long)score, _leaderboardID, (bool success) => {
            if (!success)
            {
                // show alert
            }
        });
    }
}
