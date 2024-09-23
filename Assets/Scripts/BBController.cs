using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBController : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float backSpinDrag = 0.001f;
    [SerializeField] private float liftingForce;
    [Tooltip("Weight in grams")] //(em gramas)
    [SerializeField] [Range(0.2f, 2)] private float mass = 0.2f;
    private float velocity = 0f;

    private float energyInJoules = 1.49f;
    private float conversionRate = 3.281f; //multiplica m/s por isso para chegar em pés
    private Rigidbody rig;
    private Vector3 localForward;

    void Start()
    {
        rig = GetComponent<Rigidbody>();

        //convertendo de gramas para quilos (importantissimo)
        mass = mass * 0.001f; 

        rig.mass = mass;
        velocity = Mathf.Sqrt((2 * energyInJoules)/mass);

        localForward = transform.parent.forward;
        rig.AddForce(velocity * localForward, ForceMode.Force);

        //Debug
        Debug.Log("\nVelocity: " + Mathf.Floor(velocity * conversionRate) + " feet per second - Mass: "+ mass/0.001f + "g");
    }

    void Update()
    {
        if (gameObject.transform.position.y > 0) { //SE NAO ATINGIU O CHAO
            liftingForce = Mathf.Sqrt(velocity) * backSpinDrag;
            rig.AddForce(liftingForce * localForward, ForceMode.Force);
        }else
            Destroy(gameObject);
    }
}
