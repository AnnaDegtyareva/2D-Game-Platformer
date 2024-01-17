using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerMove : MonoBehaviour
{
    PhotonView pv;
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            float H = Input.GetAxis("Horizontal");
            rb.AddForce(new Vector2(H * speed, 0)); 
        }
    }
}
