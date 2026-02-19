using UnityEngine;
using TMPro;

public class Cube : MonoBehaviour
{
    public int Value { get; private set; }

    [SerializeField] private float xLimit = 1.7f;
    [SerializeField] private float collisionPower = 0.15f;
    [SerializeField] private float upwardPower = 0.08f;
    [SerializeField] private float maxAngularSpeed = 8f;

    private Rigidbody rb;
    private bool isMerging;

    private TextMeshPro[] texts;
    private BoxCollider box;
    private Renderer cubeRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        texts = GetComponentsInChildren<TextMeshPro>();
        box = GetComponent<BoxCollider>();
        cubeRenderer = GetComponent<Renderer>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        rb.mass = 1f;
        rb.linearDamping = 0.1f;
        rb.angularDamping = 2f;

        rb.maxAngularVelocity = maxAngularSpeed;

        rb.centerOfMass = new Vector3(0, -0.2f, 0);
    }

    private float HalfWidth =>
        (box.size.x * transform.localScale.x) * 0.5f;

    public void Initialize(int value)
    {
        Value = value;
        isMerging = false;

        rb.angularVelocity = Vector3.zero;

        UpdateVisual();
        UpdateColor();
    }

    public void MoveHorizontal(float xPosition)
    {
        float min = -xLimit + HalfWidth;
        float max = xLimit - HalfWidth;

        float clampedX = Mathf.Clamp(xPosition, min, max);

        Vector3 pos = rb.position;
        pos.x = clampedX;
        rb.MovePosition(pos);
    }

    public void Launch(float force)
    {
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;

        rb.AddForce(Vector3.forward * force * 2.38f, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        float min = -xLimit + HalfWidth;
        float max = xLimit - HalfWidth;

        Vector3 pos = rb.position;
        pos.x = Mathf.Clamp(pos.x, min, max);
        rb.position = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Cube other = collision.collider.GetComponent<Cube>();

        if (other != null)
        {
            float impact = collision.relativeVelocity.magnitude;

            Vector3 dir = collision.contacts[0].normal;

            Vector3 impulse =
                -dir * impact * collisionPower +
                Vector3.up * impact * upwardPower;

            rb.AddForce(impulse, ForceMode.Impulse);
        }

        if (isMerging) return;
        if (other == null) return;
        if (other.Value != Value) return;
        if (other.isMerging) return;

        if (GetInstanceID() > other.GetInstanceID())
            return;

        Merge(other);
    }

    private void Merge(Cube other)
    {
        isMerging = true;
        other.isMerging = true;

        Vector3 combinedVelocity = rb.linearVelocity + other.rb.linearVelocity;

        int newValue = Value * 2;

        Cube newCube = Instantiate(this, transform.position, Quaternion.identity);
        newCube.Initialize(newValue);

        Rigidbody newRb = newCube.GetComponent<Rigidbody>();

        newRb.linearVelocity = combinedVelocity;

        float mergeJump = 2.5f;
        float sideChaos = 1.1f;
        float spinChaos = 1.8f;

        Vector3 randomSide = new Vector3(
            Random.Range(-sideChaos, sideChaos),
            0,
            Random.Range(-sideChaos, sideChaos)
        );

        newRb.AddForce(Vector3.up * mergeJump, ForceMode.Impulse);
        newRb.AddForce(randomSide, ForceMode.Impulse);
        newRb.AddTorque(Random.insideUnitSphere * spinChaos, ForceMode.Impulse);

        ScoreManager.Instance.AddScore(newValue / 2);

        Destroy(other.gameObject);
        Destroy(gameObject);
    }


    private void UpdateVisual()
    {
        foreach (var t in texts)
            t.text = Value.ToString();
    }

    private void UpdateColor()
    {
        cubeRenderer.material.color = GetColorByValue(Value);
    }

    private Color GetColorByValue(int value)
    {
        switch (value)
        {
            case 2: return new Color(0.9f, 0.9f, 0.9f);
            case 4: return new Color(0.8f, 0.8f, 0.6f);
            case 8: return new Color(0.95f, 0.6f, 0.3f);
            case 16: return new Color(0.9f, 0.4f, 0.2f);
            case 32: return new Color(0.8f, 0.3f, 0.2f);
            case 64: return new Color(0.7f, 0.2f, 0.2f);
            case 128: return new Color(0.6f, 0.5f, 0.2f);
            case 256: return new Color(0.5f, 0.4f, 0.7f);
            case 512: return new Color(0.4f, 0.3f, 0.8f);
            case 1024: return new Color(0.3f, 0.2f, 0.9f);
            case 2048: return new Color(1f, 0.8f, 0.1f);
            default: return Color.black;
        }
    }
}
