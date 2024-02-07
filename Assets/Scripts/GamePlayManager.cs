using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.CasualGames.SwoopGame
{
    public class GamePlayManager : MonoBehaviour
    {
        [SerializeField] GameObject m_Menu;

        [SerializeField] GameObject m_MenuPlay;

        [SerializeField] GameObject m_PausePlay;
        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 1;
            if(m_Menu != null)
            {
                m_MenuPlay.SetActive(false);
            }
            Plane._PlaneCrashedAction += ResetGame;

        }

        public void Update()
        {
            AudioManager.Instance.PlayMainmenuAudio();
        }

        public void ResetGame()
        {
            StartCoroutine(Reload());
        }
        public IEnumerator Reload()
        {
            m_MenuPlay?.SetActive(true);
            m_PausePlay?.SetActive(false);
            yield return new WaitForSeconds(1f);
            Time.timeScale = 0;
            m_Menu.SetActive(true);
        }
        void OnDestroy()
        {
            Plane._PlaneCrashedAction -= ResetGame;
        }

        public void loadScene(int id)
        {
            SceneManager.LoadScene(id);
        }

        public void PauseGame()
        {
            m_Menu?.SetActive(true);
            m_MenuPlay?.SetActive(false);
            m_PausePlay?.SetActive(true);
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            m_Menu?.SetActive(false);
            Time.timeScale = 1;
        }
    }
}