using UnityEngine;

[RequireComponent(typeof(PoolObject))]

class Asteroid : MonoBehaviour
{
    [Space(10)]
    [SerializeField] private Rigidbody2D rigidbodyAsteroid;
    [SerializeField] private PoolObject poolObject;
    [Space(10)]
    [SerializeField] private float maxSpeedAsteroid;
    [SerializeField] private float minSpeedAsteroid;
    [Space(10)]
    [SerializeField] private int maxLecelAsteroid = 3;

    private Transform _transformAsteroid;
    public Transform TransformAsteroid => _transformAsteroid;

    private float _speedAsteroid;
    public float SpeedAsteroid => _speedAsteroid;

    private int lvlAsteroid;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;

        _transformAsteroid = this.transform;

        _speedAsteroid = Random.Range(minSpeedAsteroid, maxSpeedAsteroid);
    }

    private void FixedUpdate()
    {
        Move();
        CheckPosition();
    }

    public void Move()
    {
        rigidbodyAsteroid.velocity = _transformAsteroid.up * _speedAsteroid;
    }

    private void CheckPosition()
    {
        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect + 1f;
        float sceneHeight = mainCam.orthographicSize * 2 + 1f;
        float scenerRightEdge = sceneWidth / 2;
        float scenerLeftEdge = scenerRightEdge * -1;
        float scenerTopEdge = sceneHeight / 2;
        float scenerBottomEdge = scenerTopEdge * -1;

        if (_transformAsteroid.position.x > scenerRightEdge)
        {
            _transformAsteroid.position = new Vector2(scenerLeftEdge, _transformAsteroid.position.y);
        }
        if (_transformAsteroid.position.x < scenerLeftEdge)
        {
            _transformAsteroid.position = new Vector2(scenerRightEdge, _transformAsteroid.position.y);
        }
        if (_transformAsteroid.position.y > scenerTopEdge)
        {
            _transformAsteroid.position = new Vector2(_transformAsteroid.position.x, scenerBottomEdge);
        }
        if (_transformAsteroid.position.y < scenerBottomEdge)
        {
            _transformAsteroid.position = new Vector2(_transformAsteroid.position.x, scenerTopEdge);
        }
    }

    public void ScaleAsteroid(int generation)
    {
        if (generation > 0 && generation <= maxLecelAsteroid)
        {
            lvlAsteroid = generation;
        }
        else if (generation <= 0)
        {
            lvlAsteroid = 1;
        }
        else if (generation > maxLecelAsteroid)
        {
            lvlAsteroid = maxLecelAsteroid;
        }

        _transformAsteroid.localScale = new Vector3((1f / lvlAsteroid),
                                                    (1f / lvlAsteroid),
                                                    (1f / lvlAsteroid));
    }

    private void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.tag == "RocketBullet")
        {
            GameLogic.Instance.NumberOfPoints(lvlAsteroid);
            poolObject.ReturnToPool();
            lvlAsteroid++;

            if (lvlAsteroid <= maxLecelAsteroid)
            {
                GameLogic.Instance.AsteroidCreation(transform.position, transform.rotation * Quaternion.Euler(0, 0, 45f), lvlAsteroid);
                GameLogic.Instance.AsteroidCreation(transform.position, transform.rotation * Quaternion.Euler(0, 0, -90f), lvlAsteroid);
            }
            collisionInfo.GetComponent<PoolObject>().ReturnToPool();
        }

        if (collisionInfo.tag == "Rocket")
        {
            poolObject.ReturnToPool();
        }

        if (collisionInfo.tag == "FlyingSaucer")
        {
            poolObject.ReturnToPool();
        }
    }
}
