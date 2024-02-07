using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.CasualGames.SwoopGame
{
    public class Fuel : MonoBehaviour
    {
        public Slider _bar;

        public static Fuel Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void SetFuelValue(bool refill,float value)
        {
            if(refill)
            {
                _bar.maxValue = value;
            }
            else
            {
                _bar.value = value;
            }
            
        }

    }
}
