using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/SebLague/Solar-System/blob/Episode_01/Assets/Scripts/Planets/CelestialBody.cs
[ExecuteInEditMode] //TODO remove later
[RequireComponent (typeof (Rigidbody))]
public class AstroBody : MonoBehaviour
{
    public float mass;
    public float radius;
    public float distance;
    public float angle;
    public string bodyName = "unNamed";
    public float initialVelocity;

    private Rigidbody rb;
    private Vector3 velocity;
    private Vector3 initialPosition;
    private Vector3 position;

    private void Awake() // initial values
    {
        rb = this.GetComponent<Rigidbody>();
        rb.mass = mass;
        velocity = new Vector3(-Mathf.Sin(angle)*initialVelocity, 0f, Mathf.Cos(angle)*initialVelocity);
        this.gameObject.name = name;
        position = new Vector3(Mathf.Cos(angle)*distance, 0f, Mathf.Sin(angle)*distance);
        this.transform.position = position / UniversalUnits.floatLimitOffset;
        initialPosition = this.transform.position;
    }

    private void Update()
    {
        if (!Application.isPlaying) 
        {
            position = new Vector3(Mathf.Cos(angle)*distance, 0f, Mathf.Sin(angle)*distance);
            this.transform.position = position / UniversalUnits.floatLimitOffset;
            initialPosition = this.transform.position;
            velocity = new Vector3(-Mathf.Sin(angle)*initialVelocity, 0f, Mathf.Cos(angle)*initialVelocity);
        }
    }

    public void UpdateVelocity(Vector3 acceleration, float timeStep) // update velocity based on acceleration
    {
        velocity += acceleration * timeStep;
    }

    public void UpdatePosition(float timeStep) // Update position based on velocity
    {
        position = position + velocity * timeStep;
        rb.MovePosition(position / UniversalUnits.floatLimitOffset);
    }

    public void ResetAstroBody(bool updateInitialPosition) // resets astrobody back to initial values
    {
        velocity = new Vector3(-Mathf.Sin(angle)*initialVelocity, 0f, Mathf.Cos(angle)*initialVelocity);
        rb.mass = mass;
        if (updateInitialPosition)
            initialPosition = this.transform.position;
        else
            this.transform.position = initialPosition;
    }



    public Vector3 GetPosition {get {return position;}}

    public Vector3 GetVelocity {get{return velocity;}}

    public float GetMass {get{return mass / UniversalUnits.floatLimitOffset;}}
}