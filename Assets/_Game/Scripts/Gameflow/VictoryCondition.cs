using System;
using UnityEngine;

public class VictoryCondition : GameflowBehavior
{
    public static Action OnVictoryConditionMet;

    [SerializeField] private float m_delayBeforeTriggerVictory = 1f;

    private float m_targetScore;
    private float m_currentScore;
    private bool m_canTrackVictory;

    protected override void OnEnable()
    {
        base.OnEnable();
        Manager_Score.OnSendCurrentScore += OnSendCurrentScore;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Manager_Score.OnSendCurrentScore -= OnSendCurrentScore;
    }


    protected override void OnGameplay()
    {
        base.OnGameplay();
        m_canTrackVictory = true;
    }

    protected override void OnVictory()
    {
        base.OnVictory();
        m_canTrackVictory = false;
    }

    protected override void OnGameover()
    {
        base.OnGameover();
        m_canTrackVictory = false;
    }

    private void OnSendTargetScoreToReach(float targetScore)
    {
        m_targetScore = targetScore;
    }

    private void OnSendCurrentScore(float currentScore)
    {
        if (m_canTrackVictory == false)
            return;

        m_currentScore = currentScore;

        if (m_currentScore >= m_targetScore)
        {
            m_canTrackVictory = false;
            Invoke(nameof(TriggetVictory), m_delayBeforeTriggerVictory);
        }
    }

    private void TriggetVictory()
    {
        OnVictoryConditionMet?.Invoke();
    }
}