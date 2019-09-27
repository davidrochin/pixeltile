using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public CharacterData data;

    public LayerMask shadowCollision;

    SpriteRenderer renderer;
    CharacterMotor motor;

    Coroutine animationCoroutine;

    SpriteRenderer shadowRenderer;

    private CardinalDirection currentDirection;

    private void Awake() {
        renderer = transform.GetComponentInChildren<SpriteRenderer>();
        motor = GetComponent<CharacterMotor>();

        // Create shadow
        GameObject shadow = new GameObject("Shadow");
        shadowRenderer = shadow.AddComponent<SpriteRenderer>();
        shadowRenderer.sprite = Resources.Load<Sprite>("shadow");
        IsoSprite isoSprite = shadow.AddComponent<IsoSprite>();
        shadow.transform.parent = transform;
        isoSprite.alwaysOnBehindOf = renderer;
        isoSprite.sortingOffset = 1;
    }

    private void Start() {
        currentDirection = DetermineDirection();
        //PlayAnimation(data.graphics.GetAnimation(AnimationType.Running, currentDirection), 0.1f);

        /*motor.OnFrameFinish += delegate () {

            CardinalDirection cardinalDirection = DetermineDirection();
            if (cardinalDirection != currentDirection) {
                if (motor.Grounded) {
                    PlayAnimation(data.graphics.GetAnimation(motor.Moving ? AnimationType.Running : AnimationType.Idle, cardinalDirection), 0.1f);
                }
                currentDirection = cardinalDirection;
            }
            
        };*/

        /*motor.OnGroundedChange += delegate () {
            if (motor.Grounded) {
                PlayAnimation(data.graphics.GetAnimation(motor.Moving ? AnimationType.Running : AnimationType.Idle, currentDirection), 0.1f);
            } else {
                PlayAnimation(data.graphics.GetAnimation(AnimationType.Falling, currentDirection), 0.1f);
            }
        };*/

        /*motor.OnWalk += delegate () {
            CardinalDirection cardinalDirection = DetermineDirection();
            PlayAnimation(data.graphics.GetAnimation(AnimationType.Running, cardinalDirection), 0.1f);
        };*/

        /*motor.OnStop += delegate () {
            CardinalDirection cardinalDirection = DetermineDirection();
            PlayAnimation(data.graphics.GetAnimation(AnimationType.Idle, cardinalDirection), 0.1f);
        };*/
    }

    private void Update() {

        // Manage Input
        Vector3 v = Vector3.zero;

        if (GetInputMagnitude() > 0.05f) {
            motor.TurnTowards(GetInputVector());
            motor.Walk(GetInputVector() * GetInputMagnitude() * 1f);
            v = GetInputVector() * GetInputMagnitude() * 1f;
        }

        motor.animator.SetFloat("Move/North", Vector3.Dot(v, new Vector3(1f, 0f, 1f).normalized));
        motor.animator.SetFloat("Move/East", Vector3.Dot(v, new Vector3(1f, 0f, -1f).normalized));

        if (Input.GetButtonDown("Jump")) {
            motor.Jump();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.Break();
        }

        // Manage shadow
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, float.MaxValue, shadowCollision)) {
            shadowRenderer.enabled = true;
            shadowRenderer.transform.position = hit.point + Vector3.up * 0.5f;
        } else {
            shadowRenderer.enabled = false;
        }

        
    }

    public InputType inputType;


    Vector3 GetInputVector() {
        Vector3 input = Vector3.zero;

        if (inputType == InputType.Normal) {
            input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        } else if (inputType == InputType.Raw) {
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        }

        //if (virtualJoystick != null) { input = new Vector3(input.x + virtualJoystick.input.x, 0f, input.y + virtualJoystick.input.y); }

        //Transformar la direccion para que sea relativa a la camara.
        //Vector3 transDirection = Camera.main.transform.TransformDirection(input);
        Quaternion tempQ = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
        //Debug.Log(tempQ);
        Vector3 transDirection = tempQ * input;

        //Hacer que el Vector no apunte hacia arriba.
        //transDirection = new Vector3(transDirection.x, 0f, transDirection.z).normalized;
        finalMovementVector = transDirection;
        return transDirection.normalized;
    }

    float GetInputMagnitude() {
        Vector3 input = Vector3.zero;

        // Get Input from standard Input methods
        if (inputType == InputType.Normal) { input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")); } else if (inputType == InputType.Raw) { input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")); }

        // Get Input from Virtual Joystick if available
        //if (virtualJoystick != null) { input = new Vector3(input.x + virtualJoystick.input.x, 0f, input.y + virtualJoystick.input.y); }

        // Clamp magnitude to 1
        return Vector3.ClampMagnitude(input, 1f).magnitude;
    }

    Vector3 finalMovementVector;
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, finalMovementVector);
    }

    public enum InputType { Normal, Raw }

    public void PlayAnimation(Sprite[] animation, float frequency) {
        if(animationCoroutine != null) {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimationCoroutine(animation, frequency));
    }

    public CardinalDirection DetermineDirection() {
        float r = motor.transform.rotation.eulerAngles.y;
        if((r > 337.5 && r <= 361) || (r >= 0f && r <= 22.5)) {
            return CardinalDirection.NW;
        } else if (r > 22.5 && r <= 67.5) {
            return CardinalDirection.N;
        } else if (r > 67.5 && r <= 112.5) {
            return CardinalDirection.NE;
        } else if (r > 112.5 && r <= 157.5) {
            return CardinalDirection.E;
        } else if (r > 157.5 && r <= 202.5) {
            return CardinalDirection.SE;
        } else if (r > 202.5 && r <= 247.5) {
            return CardinalDirection.S;
        } else if (r > 247.5 && r <= 292.5) {
            return CardinalDirection.SW;
        } else if (r > 292.5 && r <= 337.5) {
            return CardinalDirection.W;
        }
        return CardinalDirection.S;
    }

    private IEnumerator AnimationCoroutine(Sprite[] animation, float frequency) {
        int index = 0;
        while (true) {
            if(index >= animation.Length) {
                index = 0;
            }
            renderer.sprite = animation[index];
            index++;
            yield return new WaitForSeconds(frequency);
        }
    }
}

public enum CardinalDirection {
    S, SW, W, NW, N, NE, E, SE
}