using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBController : MonoBehaviour
{
    
    [Tooltip("Weight in grams")] //(em gramas)
    [SerializeField] [Range(0.2f, 0.34f)] private float mass = 0.2f;

    private float velocity = 0f;
    private float liftingForce;
    private float energyInJoules = 1.49f;
    private float conversionRate = 3.281f; //multiplica m/s por isso para chegar em pés
    private Rigidbody rig;

    private Vector3 localForward;
    private Vector3 localUp;
    public float backSpinDrag;

    void Start()
    {
        rig = GetComponent<Rigidbody>();

        //convertendo de gramas para quilos (importantissimo)
        mass = mass * 0.001f; 

        rig.mass = mass;
        velocity = Mathf.Sqrt((2 * energyInJoules)/mass);

        localForward = transform.parent.forward;
        
        //Debug
        Debug.Log("\nVelocity: " + Mathf.Floor(velocity * conversionRate) + " feet per second - Mass: "+ mass/0.001f + "g");
    }

    void FixedUpdate()
    {
        rig.AddForce(velocity * localForward, ForceMode.Force); // velocidade linear

        liftingForce = Mathf.Sqrt(rig.velocity.magnitude) * backSpinDrag * 100; // Força de sustentação
        localUp = transform.parent.up;
        rig.AddForce(liftingForce * localUp * Time.deltaTime, ForceMode.Force); // velocidade angular (backspin)
    }
    public float GetMass()
    {
        return mass;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 12)
        {
            Destroy(gameObject);
        }
    }
}
