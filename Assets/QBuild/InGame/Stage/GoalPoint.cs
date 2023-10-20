using System;
using QBuild.Part;
using QBuild.Player.Controller;
using UnityEngine;
using VContainer;

namespace QBuild.Stage
{
    public class GoalPoint : MonoBehaviour
    {
        private void Awake()
        {
            if (transform.position.x % 1 != 0 || transform.position.y % 1 != 0 || transform.position.z % 1 != 0)
            {
                transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y),
                    Mathf.Round(transform.position.z));
            }
        }

        private void OnCollisionEnter(Collision other)
        {
        }
    }
}