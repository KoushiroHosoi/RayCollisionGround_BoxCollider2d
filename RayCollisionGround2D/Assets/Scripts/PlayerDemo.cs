using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //Demo用としてプレイヤーを動かすクラス
    public class PlayerDemo : MonoBehaviour
    {
        private float moveX, moveY;

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float moveSpeed;

        // Update is called once per frame
        void Update()
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(moveX, moveY).normalized * moveSpeed;
        }
    }
}
