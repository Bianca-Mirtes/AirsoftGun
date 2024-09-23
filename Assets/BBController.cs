using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBController : MonoBehaviour
{
    [SerializeField] private float backSpinDrag = 0.001f;
    [SerializeField] private float liftingForce;
    [SerializeField] private float velocity = 2f;

    private float energyJoules = 1.49f;
    private Rigidbody rig;

    // 3,281 - multiplica m/s por isso para chegar em pés
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        liftingForce = Mathf.Sqrt(velocity) * backSpinDrag * Time.deltaTime;
        rig.AddForce(Vector3.right*liftingForce);
    }
}
