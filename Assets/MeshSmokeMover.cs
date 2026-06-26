using UnityEngine;

public class MeshSmokeMover : MonoBehaviour
{
    public float riseSpeed = 0.5f;
    public float maxHeight = 4f;
    public float swayAmount = 0.3f;
    public float swaySpeed = 1f;

    private Vector3 startPos;
    private Vector3 startScale;
    private float randomOffset;

    void Start()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;
        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        transform.localPosition += Vector3.up * riseSpeed * Time.deltaTime;

        float swayX = Mathf.Sin(Time.time * swaySpeed + randomOffset) * swayAmount;
        transform.localPosition = new Vector3(
            startPos.x + swayX,
            transform.localPosition.y,
            startPos.z
        );

        float heightPercent = (transform.localPosition.y - startPos.y) / maxHeight;
        transform.localScale = Vector3.Lerp(startScale, startScale * 1.5f, heightPercent);

        if (transform.localPosition.y >= startPos.y + maxHeight)
        {
            transform.localPosition = startPos;
            transform.localScale = startScale;
        }
    }
}