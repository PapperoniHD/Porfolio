using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    [Header("Values")]
    public float speed = 10f;
    public float rotSpeed = 10f;
    public float maxSpeed = 5f;
    public float bulletSpeed = 20f;

    [Header("Health & Points")]
    public int health = 10;

    [Header("Components")]
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletSpawnPos;
    [SerializeField]
    private TextMeshProUGUI healthUI;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //rb.AddForce(movement.y * speed * transform.up);
        rb.velocity = (movement.y * speed * transform.up);
        rb.rotation += movement.x * rotSpeed * Time.deltaTime;

        if (rb.velocity.y >= maxSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x,maxSpeed);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shoot());
        }
        if (health <= 0)
        {
            SceneManager.LoadScene(0);
        }

        healthUI.SetText("Health: " + health);
    }

    IEnumerator Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPos.position, bulletSpawnPos.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = bulletSpeed * transform.up;
        yield return new WaitForSeconds(1f);
        Destroy(bullet);
    }

    public IEnumerator Damage()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(0, 255, 255);
    }

}
