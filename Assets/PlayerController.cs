using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (!photonView.IsMine)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            HandleMovement();
            HandleJump();
        }
    }

    private void HandleMovement()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void ChangeSize(float multiplier)
    {
        // Change the player's size
        transform.localScale *= multiplier;

        // Optional: Adjust other properties like moveSpeed or jumpForce based on size
        moveSpeed *= multiplier;
        jumpForce *= multiplier;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(rb.velocity);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            rb.velocity = (Vector2)stream.ReceiveNext();
        }
    }
}

