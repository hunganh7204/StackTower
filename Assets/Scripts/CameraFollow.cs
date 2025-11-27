using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    private float smoothSpeed = 2f;
    private float targetY;

    void Start()
    {
        if (GameManager.instance.currentBlock != null)
        {
            offset = transform.position - GameManager.instance.currentBlock.position;
        }
        else
        {
            offset = new Vector3(0, 3, -5);
        }

        targetY = transform.position.y;
    }

    void Update()
    {
        if (GameManager.instance.currentBlock == null) return;

        float destinationY = GameManager.instance.currentBlock.position.y + offset.y;
        if (destinationY > targetY)
        {
            targetY = destinationY;
        }

        Vector3 newPos = new Vector3(transform.position.x, targetY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, smoothSpeed * Time.deltaTime);
    }
}