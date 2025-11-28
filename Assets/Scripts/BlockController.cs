using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float moveRange = 3.5f;

    private bool isMoving = true;
    private bool hasDropped = false;
    private Vector3 startPos;
    public Transform previousBlock;

    void Start()
    {
        startPos = transform.position;
        if (previousBlock == null)
        {
            previousBlock = GameManager.instance.currentBlock;
        }
    }

    void Update()
    {
        if (!isMoving || GameManager.instance.isGameOver) return;
        float x = Mathf.PingPong(Time.time * moveSpeed, moveRange * 2f) - moveRange;
        transform.position = new Vector3(x, startPos.y, startPos.z);
    }

    public void Drop()
    {
        if (!isMoving) return;
        if (hasDropped) return;
        hasDropped = true;
        isMoving = false;
        StartCoroutine(HandleLanding());
    }

    IEnumerator HandleLanding()
    {
        yield return new WaitForSeconds(0.05f);
        if (previousBlock == null)
        {
            MakeThisFall();
            GameManager.instance.SetGameOver();
            yield break;
        }

        float prevX = previousBlock.position.x;
        float prevWidth = previousBlock.localScale.x;
        float curX = transform.position.x;
        float curWidth = transform.localScale.x;

        float prevLeft = prevX - prevWidth / 2f;
        float prevRight = prevX + prevWidth / 2f;
        float curLeft = curX - curWidth / 2f;
        float curRight = curX + curWidth / 2f;

        float overlapLeft = Mathf.Max(prevLeft, curLeft);
        float overlapRight = Mathf.Min(prevRight, curRight);
        float overlap = overlapRight - overlapLeft;

        if (overlap <= 0f)
        {
            MakeThisFall();
            GameManager.instance.SetGameOver();
            yield break;
        }

        float newSize = overlap;
        float newCenter = (overlapLeft + overlapRight) / 2f;

        float leftPieceWidth = overlapLeft - curLeft;
        float rightPieceWidth = curRight - overlapRight;

        if (leftPieceWidth > 0.01f)
        {
            float centerLeft = (curLeft + overlapLeft) / 2f;
            SpawnFallingPiece(centerLeft, leftPieceWidth);
        }

        if (rightPieceWidth > 0.01f)
        {
            float centerRight = (overlapRight + curRight) / 2f;
            SpawnFallingPiece(centerRight, rightPieceWidth);
        }

        transform.localScale = new Vector3(newSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newCenter, transform.position.y, transform.position.z);

        GameManager.instance.OnBlockLanded();
    }

    void MakeThisFall()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 2f;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    void SpawnFallingPiece(float centerX, float width)
    {
        if (width <= 0.01f) return;
        GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        piece.transform.localScale = new Vector3(width, transform.localScale.y, transform.localScale.z);
        piece.transform.position = new Vector3(centerX, transform.position.y + 0.06f, transform.position.z);
        Renderer src = GetComponent<Renderer>();
        Renderer r = piece.GetComponent<Renderer>();
        if (src != null && r != null)
        {
            r.material = new Material(src.sharedMaterial);
        }
        Rigidbody rb = piece.AddComponent<Rigidbody>();
        rb.mass = 1f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Destroy(piece, 2f);
    }
}
