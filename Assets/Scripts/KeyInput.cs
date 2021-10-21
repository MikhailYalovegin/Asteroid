using UnityEngine;

public class KeyInput : MonoBehaviour
{
    [SerializeField] private bool _mouseFlag = true;

    public bool MouseFlag => _mouseFlag;

    private bool _forwardMovement;
    public bool ForwardMovement => _forwardMovement;

    private float _turnDirection;
    public float TurnDirection => _turnDirection;

    private bool _attak;
    public bool Attak => _attak;

    private Vector3 _mousePos;
    public Vector3 MousePos => _mousePos;

    public void ControlSwitch()
    {
        _mouseFlag = !_mouseFlag;
    }

    private void Update()
    {
        bool _at;

        if (_mouseFlag) _at = Input.GetMouseButtonDown(0);
        else _at = (Input.GetKeyDown(KeyCode.Space));

        _attak = _at ? true : false;
    }

    private void FixedUpdate()
    {
        bool _forwardMove;

        if (_mouseFlag) _forwardMove = (Input.GetMouseButton(1) || Input.GetKey(KeyCode.UpArrow));

        else _forwardMove = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));

        _forwardMovement = _forwardMove ? true : false;
        _turnDirection = Input.GetAxis("Horizontal");
        _mousePos = Input.mousePosition;
    }
}