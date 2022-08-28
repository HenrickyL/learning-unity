using UnityEngine;
public class GravityAttractor : MonoBehaviour
{
    public float gravity = -10f;
    public void Attract(Rigidbody body){
        Vector3 gravityUp  =
            (body.position - transform.position).normalized;
        var bodyUp = body.transform.up;
        body.AddForce(gravityUp*gravity);
        body.rotation = Quaternion
            .FromToRotation(bodyUp,gravityUp )*body.rotation;
    }
}
