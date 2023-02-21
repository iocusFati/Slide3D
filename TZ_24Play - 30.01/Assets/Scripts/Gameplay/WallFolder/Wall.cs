using System;
using UnityEngine;

namespace Gameplay.WallFolder
{
    public class Wall : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            // Debug.Log(collision.gameObject.name);
            // if (collision.gameObject.CompareTag("Block"))
            // {
            //     collision.transform.SetParent(null);
            // }
        }
    }
}