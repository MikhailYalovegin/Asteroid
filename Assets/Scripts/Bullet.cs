using UnityEngine;

[RequireComponent(typeof(PoolObject))]

public class Bullet : MonoBehaviour
{
    [Space(10)]
    [SerializeField] private PoolObject poolObject;
    [Space(10)]
    [SerializeField] private float speedBullet = 20f;
    [Space]
    [SerializeField] AudioSource ShootSound;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        ShootSound.Play();
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speedBullet);
        CheckPosition();
    }

    private void CheckPosition()
    {
        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;
        float scenerRightEdge = sceneWidth / 2;
        float scenerLeftEdge = scenerRightEdge * -1;
        float scenerTopEdge = sceneHeight / 2;
        float scenerBottomEdge = scenerTopEdge * -1;

        if (transform.position.x > scenerRightEdge ||
           transform.position.x < scenerLeftEdge ||
           transform.position.y > scenerRightEdge ||
           transform.position.y < scenerBottomEdge)
        {
            poolObject.ReturnToPool();
        }
    }
}
