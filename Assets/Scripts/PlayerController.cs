using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Renderer swordMeshRenderer;
    private readonly Color[] colors = { Color.red, Color.blue, Color.green };
    private int currentColorIndex = 0;
    private float horizontalInput;
    private Vector3 startPosition;

    private void Start()
    {
        SetInitialColor();
        startPosition = gameObject.transform.position;
    }

    private void Update()
    {
        HandleInput();

        if (GameManager.Instance != null && GameManager.Instance.IsGameActive)
        {
            HandleColorChange();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameActive)
        {
            HandleMovement();
        }

    }

    private void SetInitialColor()
    {
        swordMeshRenderer.material.color = colors[currentColorIndex];
    }
    private void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }


    private void HandleMovement()
    {
        Vector3 movement = CalculateMovement(horizontalInput);
        MovePlayer(movement);
    }

    private Vector3 CalculateMovement(float input)
    {
        return new Vector3(input * moveSpeed * Time.deltaTime, 0f, 0f);
    }

    private void MovePlayer(Vector3 movement)
    {

        transform.Translate(movement);
    }

    private void HandleColorChange()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeColor();
        }
    }

    private void ChangeColor()
    {
        currentColorIndex = GetNextColorIndex();
        ApplyColor(currentColorIndex);
    }

    private int GetNextColorIndex()
    {
        return (currentColorIndex + 1) % colors.Length;
    }

    private void ApplyColor(int colorIndex)
    {
        swordMeshRenderer.material.color = colors[colorIndex];
    }

    public Color GetRendererColor()
    {
        return swordMeshRenderer.material.color;
    }

    public void ResetPlayerPostion()
    {
        gameObject.transform.position = startPosition;
    }
}
