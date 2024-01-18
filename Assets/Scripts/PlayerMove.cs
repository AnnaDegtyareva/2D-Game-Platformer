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

    public float GunRorationAngle;

    public GameObject Gun;
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
    private void Update()
    {
        if (pv.IsMine)
        {
            Vector2 center = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 scope = Input.mousePosition;
            Vector2 dir = new Vector2(scope.x - center.x, scope.y - center.y).normalized;

            GunRorationAngle = GetAngle(dir);
        }     

        Gun.transform.rotation = Quaternion.Euler(0, 0, GunRorationAngle);
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
