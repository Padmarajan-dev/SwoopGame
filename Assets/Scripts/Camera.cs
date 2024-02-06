using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.CasualGames.SwoopGame
{
    public class Camera : MonoBehaviour
    {
        public Transform target;
        // Height of the camera relative to the plane
        public Vector3 Offset;
        void LateUpdate()
        {
            //Calculate the desired position of the camera
            //transform.RotateAround(target.transform.position, Vector3.down, 30 * Time.deltaTime);

            // Update the camera position
            transform.position = target.transform.localPosition + Offset;

            // Make the camera look at the plane's position (ignoring forward direction)
            transform.LookAt(target);
        }
    }
}
