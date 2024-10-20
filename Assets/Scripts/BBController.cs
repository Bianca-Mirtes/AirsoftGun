using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBController : MonoBehaviour
{
    [Tooltip("Weight in grams")] //(em gramas)
    [SerializeField] [Range(0.2f, 0.34f)] private float mass = 0.2f;

    private float velocity = 0f;
    private float energyInJoules = 1.49f;
    private float conversionRate = 3.281f; //multiplica m/s por isso para chegar em pés
    private Rigidbody rig;
    private Vector3 localForward;
    public float backSpinDrag;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        //convertendo de gramas para quilos (importantissimo)
        mass = mass * 0.001f;
        rig.mass = mass;

        velocity = Mathf.Sqrt((2 * energyInJoules)/mass);

        localForward = transform.parent.forward;

        // aplicando a força linear do disparo
        rig.velocity = localForward * velocity;

        //Debug
        Debug.Log("\nVelocity: " + Mathf.Floor(velocity * conversionRate) + " feet per second - Mass: " + mass / 0.001f + "g");
    }

    void Update()
    {
        // Aplica a força de sustentação
        Vector3 liftingForce = CalculateLiftingForce();
        rig.AddForce(liftingForce * Time.deltaTime, ForceMode.Acceleration);
    }

    Vector3 CalculateLiftingForce()
    {
        // Velocidade da BB
        float speed = rig.velocity.magnitude;

        // Força de sustentação simplificada
        float liftAmount = Mathf.Sqrt(speed) * backSpinDrag;

        // Aplicada na direção perpendicular à direção da BB
        return Vector3.up * liftAmount;
    }

    public float GetMass()
    {
        return mass;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 12)
            Destroy(gameObject);
    }
}
