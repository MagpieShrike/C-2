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

    public AudioClip bgMusic;
    public AudioClip winMusic;
    public AudioSource musicSource;

    private Rigidbody2D rb2d;
	private int count;
	private int lives;
    private Vector3 originalPos;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalPos = gameObject.transform.position;
        musicSource.clip = bgMusic;
        musicSource.Play();

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

        if (count == 4)
        {
            if (other.gameObject.CompareTag("Checkpoint"))
            {
                SceneManager.LoadScene("Level2");
            }
        }

        if (other.gameObject.CompareTag("LiquidDeath"))
        {
            lives = lives - 1;
            lifeCounter();

            anim.SetInteger("State", 3);

            gameObject.transform.position = originalPos;
        }
    }

	void setScore()
	{
		score.text = "Score: " + count.ToString();

		if (count == 4)
		{
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                win.text = "Level Clear \n Proceed to Checkpoint";
                musicSource.Stop();
                musicSource.clip = winMusic;
            }
            else if (SceneManager.GetActiveScene().name == "Level2")
            {
                win.text = "You Win!";
                musicSource.Stop();
                musicSource.clip = winMusic;
                musicSource.Play();
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
            gameObject.SetActive(false);
            win.text = "You Lose!";
            musicSource.Stop();

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
