using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.CasualGames.SwoopGame
{
    [CreateAssetMenu(fileName ="GemData",menuName ="ScriptableObjects/GemData")]
    public class GemData : ScriptableObject
    {
        public string _ObjectName;

        public int _Score;

        public float _Fuel;
    }
}