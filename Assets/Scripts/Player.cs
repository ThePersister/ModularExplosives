using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 200f;

    //[SerializeField]
    //private float _friction = 60f;

    private Rigidbody _rigidBody;
    private Transform _transform;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _transform = this.transform;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
        FaceTowards();
    }

    private void Move()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }

        if (direction != Vector3.zero)
        {
            direction.Normalize();
        }

        //
        // TODO: Try to make movement more natural, use AddForce and Friction. (Maybe check out earlier project).
        //
        //float frictionOverTime = _friction * Time.deltaTime;
        //Vector3 fricVelocity = new Vector3(
        //    Mathf.Clamp(_rigidBody.velocity.x - frictionOverTime, 0, Mathf.Infinity),
        //    _rigidBody.velocity.y,
        //    Mathf.Clamp(_rigidBody.velocity.z - frictionOverTime, 0, Mathf.Infinity)
        //);

        //Debug.Log("Velocity: " + _rigidBody.velocity + ", fricVelocity: " + fricVelocity);

        //_rigidBody.velocity = fricVelocity;
        //_rigidBody.AddForce(direction * _movementSpeed * Time.deltaTime);

        _rigidBody.velocity = direction * _movementSpeed * Time.deltaTime;
    }

    private void FaceTowards()
    {
        Vector3 futurePosition = _transform.position + _rigidBody.velocity;
        _transform.LookAt(new Vector3(futurePosition.x, _transform.position.y, futurePosition.z));
    }
}
