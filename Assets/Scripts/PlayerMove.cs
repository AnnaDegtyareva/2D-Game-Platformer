using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerMove : MonoBehaviour, IPunObservable
{
    PhotonView pv;
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    [SerializeField] GameObject camera;

    public float GunRorationAngle = 0f;

    public GameObject Gun;
    [HideInInspector] public bool isGround;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        if (!pv.IsMine)
        {
            camera.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            float H = Input.GetAxis("Horizontal");
            rb.AddForce(new Vector2(H * speed, 0));
        }
        
    }
    private void Update()
    {
        if (pv.IsMine)
        {
            Vector2 center = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 scope = Input.mousePosition;
            Vector2 dir = new Vector2(scope.x - center.x, scope.y - center.y).normalized;

            GunRorationAngle = GetAngle(dir);

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {
                if (isGround)
                {
                    pv.RPC("Jump", RpcTarget.All);
                }
            }
        }

        Gun.transform.rotation = Quaternion.Euler(0, 0, GunRorationAngle);
    }
    [PunRPC]
    public void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce);
    }
    public float GetAngle(Vector2 V)
    {
        Vector2 correct = V.normalized;
        float Angle = Mathf.Acos(correct.x) * Mathf.Rad2Deg;
        if (correct.y < 0)
        {
            Angle = -Angle;
        }
        return Angle;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GunRorationAngle);
        }
        else
        {
            GunRorationAngle = (float)stream.ReceiveNext();
        }
    }
}
