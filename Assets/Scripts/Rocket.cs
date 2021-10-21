using UnityEngine;

[RequireComponent(typeof(Pool))]
[RequireComponent(typeof(KeyInput))]

public class Rocket : MonoBehaviour
{
    [Space(10)]
    [SerializeField] private Rigidbody2D rigidbodyPlayer;
    [SerializeField] private KeyInput keyInput;
    [SerializeField] private Pool pool;
    [Space(10)]
    [SerializeField] private float moveAcceleration = 5f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float maxMoveSpeed = 1f;
    [Space(10)]
    [SerializeField] private float delayAttacks = 0.3f;

    private Camera mainCam;

    private float _timeAfterLastShoot;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void FixedUpdate()
    {
        MoveRocket();
        CheckPosition();
    }

    private void Update()
    {
        _timeAfterLastShoot -= Time.deltaTime;

        if (keyInput.Attak && Time.timeScale != 0)
        {
            Attack();
        }
    }

    private void MoveRocket()
    {
        if (keyInput.ForwardMovement)
        {
            rigidbodyPlayer.AddForce(transform.up * moveAcceleration);
        }

        if (keyInput.MouseFlag)
        {
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(keyInput.MousePos) - transform.position;
            float angle = Mathf.Atan2(-vector3.x, vector3.y) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -keyInput.TurnDirection * rotationSpeed), Space.World);
        }
        rigidbodyPlayer.velocity = new Vector2(Mathf.Clamp(rigidbodyPlayer.velocity.x, -maxMoveSpeed, maxMoveSpeed),
                                               Mathf.Clamp(rigidbodyPlayer.velocity.y, -maxMoveSpeed, maxMoveSpeed));
    }

    private void CheckPosition()
    {
        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect + 1f;
        float sceneHeight = mainCam.orthographicSize * 2 + 1f;
        float scenerRightEdge = sceneWidth / 2;
        float scenerLeftEdge = scenerRightEdge * -1;
        float scenerTopEdge = sceneHeight / 2;
        float scenerBottomEdge = scenerTopEdge * -1;

        if (transform.position.x > scenerRightEdge)
        {
            transform.position = new Vector2(scenerLeftEdge, transform.position.y);
        }
        if (transform.position.x < scenerLeftEdge)
        {
            transform.position = new Vector2(scenerRightEdge, transform.position.y);
        }
        if (transform.position.y > scenerTopEdge)
        {
            transform.position = new Vector2(transform.position.x, scenerBottomEdge);
        }
        if (transform.position.y < scenerBottomEdge)
        {
            transform.position = new Vector2(transform.position.x, scenerTopEdge);
        }
    }

    private void Attack()
    {
        if (!(_timeAfterLastShoot < 0)) return;

        pool.GetFreeElement(transform.position, transform.rotation, Color.green);

        _timeAfterLastShoot = delayAttacks;
    }

    private void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.tag == "EnemyBullet")
        {
            GameLogic.Instance.RocketFail();
            collisionInfo.GetComponent<PoolObject>().ReturnToPool();
        }

        if (collisionInfo.tag == "FlyingSaucer")
        {
            GameLogic.Instance.RocketFail();
        }

        if (collisionInfo.tag == "Asteroid")
        {
            GameLogic.Instance.RocketFail();
        }
    }
}