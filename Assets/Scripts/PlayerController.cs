using UnityEngine;
using UnityEngine.Assertions.Must;

namespace SlingRun
{
	public class PlayerController : MonoBehaviour
	{

		public float speedFactor;

		public GameObject defaultSprite;
		
		
		public bool moving;
		public bool locked;

		private Rigidbody2D rb2d;
		private Vector3 startPos;
		private bool mouseLastClicked;
		private Vector3 mouseStartPos;
		private bool sliding;
		private GameObject line;

		// Use this for initialization
		private void Start()
		{
			rb2d = GetComponent<Rigidbody2D>();
			startPos = rb2d.position;
		}

		// Update is called once per frame
		private void Update()
		{
			if (UIController.paused || locked) return;
			if (moving)
				return;
			if (Input.GetMouseButton(0))
			{
				if (!mouseLastClicked)
				{
					var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

					if ((mousePos - Constants.MOUSE_POS_DEPTH - transform.position).magnitude < Constants.BALL_TOUCH_SIZE)
					{
						mouseStartPos = mousePos;
						sliding = true;
					}
				}

				if (sliding)
				{
					var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseStartPos;

					if (delta.magnitude > Constants.BALL_MAX_MAGNITUDE)
						delta = delta.normalized * Constants.BALL_MAX_MAGNITUDE;

					rb2d.position = startPos + delta;
					
					if(line != null)
						Destroy(line);
					line = LineFactory.createDashedLine(defaultSprite,
						startPos+delta+Constants.PREDICT_LINE_DEPTH, startPos-delta+Constants.PREDICT_LINE_DEPTH,
						Constants.PREDICT_LINE_COLOR, Constants.PREDICT_LINE_THICKNESS, Constants.PREDICT_LINE_DASH_LENGTH);
				}

				mouseLastClicked = true;
			}
			else
			{
				if (mouseLastClicked)
				{
					var force = (startPos - (Vector3) rb2d.position) * speedFactor;

					Destroy(line);
					
					if (force.magnitude >= Constants.BALL_MIN_SPEED)
					{
						rb2d.velocity = force;
						moving = true;
					}
					else
					{
						rb2d.position = startPos;
					}

					sliding = false;
				}

				mouseLastClicked = false;
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (locked || UIController.paused) return;
			if (collision.gameObject.CompareTag(Constants.RESPAWN_TAG))
			{
				Respawn();
			}
			else if (collision.gameObject.CompareTag(Constants.FINISH_TAG))
			{
				GameManager.instance.NextLevel();
				rb2d.velocity = Vector2.zero;
				moving = false;
				locked = true;
			}
		}

		internal void Release()
		{
			rb2d.velocity = Vector2.zero;
			moving = false;
			locked = false;
			startPos = rb2d.position;
		}

		internal void Respawn()
		{
			if (moving && !locked)
			{
				rb2d.velocity = Vector2.zero;
				moving = false;
				rb2d.position = startPos;
				GameManager.instance.LooseLife();
			}
		}
	}
}
