using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace com.CasualGames.SwoopGame
{
    public class ScoreCard : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_GameScoreText;

        [SerializeField] TextMeshProUGUI m_MenuScoreText;
        void Start()
        {
            Plane._UpdateScore += UpdateScore;
        }
        //update score from plane
        public void UpdateScore(int score)
        {
            m_GameScoreText.text = m_MenuScoreText.text = score.ToString();
        }

        private void OnDestroy()
        {
            Plane._UpdateScore -= UpdateScore;
        }
    }
}