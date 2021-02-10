using UnityEngine;
using System.Collections;
using Enums;

public class GamePadInput : MonoBehaviour {

	[Range(0.1f,1f)]
	public float inputThreshold = 0.25f;
	[Range(0.1f,1f)]
	public float scrollThreshold = 0.35f;
	private float inputDelay_sholder;
	private float inputDelay_stick_L;
	private float inputDelay_stick_R;

	public Vector2 l_Stick;
	public Vector2 r_Stick;
	private bool directionSent_up_LS;
	private bool directionSent_down_LS;
	private bool directionSent_left_LS;
	private bool directionSent_right_LS;
	private bool directionSent_up_RS;
	private bool directionSent_down_RS;
	private bool directionSent_left_RS;
	private bool directionSent_right_RS;

	[Space (8f)]
	public float lTrigger;
	public float rTrigger;
	public float triggerAxis;
	private float triggerAxis_L;
	private float triggerAxis_R;

	public delegate void Triggers(float l, float r);
	public static event Triggers OnTriggers;
	public delegate void TriggersAxis(float l_pos, float r_neg);
	public static event TriggersAxis OnTriggersAxis;

	public delegate void L_Stick(Vector2 lStick);
	public static event L_Stick On_L_Stick;
	public delegate void R_Stick(Vector2 rStick);
	public static event R_Stick On_R_Stick;

	public delegate void GamePad_Up_LS(UserInput b); // b for button INPUT
	public static event GamePad_Up_LS OnGamePad_Up_LS;
	public delegate void GamePad_Down_LS(UserInput b); // b for button INPUT
	public static event GamePad_Down_LS OnGamePad_Down_LS;
	public delegate void GamePad_Left_LS(UserInput b); // b for button INPUT
	public static event GamePad_Left_LS OnGamePad_Left_LS;
	public delegate void GamePad_Right_LS(UserInput b); // b for button INPUT
	public static event GamePad_Right_LS OnGamePad_Right_LS;
	public delegate void GamePad_Up_RS(UserInput b); // b for button INPUT
	public static event GamePad_Up_RS OnGamePad_Up_RS;
	public delegate void GamePad_Down_RS(UserInput b); // b for button INPUT
	public static event GamePad_Down_RS OnGamePad_Down_RS;
	public delegate void GamePad_Left_RS(UserInput b); // b for button INPUT
	public static event GamePad_Left_RS OnGamePad_Left_RS;
	public delegate void GamePad_Right_RS(UserInput b); // b for button INPUT
	public static event GamePad_Right_RS OnGamePad_Right_RS;

	public delegate void GamePad_A(UserInput b); // b for button INPUT
	public static event GamePad_A OnGamePad_A;
	public delegate void GamePad_B(UserInput b); // b for button INPUT
	public static event GamePad_B OnGamePad_B;
	public delegate void GamePad_X(UserInput b); // b for button INPUT
	public static event GamePad_X OnGamePad_X;
	public delegate void GamePad_Y(UserInput b); // b for button INPUT
	public static event GamePad_Y OnGamePad_Y;

	private bool L_ButtonDown;
	private bool R_ButtonDown;
	public delegate void L_Button(UserInput b); // b for button INPUT
	public static event L_Button On_L_Button;
	public delegate void R_Button(UserInput b); // b for button INPUT
	public static event R_Button On_R_Button;

	void Start () {

	}

