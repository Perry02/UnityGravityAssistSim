using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/SebLague/Solar-System/blob/Episode_01/Assets/Scripts/Planets/NBodySimulation.cs
public class NBodySimulation : MonoBehaviour
{
    private AstroBody[] bodies;
    private static NBodySimulation instance;

    private void Awake() // initialize simulation
    {
        if (instance != null) { // to make sure no duplicates will exist
            Destroy(this);
            return;
        }
        Time.fixedDeltaTime = UniversalUnits.physicsTimeStep;
        ResetSimulation();
    }

    private void FixedUpdate() // main update cycle, only runs when the MonoBehaviour is enabled
    {
        for (int i = 0; i < bodies.Length; i++) // update velocity for all astro bodies
        {
            Vector3 acceleration = CalculateAcceleration(bodies[i].GetPosition, bodies[i]);
            bodies[i].UpdateVelocity(acceleration, UniversalUnits.simulationTimeStep);
        }

        for (int i = 0; i < bodies.Length; i++) // update position for all astro bodies
        {
            bodies[i].UpdatePosition(UniversalUnits.simulationTimeStep);
        }
    }

    public static Vector3 CalculateAcceleration(Vector3 position, AstroBody ignoreBody = null) // calculate acceleration based on Newton's law of universal gravitation and all other astro bodies
    {
        Vector3 acceleration = Vector3.zero;
        foreach (var body in Instance.bodies) 
        {
            if (body != ignoreBody) {
                acceleration += CalculateObjectsAcceleration(body.GetPosition, body.mass, position);
            }
        }

        return acceleration;
    }

    public static Vector3 CalculateObjectsAcceleration(Vector3 effectorPosition, float effectorMass, Vector3 effectedPosition) // calculate a single objects acceleration based on Newton's law of universal gravitation
    {
        float sqrDst = (effectorPosition - effectedPosition).sqrMagnitude; // the squared distance between the two bodies
        Vector3 forceDir = (effectorPosition - effectedPosition).normalized; 

        return (forceDir * UniversalUnits.gravitationalConstant * effectorMass / sqrDst) / UniversalUnits.floatLimitOffset;
    }

    private void OnEnable() // starts the simulation
    {
        bodies = FindObjectsOfType<AstroBody>();
    }

    private void OnDisable() // pauses the simulation
    {
        //TODO
    }

    public void ResetSimulation() // resets the simulation and all astrobodies
    {
        //TODO
    }

    public void DestroySimulation() // removes all astrobodies and disables the simulation
    {
        //TODO
    }



#region global values
    public static AstroBody[] Bodies 
    {
        get {return Instance.bodies;}
    }

    static NBodySimulation Instance 
    {
        get 
        {
            if (instance == null) 
            {
                instance = FindObjectOfType<NBodySimulation>();
            }
            return instance;
        }
    }
#endregion
}
