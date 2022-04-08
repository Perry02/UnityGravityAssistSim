using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/SebLague/Solar-System/blob/Episode_01/Assets/Scripts/Debug/OrbitDebugDisplay.cs
[ExecuteInEditMode]
public class OrbitVisualizer : MonoBehaviour
{
    public int numSteps = 1000;
    public float timeStep = 0.1f;
    public bool useSimulatorTimeStep = false;

    public AstroBody target;
    public bool relativeToTarget;
    [Header("pathSettings")]
    public float width = 100f;
    public bool useThickLines;

    // Simulation reference
    AstroBody[] bodies = NBodySimulation.Bodies;
    VirtualBody[] virtualBodies;
    // Path drawing
    Vector3[][] drawPoints;

    private void Update()
    {
        DrawOrbits();
    }

    private void DrawOrbits()
    {
        if (Application.isPlaying) 
        {
            bodies = NBodySimulation.Bodies; // retrives the current astrobodies in simulation
        } else
        {
            bodies = FindObjectsOfType<AstroBody>();
        }
        virtualBodies = new VirtualBody[bodies.Length];
        drawPoints = new Vector3[bodies.Length][];
        int referenceFrameIndex = 0;
        Vector3 referenceBodyInitialPosition = Vector3.zero;

        // ! initialize virtual bodies (don't want to move the actual bodies)
        for (int i = 0; i < virtualBodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(bodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            if (bodies[i] == target && relativeToTarget){
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualBodies[i].position;
            }
        }

        // ! simulate
        for (int step = 0; step < numSteps; step++)
        {
            Vector3 referenceBodyPosition = (relativeToTarget) ? virtualBodies[referenceFrameIndex].position : Vector3.zero;
            // Update velocities
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                virtualBodies[i].velocity += CalculateAcceleration(i, virtualBodies) * timeStep;
            }
            // Update positions
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
                virtualBodies[i].position = newPos;
                if (relativeToTarget) {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    newPos -= referenceFrameOffset;
                }
                if (relativeToTarget && i == referenceFrameIndex) {
                    newPos = referenceBodyInitialPosition;
                }
                drawPoints[i][step] = newPos / UniversalUnits.floatLimitOffset;
            }
        }

        // ! Draw paths
        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++)
        {
            var pathColor = bodies[bodyIndex].gameObject.GetComponent<MeshRenderer>().sharedMaterial.color;

            if (useThickLines) {
                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                lineRenderer.enabled = true;
                lineRenderer.positionCount = drawPoints[bodyIndex].Length;
                lineRenderer.SetPositions(drawPoints[bodyIndex]);
                lineRenderer.startColor = pathColor;
                lineRenderer.endColor = pathColor;
                lineRenderer.widthMultiplier = width;
            } 
            else {
                for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
                {
                    Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColor);
                }

                // Hide renderer
                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                if (lineRenderer) {
                    lineRenderer.enabled = false;
                }
            }
        }
    }

    private void SimulateOrbitsAnalytically()
    {
        //TODO
    }

        private void SimulateOrbitsIteratively()
    {
        //TODO
    }

    private void HideOrbits()
    {
        AstroBody[] bodies = FindObjectsOfType<AstroBody>();

        for (int bodyIndex = 0; bodyIndex < bodies.Length; bodyIndex++) {
            var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer> ();
            lineRenderer.positionCount = 0;
        }
    }

    Vector3 CalculateAcceleration (int i, VirtualBody[] virtualBodies) 
    {
        Vector3 acceleration = Vector3.zero;
        for (int j = 0; j < virtualBodies.Length; j++)
        {
            if (i == j)
                continue; //skip calculating the acceleration on the very same object.

            acceleration += NBodySimulation.CalculateObjectsAcceleration(virtualBodies[j].position, virtualBodies[j].mass, virtualBodies[i].position);
        }

        return acceleration;
    }



    private class VirtualBody 
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public VirtualBody (AstroBody body)
        {
            if (Application.isPlaying) 
            {
                position = body.GetPosition;
                velocity = body.GetVelocity;
            } else
            {
               position = body.GetPosition;
               velocity = body.GetVelocity;
            }
            mass = body.mass;
        }
    }
}


// setting object for lines
public class LineObject 
{
    public float width = 100f;
    public bool useThickLines;
}