	void Update () {
		lTrigger = Input.GetAxis ("lTrigger");
		rTrigger = Input.GetAxis ("rTrigger");
		triggerAxis = Input.GetAxis ("TriggersAxis");
		
//		Debug.Log("lTrigger: "+lTrigger + " -- " + "rTrigger: " + rTrigger);

		if (triggerAxis > 0) {
			triggerAxis_L = triggerAxis;
		} else if (triggerAxis < 0) {
			triggerAxis_R = triggerAxis;
		}

		if (OnTriggers != null) {
			OnTriggers (lTrigger, rTrigger);
		}
		if (OnTriggersAxis != null) {
			OnTriggersAxis (triggerAxis_L, triggerAxis_R);
		}

//*****************************************Stick axises
		l_Stick = new Vector2 (Input.GetAxis ("L_Stick_Hoz"), Input.GetAxis ("L_Stick_Vert"));
		r_Stick = new Vector2 (Input.GetAxis ("R_Stick_Hoz"), Input.GetAxis ("R_Stick_Vert"));

		if (On_R_Stick != null) {
			On_R_Stick (r_Stick);
		}
		if (On_L_Stick != null) {
			On_L_Stick (l_Stick);
		}

//*****************************************LEFT stick direction clicks
		//reset input delay counter
		if (l_Stick.x < inputThreshold && l_Stick.x > -inputThreshold && l_Stick.y < inputThreshold && l_Stick.y > -inputThreshold) {
			inputDelay_stick_L = 0;
		}

		//UP
		if (l_Stick.y > -inputThreshold) {
			directionSent_up_LS = false;
		}
		if (l_Stick.y < -inputThreshold) {
			if (!directionSent_up_LS) {
				directionSent_up_LS = true;
				if (OnGamePad_Up_LS != null) {
					OnGamePad_Up_LS (UserInput.Up);
				}
				Debug.Log (l_Stick + " - UP");
			}
			inputDelay_stick_L += Time.deltaTime;
			if (inputDelay_stick_L > inputThreshold) {
				inputDelay_stick_L = 0;
				if (OnGamePad_Up_LS != null) {
					OnGamePad_Up_LS (UserInput.Up);
				}
				Debug.Log (l_Stick + " - UP");
			}
		}
		//DOWN
		if (l_Stick.y < inputThreshold) {
			directionSent_down_LS = false;
		}
		if (l_Stick.y > inputThreshold) {
			if (!directionSent_down_LS) {
				directionSent_down_LS = true;
				if (OnGamePad_Down_LS != null) {
					OnGamePad_Down_LS (UserInput.Down);
				}
				Debug.Log (l_Stick + " - DOWN");
			}
			inputDelay_stick_L += Time.deltaTime;
			if (inputDelay_stick_L > inputThreshold) {
				inputDelay_stick_L = 0;
				if (OnGamePad_Down_LS != null) {
					OnGamePad_Down_LS (UserInput.Down);
				}
				Debug.Log (l_Stick + " - DOWN");
			}
		}
		//LEFT
		if (l_Stick.x > -inputThreshold) {
			directionSent_left_LS = false;
		}
		if (l_Stick.x < -inputThreshold) {
			if (!directionSent_left_LS) {
				directionSent_left_LS = true;
				if (OnGamePad_Left_LS != null) {
					OnGamePad_Left_LS (UserInput.Left);
				}
				Debug.Log (l_Stick + " - LEFT");
			}
			inputDelay_stick_L += Time.deltaTime;
			if (inputDelay_stick_L > inputThreshold) {
				inputDelay_stick_L = 0;
				if (OnGamePad_Left_LS != null) {
					OnGamePad_Left_LS (UserInput.Left);
				}
				Debug.Log (l_Stick + " - LEFT");
			}
		}
		//RIGHT
		if (l_Stick.x < inputThreshold) {
			directionSent_right_LS = false;
		}
		if (l_Stick.x > inputThreshold) {
			if (!directionSent_right_LS) {
				directionSent_right_LS = true;
				if (OnGamePad_Right_LS != null) {
					OnGamePad_Right_LS (UserInput.Right);
				}
				Debug.Log (r_Stick + " - RIGHT");
			}
			inputDelay_stick_L += Time.deltaTime;
			if (inputDelay_stick_L > inputThreshold) {
				inputDelay_stick_L = 0;
				if (OnGamePad_Right_LS != null) {
					OnGamePad_Right_LS (UserInput.Right);
				}
				Debug.Log (l_Stick + " - RIGHT");
			}
		}
//*****************************************END

//*****************************************RIGHT stick direction clicks
		//reset input delay counter
		if (r_Stick.x < inputThreshold && r_Stick.x > -inputThreshold && r_Stick.y < inputThreshold && r_Stick.y > -inputThreshold) {
			inputDelay_stick_R = 0;
		}

		//UP
		if (r_Stick.y > -inputThreshold) {
			directionSent_up_RS = false;
		}
		if (r_Stick.y < -inputThreshold) {
			if (!directionSent_up_RS) {
				directionSent_up_RS = true;
				if (OnGamePad_Up_RS != null) {
					OnGamePad_Up_RS (UserInput.Up);
				}
				Debug.Log (r_Stick + " - UP");
			}
			inputDelay_stick_R += Time.deltaTime;
			if (inputDelay_stick_R > inputThreshold) {
				inputDelay_stick_R = 0;
				if (OnGamePad_Up_RS != null) {
					OnGamePad_Up_RS (UserInput.Up);
				}
				Debug.Log (r_Stick + " - UP");
			}
		}
		//DOWN
		if (r_Stick.y < inputThreshold) {
			directionSent_down_RS = false;
		}
		if (r_Stick.y > inputThreshold) {
			if (!directionSent_down_RS) {
				directionSent_down_RS = true;
				if (OnGamePad_Down_RS != null) {
					OnGamePad_Down_RS (UserInput.Down);
				}
				Debug.Log (r_Stick + " - DOWN");
			}
			inputDelay_stick_R += Time.deltaTime;
			if (inputDelay_stick_R > inputThreshold) {
				inputDelay_stick_R = 0;
				if (OnGamePad_Down_RS != null) {
					OnGamePad_Down_RS (UserInput.Down);
				}
				Debug.Log (r_Stick + " - DOWN");
			}
		}
		//LEFT
		if (r_Stick.x > -inputThreshold) {
			directionSent_left_RS = false;
		}
		if (r_Stick.x < -inputThreshold) {
			if (!directionSent_left_RS) {
				directionSent_left_RS = true;
				if (OnGamePad_Left_RS != null) {
					OnGamePad_Left_RS (UserInput.Left);
				}
				Debug.Log (r_Stick + " - LEFT");
			}
			inputDelay_stick_R += Time.deltaTime;
			if (inputDelay_stick_R > inputThreshold) {
				inputDelay_stick_R = 0;
				if (OnGamePad_Left_RS != null) {
					OnGamePad_Left_RS (UserInput.Left);
				}
				Debug.Log (r_Stick + " - LEFT");
			}
		}
		//RIGHT
		if (r_Stick.x < inputThreshold) {
			directionSent_right_RS = false;
		}
		if (r_Stick.x > inputThreshold) {
			if (!directionSent_right_RS) {
				directionSent_right_RS = true;
				if (OnGamePad_Right_RS != null) {
					OnGamePad_Right_RS (UserInput.Right);
				}
				Debug.Log (r_Stick + " - RIGHT");
			}
			inputDelay_stick_R += Time.deltaTime;
			if (inputDelay_stick_R > inputThreshold) {
				inputDelay_stick_R = 0;
				if (OnGamePad_Right_RS != null) {
					OnGamePad_Right_RS (UserInput.Right);
				}
				Debug.Log (r_Stick + " - RIGHT");
			}
		}
//*****************************************END

//******************************************BUTTONS
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			//OnGamePad_Up (UserInput.Up);
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			//OnGamePad_Down (UserInput.Down);
		}

