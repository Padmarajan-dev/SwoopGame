using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.CasualGames.SwoopGame
{
    public class GamePlayManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Plane._PlaneCrashed += ResetGame;
        }

        public void ResetGame()
        {
            StartCoroutine(Reload());
        }
        public IEnumerator Reload()
        {
            print("Relaoad scene triggered");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(0);
        }
        void OnDestroy()
        {
            Plane._PlaneCrashed -= ResetGame;
        }
    }
}