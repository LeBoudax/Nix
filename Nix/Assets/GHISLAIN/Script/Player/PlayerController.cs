using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity, _scrollSpeed, _distanceBetweenBeacons;
    [SerializeField] private GameObject _beacon;

    private List<GameObject> _beaconList = new List<GameObject>();

    private float _angle, _movement;

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
            _angle = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        }

        // Move
        _movement = Input.GetAxis("Mouse ScrollWheel") * _scrollSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_movement > 0)
        {
            transform.Translate(Vector3.forward * _movement);

            // Drop a new beacon
            if ((transform.position - _beaconList.Last().transform.position).sqrMagnitude > _distanceBetweenBeacons)
            {
                DropBeacon();
            }
        }
        else if (_movement < 0)
        {
            Vector3 direction = transform.position - _beaconList.Last().transform.position;
            
            // Rotate the head
            transform.rotation = Quaternion.LookRotation(- direction);

            // Move backward, and pick up last beacon if necessary
            if (direction.magnitude < Mathf.Abs(_movement))
            {
                Debug.Log("magnitude = " + direction.magnitude + "; movement = " + _movement);
                transform.Translate(Vector3.back * direction.magnitude);
                if (_beaconList.Count > 1)
                {
                    PickUpBeacon();
                }
            }
            else
            {
                transform.Translate(Vector3.back * _movement);
            }
        }

        if (_angle != 0)
        {
            transform.Rotate(Vector3.up * _angle);
        }
    }

    private void DropBeacon()
    {
        var beacon = Instantiate(_beacon, transform);
        beacon.transform.SetParent(transform.parent);
        _beaconList.Add(beacon);
    }

    private void PickUpBeacon()
    {
        Destroy(_beaconList.Last(), 1f);
        _beaconList.Remove(_beaconList.Last());
    }
}
