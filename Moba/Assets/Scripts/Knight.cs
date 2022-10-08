using UnityEngine;
using UnityEngine.AI;
public class Knight : MonoBehaviour
{
    NavMeshAgent agent;
    public Animator animator;
    public float rotateSpeedMoviment = 0.075f;
    float rotateVelocity;
    Vector3 target;
    Rigidbody rigidbody;
    public float jumpForce = 220;
    [SerializeField]float destinationReachedTreshold;
    bool InMove = false;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1)){
            Debug.Log("click");
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, Mathf.Infinity)){
                MovimentTo(hit.point);
            }
        }
        if (Input.GetButtonDown("Jump")) {
            Debug.Log("jump");
            rigidbody.AddForce(transform.up * jumpForce);
		}
        CheckDestinationReached();
    }

    void MovimentTo(Vector3 point){
        //move
        SetTarget(point);
        //rotation
        Quaternion rotationToLookAt = Quaternion.LookRotation(point - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
        rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMoviment *(Time.deltaTime*5));
        transform.eulerAngles = new Vector3(0,rotationY,0);

        animator.SetBool("move",true);

    }
    void SetTarget(Vector3 position) {
        target = position;
        agent.SetDestination(target);
        InMove=true;
    }
    void CheckDestinationReached() {
        if(InMove){
            float distanceToTarget = Vector3.Distance(transform.position, target);
            if(distanceToTarget < 0.1)
            {
                InMove = false;
                animator.SetBool("move",false);
            }
        }
    }
}
