using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Sprite[] backgroundSprites = new Sprite[5];
    [SerializeField]
    private Parallax[] _parallaxes = new Parallax[5];

    public void Init(Sprite[] sprites = null)
    {
        if (sprites == null)
            sprites = backgroundSprites;

        for (int i = 0; i < transform.childCount; i++)
        {
            var background = transform.GetChild(i).GetComponent<Background>();

            background.SetSprite(sprites[i]);

            if (background.transform.childCount > 0)
            {
                background.transform.GetChild(0).GetComponent<Background>().SetSprite(sprites[i]);
                background.transform.GetChild(1).GetComponent<Background>().SetSprite(sprites[i]);
            }
        }

        foreach (var parallax in _parallaxes)
            parallax.Init();
    }
}