		if (Input.GetButtonDown("A_Button")) {
			if (OnGamePad_A != null) {
				OnGamePad_A (UserInput.GamePad_A);
			}
		}
		if (Input.GetButtonDown("B_Button")) {
			if (OnGamePad_B != null) {
				OnGamePad_B (UserInput.GamePad_B);
			}
		}
		if (Input.GetButtonDown("X_Button")) {
			if (OnGamePad_X != null) {
				OnGamePad_X (UserInput.GamePad_X);
			}
		}
		if (Input.GetButtonDown("Y_Button")) {
			if (OnGamePad_Y != null) {
				OnGamePad_Y (UserInput.GamePad_Y);
			}
		}
//******************************************END

//******************************************SHOULDER BUTTONS
		if (Input.GetButtonDown("L_Button") && !R_ButtonDown) {
			inputDelay_sholder = 0;
			if (On_L_Button != null) {
				On_L_Button (UserInput.L_Button);
			}
		}
		if (Input.GetButton ("L_Button") && !R_ButtonDown) {
			L_ButtonDown = true;
			inputDelay_sholder += Time.deltaTime;
			if (inputDelay_sholder > scrollThreshold) {
				inputDelay_sholder = 0;
				if (On_L_Button != null) {
					On_L_Button (UserInput.L_Button);
				}
			}
		} 
		else {
			L_ButtonDown = false;
		}
		if (Input.GetButtonDown("R_Button") && !L_ButtonDown) {
			inputDelay_sholder = 0;
			if (On_R_Button != null) {
				On_R_Button (UserInput.R_Button);
			}
		}
		if (Input.GetButton("R_Button") && !L_ButtonDown) {
			R_ButtonDown = true;
			inputDelay_sholder += Time.deltaTime;
			if (inputDelay_sholder > scrollThreshold) {
				inputDelay_sholder = 0;
				if (On_R_Button != null) {
					On_R_Button (UserInput.R_Button);
				}
			}
		} 
		else {
			R_ButtonDown = false;
		}
//******************************************END
	}
}
