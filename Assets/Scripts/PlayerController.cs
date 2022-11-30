using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 lastMousePos;

    public float sensitivity = .16f;

    public float clampDelta = 42f;

    public float bounds = 5;

    [HideInInspector] public bool canMove, gameOver, finish;

    public GameObject breakablePlayer;
    public GameObject trail;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Time.timeScale = 1f;
    }

    private void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -bounds, bounds), transform.position.y,
            transform.position.z);
        if (canMove)
        {
            transform.position += FindObjectOfType<CameraMovement>().camVel;
        }

        if (!canMove && gameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }
        }
        else if (!canMove && !finish)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FindObjectOfType<GameManager>().RemoveUI();
                canMove = true;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }

        if (canMove)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 vector = lastMousePos - Input.mousePosition;
                lastMousePos = Input.mousePosition;
                vector = new Vector3(vector.x, 0, vector.y);

                Vector3 moveForce = Vector3.ClampMagnitude(vector, clampDelta);
                rb.AddForce(-moveForce * sensitivity - rb.velocity / 5f, ForceMode.VelocityChange);
            }
        }

        rb.velocity.Normalize();
    }

    private void GameOver()
    {
        GameObject shatterShpere =
            Instantiate(breakablePlayer, transform.position, Quaternion.identity);

        foreach (Transform o in shatterShpere.transform)
        {
            o.GetComponent<Rigidbody>().AddForce(Vector3.forward * 5, ForceMode.Impulse);
        }

        canMove = false;
        gameOver = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        trail.SetActive(false);
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!gameOver)
            {
                GameOver();
            }
        }
    }

    IEnumerator NextLevel()
    {
        finish = true;
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("Level"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Finish")
        {
            StartCoroutine("NextLevel");
        }
    }
}