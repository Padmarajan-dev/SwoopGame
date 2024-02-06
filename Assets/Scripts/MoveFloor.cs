using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace com.CasualGames.SwoopGame
{
    public class MoveFloor : MonoBehaviour
    {


        public float moveSpeed;

        private void Start()
        {
        }
        // Update is called once per frame
        void Update()
        {
            //this.transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }
}
