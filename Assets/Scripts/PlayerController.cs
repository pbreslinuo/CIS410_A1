using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;
    public Text jumpText;

    private Rigidbody rb;
    private int count;
    private bool canJump;      // detects getting the powerup
    private bool onGround;     // detects being on the ground
    private bool dblJump;      // keeps track of double jump usage
    private float speedOffset; // make it harder to turn while in the air

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
        jumpText.text = "";
        canJump = false;
        onGround = true;
        dblJump = false;
        speedOffset = 1.0f;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed * speedOffset);
        bool space = Input.GetKeyDown("space");
        if (space & canJump & (onGround | dblJump))
        {
            Vector3 jump = new Vector3(0.0f, 7.0f - rb.velocity.y, 0.0f);
            rb.AddForce(jump, ForceMode.VelocityChange);
            if (onGround == false)
            {
                dblJump = false;
            }
        }
    }

    void OnCollisionExit(Collision other)
    { // detects when the ball leaves the ground
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = false;
            speedOffset = 0.4f;
        }
    }

    void OnCollisionStay(Collision other)
    { // maintain these values when ball stays on ground
        if (other.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            dblJump = true;
            speedOffset = 1.0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
            jumpText.text = ""; // get rid of double jump instructions when you collect the next point
        }
        if (other.gameObject.CompareTag("Powerup"))
        {
            other.gameObject.SetActive(false);
            jumpText.text = "You can double jump!\n\n\n\n\n     Don't fall out!";
            canJump = true;
        }
        if (other.gameObject.CompareTag("Gameover"))
        {
            winText.text = "";
            jumpText.text = "You jumped too far!\n\n\n\n\n   Game resetting.";
        }
        if (other.gameObject.CompareTag("Reset"))
        { // learned* this from the StackOverflow user "Programmer"    *i.e. copied and pasted
            string scene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }

    void SetCountText()
    {
        countText.text = "Points: " + count.ToString();
        if (count >= 16)
        {
            winText.text = "      You win!\n\n\n\n\nJump out to reset.";
        }
    }
}