using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity, _scrollSpeed, _distanceBetweenBeacons, _maxAngle;
    [SerializeField] private GameObject _beacon;

    private List<GameObject> _beaconList = new List<GameObject>();

    private float _angle, _movement, _angleAtPreviousBeacon;

    private bool _canMoveForward = true;

    private void Start()
    {
        // Drop the first beacon
        DropBeacon();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameState.Instance.IsPlaying)
        {
            return;
        }

        // Rotation
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _angle = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.fixedDeltaTime;
        }

        // Move
        _movement = Input.GetAxis("Mouse ScrollWheel") * _scrollSpeed * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        if (_movement > 0.1f && _canMoveForward)
        {
            transform.Translate(Vector3.forward * _movement);

            // Drop a new beacon
            if ((transform.position - _beaconList.Last().transform.position).magnitude > _distanceBetweenBeacons)
            {
                DropBeacon();
            }
        }
        else if (_movement < (-0.1f))
        {
            if (_beaconList.Count > 1)
            {             
                
                Vector3 direction = _beaconList.Last().transform.position - transform.position;
            
                // Rotate the head
                transform.rotation = Quaternion.LookRotation(direction);

                // Move backwards
                transform.Translate(Vector3.forward * _movement);

                if ((transform.position - _beaconList.Last().transform.position).magnitude < _distanceBetweenBeacons)
                {
                    PickUpBeacon();
                }
            }
        }

        if (_angle != 0)
        {
            var buffer = transform.rotation.y + _angle;
            
            if (Mathf.Abs(buffer - _angleAtPreviousBeacon) < _maxAngle)
            {
                transform.Rotate(Vector3.up * _angle);
            }
        }
    }

    private void DropBeacon()
    {
        var beacon = Instantiate(_beacon, transform);
        beacon.transform.SetParent(transform.parent);
        _beaconList.Add(beacon);

        // Record angle
        _angleAtPreviousBeacon = transform.rotation.y;

    }

    private void PickUpBeacon()
    {
        Destroy(_beaconList.Last());
        _beaconList.Remove(_beaconList.Last());

        // Record angle
        _angleAtPreviousBeacon = transform.rotation.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            _canMoveForward = false;
            GameState.Instance.HitWall();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall")
        {
            _canMoveForward = true;
        }
    }
}
