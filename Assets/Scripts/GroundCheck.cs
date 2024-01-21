using UnityEngine;
public class GroundCheck : MonoBehaviour
{
    PlayerMove PM;
    private void Awake()
    {
        PM = GetComponentInParent<PlayerMove>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        PM.isGround = true;
        if (collision.gameObject.tag != "Player")
        {
            PM.isGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PM.isGround = false;
        if (collision.gameObject.tag != "Player")
        {
            PM.isGround = false;
        }
    }
}