using UnityEngine;

public class Background : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();
    public void SetSprite(Sprite sprite) => _spriteRenderer.sprite = sprite;
    public float GetLength() => _spriteRenderer.bounds.size.x;
}
