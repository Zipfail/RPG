using UnityEngine;
using UnityEngine.InputSystem.XR;

public class pl_move : sound
{
    private CharacterController _controler => transform.GetComponent<CharacterController>();



    private float _speed_move;
    [SerializeField] private float _speed;
    [SerializeField] private float _speed_run;
    [SerializeField] private float _speed_grav;
    [SerializeField] private float _jump_power;
    private bool razr_walk = true;

    [SerializeField] private float mouseSens = 100.0f;
    [SerializeField] private Transform camer;
    private float xRotation = 0f;

    private Vector3 _dvig;
    private Vector3 _grav;
    void Start()
    {
        _nf_cursor(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (razr_walk)
        {
            _mous();
            run(Input.GetKey(KeyCode.LeftShift));
            _move();
            _gravity();
            sit(Input.GetKey(KeyCode.LeftControl));
            _controler.Move(_grav * Time.fixedDeltaTime);
            _jump();
            _controler.Move(_dvig * Time.fixedDeltaTime);
        }
        
    }

    private void _move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        _dvig = (transform.right * x + transform.forward * z) * _speed_move;
        if(x != 0 || z != 0)_one_playSound(_sound[0]);
    }

    private void _gravity()
    {
        _grav.y -= _speed_grav;

    }

    private void _jump()
    {
        if (_controler.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _grav.y = _jump_power;
            //print("qwer");
            playSound(_sound[1]);
        }
        
    }

    private void _mous()
    {
        float mousX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mousY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mousY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camer.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mousX);
    }
    public void _nf_cursor(bool sost)
    {
        if (sost)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void run(bool run)
    {
        _speed_move = run ? _speed_run : _speed;
    }
    private void sit(bool canSit)
    {
        _controler.height = canSit ? 1f : 2f;
    }
    public void _nf_walk(bool tip)
    {
        razr_walk = tip;
    }
}
