using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour
{
    GravityAttractor planet;
	Rigidbody rigidbody;
    void Awake() {
        planet = 
            GameObject.FindGameObjectWithTag("Planet")
            .GetComponent<GravityAttractor>();
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    void FixedUpdate() {
        // Allow this body to be influenced by planet's gravity
		planet.Attract(rigidbody);
    }
}
