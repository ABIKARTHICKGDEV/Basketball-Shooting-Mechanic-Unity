
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasketballController : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public Transform Ball;
    public Transform Arms;
    public Transform PosOverHead;
    public Transform PosDribbled;
    public Transform Target;
   
    private bool InBallInHands = true;
    private bool IsBallFlying = false;
    private float T = 0f;

   
    void Update()
    {
        // Move the player based on input
        Vector3 Direction = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
        transform.position += Direction * MoveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + Direction);

        if (InBallInHands) 
        {
            if (Input.GetKey(KeyCode.Space)) {
                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * 180;

                // look towards the target position
                transform.LookAt(Target.parent.position);

            } else {
               
                float y = Mathf.Abs(Mathf.Sin(Time.time * 5f));
                Ball.position = PosDribbled.position + Vector3.up * y;
                Arms.localEulerAngles = Vector3.right * 0;

            }
            // throw the ball when the space key is released
            if (Input.GetKeyUp(KeyCode.Space)) {
                InBallInHands = false;
                IsBallFlying = true;
                T = 0;
            }
            
        }
        
        if (IsBallFlying) {
            T += Time.deltaTime;
            float duration = 0.5f;
            float t01 = T / duration;

            // move the ball from the overhead position to the target position
            Vector3 A = PosOverHead.position;
            Vector3 B = Target.position;
            Vector3 Pos = Vector3.Lerp(A, B, t01);

            // add a parabolic arc to the ball's trajectory
            Vector3 arc = Vector3.up * 5f * Mathf.Sin(t01 * 3.14f);

            Ball.position = Pos + arc;

            if (t01 >= 1f) {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        
    }

    private void OnTriggerEnter(Collider other) {

        InBallInHands = true;
        Ball.GetComponent<Rigidbody>().isKinematic = true;
    }
}
