using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public float spawnYIncrement = 0.7f;
    public float initialSpawnY = 1.5f;

    private int blockIndex = 0;

    void Start()
    {
        if (blockPrefab == null && GameManager.instance != null)
        {
            blockPrefab = GameManager.instance.blockPrefab;
        }
    }

    public void SpawnInitial()
    {
        if (blockPrefab == null) return;
        if (GameManager.instance.groundObject == null)
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.transform.position = new Vector3(0f, 0f, 0f);
            ground.transform.localScale = new Vector3(10f, 1f, 3f);
            ground.GetComponent<Renderer>().material.color = Color.gray;
            ground.name = "Ground";
            GameManager.instance.groundObject = ground.transform;
        }
        Vector3 pos = new Vector3(0f, initialSpawnY, 0f);
        GameObject go = Instantiate(blockPrefab, pos, Quaternion.identity);
        go.name = "Block_0";
        go.transform.localScale = new Vector3(3f, 0.5f, 3f);
        Renderer r = go.GetComponent<Renderer>();
        if (r != null) r.material.color = Color.HSVToRGB(0.5f, 0.7f, 0.9f);
        if (go.GetComponent<Rigidbody>() != null) Destroy(go.GetComponent<Rigidbody>());
        GameManager.instance.currentBlock = go.transform;
    }

    public void SpawnNext()
    {
        if (blockPrefab == null) return;
        Transform prev = GameManager.instance.currentBlock;
        if (prev == null) return;
        float nextY = prev.position.y + spawnYIncrement;
        Vector3 spawnPos = new Vector3(0f, nextY, prev.position.z);
        GameObject go = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
        blockIndex++;
        go.name = "Block_" + blockIndex;
        go.transform.localScale = prev.localScale;
        Renderer r = go.GetComponent<Renderer>();
        if (r != null) r.material.color = Color.HSVToRGB(Random.value, 0.7f, 0.9f);
        if (go.GetComponent<Rigidbody>() != null) Destroy(go.GetComponent<Rigidbody>());
        BlockController bc = go.GetComponent<BlockController>();
        if (bc != null) bc.previousBlock = prev;
        GameManager.instance.currentBlock = go.transform;
    }
}
