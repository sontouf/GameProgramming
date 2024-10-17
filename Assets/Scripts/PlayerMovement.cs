using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	public float speed;
	public float rotationSpeed;

	private Vector2 movementValue;
	private float lookValue;

	Rigidbody rb;

	private void Awake()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Locked;

		rb = GetComponent<Rigidbody>();
	}
	// Start is called before the first frame update
	public void OnMove(InputValue value)
	{
		movementValue = value.Get<Vector2>() * speed;
	}

	public void OnLook(InputValue value)
	{
		lookValue = value.Get<Vector2>().x * rotationSpeed;
	}

	void Start()
	{
		speed = 10f;
		rotationSpeed = 60f;

	}

	// Update is called once per frame
	void Update()
	{
		rb.AddRelativeForce(
			movementValue.x * Time.deltaTime,
			0,
			movementValue.y * Time.deltaTime);

		rb.AddRelativeTorque(0, lookValue * Time.deltaTime, 0);
		transform.Translate(
			movementValue.x * Time.deltaTime,
			0,
			movementValue.y * Time.deltaTime);
		transform.Rotate(0, lookValue * Time.deltaTime, 0);
	}
}
