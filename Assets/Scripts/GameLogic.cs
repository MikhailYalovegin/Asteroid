using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Pool))]

class GameLogic : MonoBehaviour
{
    public static GameLogic Instance = null;

    [Space(10)]
    [SerializeField] private GameObject rocket;
    [SerializeField] private GameObject flyingSaucer;
    [Space(10)]
    [SerializeField] private PauseMenu pauseMenu;
    [Space(10)]
    [SerializeField] private Pool pool;
    [SerializeField] private int rocketLife = 5;

    [Space(10)]
    [SerializeField] private AudioSource soundDestruction;

    public int RocketLife => rocketLife;

    private GameObject _rocketPlayer;
    public GameObject RocketPlayer => _rocketPlayer;

    private FlyingSaucer _flyingSaucer;
    private SpriteRenderer spriteRocketPlayer;
    private Collider2D colliderRocketPlayer;
    private AudioSource flyingSaucerSound;

    private Camera mainCam;

    private int numberAsteroidsOnFirstLvlGame = 2;

    private int pointsEarnedCounter = 0;
    public int PointsEarnedCounter => pointsEarnedCounter;

    private void Awake()
    {
        Instance = this;

        mainCam = Camera.main;

        _rocketPlayer = Instantiate(rocket, Vector3.zero, Quaternion.identity);

        _flyingSaucer = flyingSaucer.GetComponent<FlyingSaucer>();
        flyingSaucerSound = flyingSaucer.GetComponent<AudioSource>();

        spriteRocketPlayer = _rocketPlayer.GetComponent<SpriteRenderer>();
        colliderRocketPlayer = _rocketPlayer.GetComponent<Collider2D>();
    }

    private void Start()
    {
        Time.timeScale = 0f;

        _rocketPlayer.SetActive(true);
        flyingSaucer.SetActive(false);

        StartCoroutine(RandomMovingFlyingSaucer());
        StartCoroutine(RandomAsteroidCreation());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void AsteroidCreation(Vector3 vector3, Quaternion quaternion, int generation)
    {
        pool.GetFreeElement(vector3, quaternion, generation);
    }

    public void RocketFail()
    {
        rocketLife--;

        if (rocketLife <= 0)
        {
            pauseMenu.RocketFail();
            print("GAME OVER");
        }
        StartCoroutine(BlinkRocket());
    }

    public void NumberOfPoints(int lvlAsteroid)
    {
        soundDestruction.Play();

        switch (lvlAsteroid)
        {
            case 0:
                pointsEarnedCounter += 200;
                break;

            case 1:
                pointsEarnedCounter += 20;
                break;

            case 2:
                pointsEarnedCounter += 50;
                break;

            case 3:
                pointsEarnedCounter += 100;
                break;
        }
    }

    private IEnumerator BlinkRocket()
    {
        colliderRocketPlayer.enabled = false;
        for (int i = 0; i < 3; i++)
        {
            spriteRocketPlayer.enabled = false;
            yield return new WaitForSeconds(0.5f);
            spriteRocketPlayer.enabled = true;
            yield return new WaitForSeconds(0.5f);
        }
        colliderRocketPlayer.enabled = true;
    }

    private IEnumerator RandomAsteroidCreation()
    {
        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect + 1f;
        float sceneHeight = mainCam.orthographicSize * 2 + 1f;
        float scenerRightEdge = sceneWidth / 2;
        float scenerTopEdge = sceneHeight / 2;

        while (true)
        {
            if (pool.AreThereAnyActiveElements())
            {
                yield return new WaitForSeconds(2);

                for (int i = 0; i < numberAsteroidsOnFirstLvlGame; i++)
                {
                    float x = UnityEngine.Random.Range(scenerRightEdge, scenerRightEdge + 20);
                    float y = UnityEngine.Random.Range(scenerTopEdge, scenerTopEdge + 20);
                    int lvlOnStartAsteroet = numberAsteroidsOnFirstLvlGame > 2 ? UnityEngine.Random.Range(1, numberAsteroidsOnFirstLvlGame) : 1;

                    AsteroidCreation(new Vector3(x, y, 0), Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f)), lvlOnStartAsteroet);
                }
                numberAsteroidsOnFirstLvlGame++;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RandomMovingFlyingSaucer()
    {
        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect + 2f;
        float sceneHeight = mainCam.orthographicSize * 2 + 2f;
        float scenerRightEdge = sceneWidth / 2;
        float scenerTopEdge = sceneHeight / 2;
        scenerTopEdge *= 0.6f;

        while (true)
        {
            if (!flyingSaucer.activeSelf)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(20, 40));

                float x = scenerRightEdge + 20;
                float y = UnityEngine.Random.Range(-scenerTopEdge, scenerTopEdge);

                flyingSaucer.transform.position = new Vector3(x, y, 0);
                _flyingSaucer.RandomСharacterMove();

                flyingSaucer.SetActive(true);
                flyingSaucerSound.Play();

                StartCoroutine(RandomAttackFlyingSaucer());
            }

            StopCoroutine(RandomAttackFlyingSaucer());

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator RandomAttackFlyingSaucer()
    {
        while (flyingSaucer.activeSelf)
        {
            Vector3 vector3 = _rocketPlayer.transform.position - flyingSaucer.transform.position;
            float angle = Mathf.Atan2(-vector3.x, vector3.y) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            _flyingSaucer.AttackFlyingSaucer(rot);
            yield return new WaitForSeconds(Random.Range(2, 5));
        }
    }
}
