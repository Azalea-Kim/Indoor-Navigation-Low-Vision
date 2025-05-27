using UnityEngine;
using System.Collections;

public class RippleParticleEffect : MonoBehaviour
{
    [Header("Target & Camera References")]
    [SerializeField] private Transform target;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private ParticleSystem rippleParticles;

    [Header("Detection Settings")]
    [SerializeField] private float inViewAngle = 30f;
    [SerializeField] private float nearFrustumAngle = 45f;
    [SerializeField] private float distanceFromCamera = 2f;

    [Header("Circle Ripple Settings")]
    [SerializeField] private float travelSpeed = 2f;
    [SerializeField] private float circleLifetime = 2f;
    [SerializeField] private Color inViewColor = Color.green;
    [SerializeField] private float startSize = 0.1f;       // thinner
    [SerializeField] private float endSize = 0.5f;         // thinner

    [Header("Auto Trigger Settings")]
    [SerializeField] private bool autoTriggerWhenOutOfView = true;
    [SerializeField] private float autoTriggerInterval = 1f;

    [Header("Animation Settings")]
    [SerializeField] private AnimationCurve sizeOverLifetime = AnimationCurve.EaseInOut(0, 0.3f, 1, 1.5f);
    [SerializeField] private AnimationCurve alphaOverLifetime = AnimationCurve.EaseInOut(0, 1f, 1, 0f);
    [SerializeField] private bool showDebugInfo = true;
    private float lastEmitTime = 0f;
    [SerializeField] private float emitCooldown = 1f;
    [SerializeField] private int emitBatchCount = 3;          // How many each batch
    [SerializeField] private float restCooldown = 2f;         // rest time between two batches
    private bool isTargetInView;
    private bool isTargetInFront;
    private bool lastInViewState;
    private Vector3 rippleDirection;
    private float totalDistance;
    public int maxParticle = 5;
    private Vector3 batchStartPos;
    private Vector3 batchEndPos;
    private float batchTotalDistance;


    private Coroutine batchCoroutine;

    void Start()
    {
        SetupParticleSystem();


       //InvokeRepeating(nameof(CheckAutoTrigger), 0f, autoTriggerInterval);

    }

