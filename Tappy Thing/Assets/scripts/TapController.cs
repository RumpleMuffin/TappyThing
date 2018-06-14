using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour {

	public delegate void PlayerDelegate();
	public static event PlayerDelegate OnPlayerDied;
	public static event PlayerDelegate OnPlayerScored;

	public float tapForce = 10;
	public float tiltSmooth = 5;
	public Vector3 startPos;

	Rigidbody2D rb;
	Quaternion downRotation;
	Quaternion forwardRotaion;

	GameMnager game;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		downRotation = Quaternion.Euler(0, 0, -90);
		forwardRotaion = Quaternion.Euler(0, 0, 35);
		game = GameMnager.Instance;
		rb.simulated = false;
	}

	private void OnEnable()
	{
		GameMnager.OnGameStarted += OnGameStarted;
		GameMnager.OnGameOverConfirmed += OnGameOverConfirmed;
	}

	private void OnDisable()
	{
		GameMnager.OnGameStarted -= OnGameStarted;
		GameMnager.OnGameOverConfirmed -= OnGameOverConfirmed;
	}

	void OnGameStarted ()
	{
		rb.velocity = Vector3.zero;
		rb.simulated = true;
	}

	void OnGameOverConfirmed()
	{
		transform.position = startPos;
		transform.rotation = Quaternion.identity;

	}

	private void Update()
	{
		if (game.GameOver) return;
		
		if (Input.GetMouseButtonDown(0))
		{
			transform.rotation = forwardRotaion;
			rb.velocity = Vector3.zero;
			rb.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
		}

		transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
	}


	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "ScoreZone")
		{
			//score
			OnPlayerScored();//event sent to gamemanger;
			//play sound
		}

		if(col.gameObject.tag == "DeadZone")
		{
			rb.simulated = false;
			//register dead event
			OnPlayerDied(); //event sent to gamemanger;
			//play sound
		}
	}

}
