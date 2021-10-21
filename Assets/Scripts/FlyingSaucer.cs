using UnityEngine;

[RequireComponent(typeof(Pool))]

public class FlyingSaucer : MonoBehaviour
{
    [Space(10)]
    [SerializeField] Rigidbody2D rigidbodyFlyingSaucer;
    [SerializeField] private Pool pool;
    [Space(10)]
    [SerializeField] private float maxFlyingSaucer;
    [SerializeField] private float minFlyingSaucer;

    private Transform _transformFlyingSaucer;
    private float _speedFlyingSaucer;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;

        _transformFlyingSaucer = this.transform;
    }

    private void FixedUpdate()
    {
        Move();
        CheckPosition();
    }

    public void RandomÑharacterMove()
    {
        _speedFlyingSaucer = Random.Range(minFlyingSaucer, maxFlyingSaucer);

        if (Random.Range(0, 2) == 0) _speedFlyingSaucer *= 1;
        else _speedFlyingSaucer *= -1;
    }

    private void Move()
    {
        rigidbodyFlyingSaucer.velocity = _transformFlyingSaucer.right * _speedFlyingSaucer;
    }

    private void CheckPosition()
    {
        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect + 2f;
        float sceneHeight = mainCam.orthographicSize * 2 + 2f;
        float scenerRightEdge = sceneWidth / 2;
        float scenerLeftEdge = scenerRightEdge * -1;
        float scenerTopEdge = sceneHeight / 2;
        float scenerBottomEdge = scenerTopEdge * -1;

        if (_transformFlyingSaucer.position.x > scenerRightEdge)
        {
            _transformFlyingSaucer.position = new Vector2(scenerLeftEdge, _transformFlyingSaucer.position.y);
        }
        if (_transformFlyingSaucer.position.x < scenerLeftEdge)
        {
            _transformFlyingSaucer.position = new Vector2(scenerRightEdge, _transformFlyingSaucer.position.y);
        }
        if (_transformFlyingSaucer.position.y > scenerTopEdge)
        {
            _transformFlyingSaucer.position = new Vector2(_transformFlyingSaucer.position.x, scenerBottomEdge);
        }
        if (_transformFlyingSaucer.position.y < scenerBottomEdge)
        {
            _transformFlyingSaucer.position = new Vector2(_transformFlyingSaucer.position.x, scenerTopEdge);
        }
    }

    public void AttackFlyingSaucer(Quaternion quaternion)
    {
        pool.GetFreeElement(transform.position, quaternion, Color.red);
    }

    private void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.tag == "RocketBullet")
        {
            gameObject.SetActive(false);
            GameLogic.Instance.NumberOfPoints(0);
            collisionInfo.GetComponent<PoolObject>().ReturnToPool();
        }

        if (collisionInfo.tag == "Rocket")
        {
            gameObject.SetActive(false);
        }

        if (collisionInfo.tag == "Asteroid")
        {
            gameObject.SetActive(false);
        }
    }
}
