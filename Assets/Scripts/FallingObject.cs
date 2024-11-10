using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private float forwardForce = 100f;
    [SerializeField] private float downwardForce = 200f;
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private SliceManager sliceManager;
    private Rigidbody rb;
    public Color ObjectColor { get; set; }

    private void Start()
    {
        InitializeComponents();
        ApplyInitialForce();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null) LogMissingComponent("Rigidbody");

        SetObjectColor();
    }

    private void ApplyInitialForce()
    {
        Vector3 initialForce = new Vector3(0, -downwardForce, forwardForce);
        rb?.AddForce(initialForce);
        rb.useGravity = true;
    }

    private void SetObjectColor()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = ObjectColor;
        }
    }

    private void LogMissingComponent(string componentName) =>
        Debug.LogError($"{componentName} component missing on {gameObject.name}");

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            TriggerGameOver();
        }
    }

    private void HandlePlayerCollision(Collision collision)
    {
        if (IsPlayerColorMatching(collision.gameObject))
        {
            GameManager.Instance.AddScore(1);
            sliceManager.SliceObject(this.gameObject, collision);
        }
        else
        {
            TriggerGameOver();
        }
    }

    private bool IsPlayerColorMatching(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            LogMissingComponent("Player Renderer");
            return false;
        }
        return playerController.GetRendererColor() == ObjectColor;
    }

    private void TriggerGameOver() => GameManager.Instance.GameOver();
}