    void SetupParticleSystem()
    {
        if (rippleParticles == null)
        {
            Debug.LogError("Particle System not assigned!");
            return;
        }

        var main = rippleParticles.main;
        main.startLifetime = circleLifetime;
        main.startSpeed = 0;
        main.startSize = startSize;
        main.startColor = inViewColor;
        main.maxParticles = maxParticle;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = rippleParticles.emission;
        emission.enabled = false;

        var shape = rippleParticles.shape;
        shape.enabled = false;

        var sizeOverLifetimeModule = rippleParticles.sizeOverLifetime;
        sizeOverLifetimeModule.enabled = true;
        sizeOverLifetimeModule.size = new ParticleSystem.MinMaxCurve(endSize / startSize, sizeOverLifetime);

        var colorOverLifetime = rippleParticles.colorOverLifetime;
        colorOverLifetime.enabled = true;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(inViewColor, 0.0f),
                new GradientColorKey(inViewColor, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0f, 0.0f),
                new GradientAlphaKey(1.0f, 0.05f),
                new GradientAlphaKey(1.0f, 0.95f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        colorOverLifetime.color = gradient;
    }

    void Update()
    {
        UpdateTargetDetection();
        //HandleViewStateChanges();
        //CheckAutoTrigger();

        if (isTargetInView && batchCoroutine == null)
        {
            batchCoroutine = StartCoroutine(EmitInBatches());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TriggerSingleCircle();
        }
    }

    private IEnumerator EmitInBatches()
    {
        while (isTargetInView)
        {
            UpdateTargetDetection();
            batchStartPos = target.position;
            //batchEndPos = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
            batchEndPos = mainCamera.transform.position;
            batchTotalDistance = Vector3.Distance(batchStartPos, batchEndPos);

            for (int i = 0; i < emitBatchCount; i++)
            {
                
                TriggerSingleCircle();
                yield return new WaitForSeconds(emitCooldown);

                // If the target gets out of view , stop
                //UpdateTargetDetection();
                if (!isTargetInView)
                {
                    batchCoroutine = null;
                    yield break;
                }
            }

            yield return new WaitForSeconds(restCooldown);

         
            UpdateTargetDetection();
            if (!isTargetInView)
            {
                batchCoroutine = null;
                yield break;
            }
        }

        batchCoroutine = null;
    }


    void UpdateTargetDetection()
    {
        if (target == null || mainCamera == null) return;

        Vector3 toTarget = target.position - mainCamera.transform.position;
        Vector3 camForward = mainCamera.transform.forward;
        float angleToTarget = Vector3.Angle(mainCamera.transform.forward, toTarget);

        isTargetInFront = Vector3.Dot(camForward, toTarget) > 0;
        isTargetInView = isTargetInFront && (angleToTarget < inViewAngle);

        rippleDirection = toTarget.normalized;
        totalDistance = toTarget.magnitude;
    }

    void HandleViewStateChanges()
    {
        if (lastInViewState != isTargetInView)
        {
            lastInViewState = isTargetInView;

            if (isTargetInView && autoTriggerWhenOutOfView)  // ✅ only when in view
            {
                TriggerSingleCircle();
            }
        }
    }

    void CheckAutoTrigger()
    {
        if (isTargetInView && Time.time - lastEmitTime > emitCooldown)
        {
            TriggerSingleCircle();
            lastEmitTime = Time.time;
        }
    }

    public void TriggerSingleCircle1()
    {
        StartCoroutine(MoveSingleCircle());
    }
    public void TriggerSingleCircle()
    {
        if (target == null || mainCamera == null) return;

        Vector3 startPos = target.position;
        Vector3 direction = (mainCamera.transform.position - startPos).normalized;

        var velocityModule = rippleParticles.velocityOverLifetime;
        velocityModule.enabled = true;
        velocityModule.space = ParticleSystemSimulationSpace.World;
        velocityModule.x = new ParticleSystem.MinMaxCurve(direction.x * travelSpeed);
        velocityModule.y = new ParticleSystem.MinMaxCurve(direction.y * travelSpeed);
        velocityModule.z = new ParticleSystem.MinMaxCurve(direction.z * travelSpeed);

        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = startPos;
        emitParams.velocity = Vector3.zero; // initial velocity will be overwritten by module
        emitParams.startLifetime = circleLifetime;
        emitParams.startSize = startSize;
        emitParams.startColor = inViewColor;

        rippleParticles.Emit(emitParams, 1);
    }

    private IEnumerator MoveSingleCircle()
    {
        if (target == null || mainCamera == null) yield break;

        Color currentColor = inViewColor;

        Vector3 startPos = batchStartPos;
        Vector3 endPos = batchEndPos;
        float journeyTime = batchTotalDistance / travelSpeed;

        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = startPos;
        emitParams.velocity = Vector3.zero;
        emitParams.startLifetime = circleLifetime;
        emitParams.startSize = startSize;
        emitParams.startColor = currentColor;

        rippleParticles.Emit(emitParams, 1);

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[rippleParticles.main.maxParticles];
        int numParticles = rippleParticles.GetParticles(particles);

        if (numParticles > 0)
        {
            int newestParticleIndex = numParticles - 1;
            float startTime = Time.time;
            float endTime = startTime + journeyTime;

            while (Time.time < endTime && Time.time - startTime < circleLifetime)
            {
                float progress = Mathf.InverseLerp(startTime, endTime, Time.time);
                Vector3 currentPos = Vector3.Lerp(startPos, endPos, progress);
                particles[newestParticleIndex].position = currentPos;

                float sizeMultiplier = sizeOverLifetime.Evaluate(progress);
                particles[newestParticleIndex].startSize = startSize * sizeMultiplier;

                Color particleColor = currentColor;
                particleColor.a = alphaOverLifetime.Evaluate(progress);
                particles[newestParticleIndex].startColor = particleColor;

                rippleParticles.SetParticles(particles, numParticles);

                yield return null;

                numParticles = rippleParticles.GetParticles(particles);
                if (numParticles <= newestParticleIndex) break;
            }
        }
    }


    private IEnumerator MoveSingleCircle1()
    {
        if (target == null || mainCamera == null) yield break;

        //UpdateTargetDetection();
        Color currentColor = inViewColor;

       // Vector3 startPos = target.position;
        //Vector3 endPos = mainCamera.transform.position + (mainCamera.transform.forward * distanceFromCamera);

        // 不再计算 startPos 和 endPos
        Vector3 startPos = batchStartPos;
        Vector3 endPos = batchEndPos;
        float journeyTime = batchTotalDistance / travelSpeed;


        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = startPos;
        emitParams.velocity = Vector3.zero;
        emitParams.startLifetime = circleLifetime;
        emitParams.startSize = startSize;
        emitParams.startColor = currentColor;

        rippleParticles.Emit(emitParams, 1);

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[rippleParticles.main.maxParticles];
        int numParticles = rippleParticles.GetParticles(particles);

        if (numParticles > 0)
        {
            int newestParticleIndex = numParticles - 1;
            float elapsedTime = 0f;
            //float journeyTime = totalDistance / travelSpeed;

            while (elapsedTime < journeyTime && elapsedTime < circleLifetime)
            {
                float progress = elapsedTime / journeyTime;
                Vector3 currentPos = Vector3.Lerp(startPos, endPos, progress);
                particles[newestParticleIndex].position = currentPos;

                float sizeMultiplier = sizeOverLifetime.Evaluate(progress);
                particles[newestParticleIndex].startSize = startSize * sizeMultiplier;

                Color particleColor = currentColor;
                particleColor.a = alphaOverLifetime.Evaluate(progress);
                particles[newestParticleIndex].startColor = particleColor;

                rippleParticles.SetParticles(particles, numParticles);

                elapsedTime += Time.deltaTime;
                yield return null;

                numParticles = rippleParticles.GetParticles(particles);
                if (numParticles <= newestParticleIndex) break;
            }
        }
    }

    public void TriggerAttentionPulse()
    {
        StartCoroutine(CreateAttentionPulse());
    }

    private IEnumerator CreateAttentionPulse()
    {
        UpdateTargetDetection();

        Vector3 pulsePos = mainCamera.transform.position + (mainCamera.transform.forward * distanceFromCamera);
        Color pulseColor = inViewColor;

        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = pulsePos;
        emitParams.velocity = Vector3.zero;
        emitParams.startLifetime = 1f;
        emitParams.startSize = startSize * 2f;
        emitParams.startColor = pulseColor;

        rippleParticles.Emit(emitParams, 1);

        yield return null;
    }

    public bool IsTargetInView() => isTargetInView;
    public bool IsTargetInFront() => isTargetInFront;

    public float GetAngleToTarget()
    {
        if (target == null || mainCamera == null) return 0f;
        Vector3 toTarget = target.position - mainCamera.transform.position;
        return Vector3.Angle(mainCamera.transform.forward, toTarget);
    }

    void OnDrawGizmos()
    {
        if (!showDebugInfo || target == null || mainCamera == null) return;

        Gizmos.color = isTargetInView ? Color.green : Color.red;
        Gizmos.DrawLine(target.position, mainCamera.transform.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * 3f);

        Gizmos.color = Color.yellow;
        Vector3 leftBoundary = Quaternion.AngleAxis(-inViewAngle, mainCamera.transform.up) * mainCamera.transform.forward;
        Vector3 rightBoundary = Quaternion.AngleAxis(inViewAngle, mainCamera.transform.up) * mainCamera.transform.forward;
        Gizmos.DrawRay(mainCamera.transform.position, leftBoundary * 5f);
        Gizmos.DrawRay(mainCamera.transform.position, rightBoundary * 5f);

        Gizmos.color = isTargetInView ? Color.green : Color.red;
        Gizmos.DrawWireSphere(target.position, 0.2f);

        Vector3 endPos = mainCamera.transform.position + (mainCamera.transform.forward * distanceFromCamera);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(endPos, 0.1f);
    }
}
