using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
	public Text score;
    public Text level;
    public Text win;

    private Rigidbody2D rb2d;
	private int count;
	private int lives;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

		count = 0;
		setScore();

		lives = 3;
        win.text = "";

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            level.text = "Level 1";
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            level.text = "Level 2";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0);

        rb2d.AddForce(movement * speed);

        transition();

        if (Input.GetKey("escape"))
            Application.Quit();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            setScore();
        }

		if (other.gameObject.CompareTag("Enemy"))
		{
			other.gameObject.SetActive(false);
			lives = lives - 1;
			lifeCounter();

			anim.SetInteger("State", 3);
		}
	}

	void setScore()
	{
		score.text = "Score: " + count.ToString();

		if (count == 4)
		{
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                win.text = "Level Clear";
                SceneManager.LoadScene("Level2");
            }
            else if (SceneManager.GetActiveScene().name == "Level2")
            {
                win.text = "You Win!";
            }
        }
	}

    void lifeCounter()
	{
        if (lives == 2)
		{
			GameObject.FindWithTag("H3").SetActive(false);
		}
		if (lives == 1)
		{
			GameObject.FindWithTag("H2").SetActive(false);
		}
		if (lives == 0)
		{
			GameObject.FindWithTag("H1").SetActive(false);

			anim.SetInteger("State", 4);
		}
	}

    void transition()
    {
        // idle - jump
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetInteger("State", 2);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            anim.SetInteger("State", 0);
        }

        // idle - move
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
			transform.localScale = new Vector2 (-1.5f, 1.5f);
            anim.SetInteger("State", 1);
        }
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			transform.localScale = new Vector2(1.5f, 1.5f);
			anim.SetInteger("State", 1);
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.SetInteger("State", 0);
        }
    }
}
