using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnRangeVisualizer : MonoBehaviour
{
    public static SpawnRangeVisualizer Instance;
    private GameObject spriteBox;

    public float minX = -25f;
    public float maxX = 25f;
    public float minZ = -15f;
    public float maxZ = 15f;
    public float y = 0.11f;

    public Sprite boxSprite;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            CreateBox();
        };
    }

    void Start()
    {
        CreateBox();
    }

    void CreateBox()
    {
        if (spriteBox != null)
        {
            Destroy(spriteBox);
            spriteBox = null;
        }

        float centerX = (minX + maxX) / 2f;
        float centerZ = (minZ + maxZ) / 2f;
        float sizeX = Mathf.Abs(maxX - minX);
        float sizeZ = Mathf.Abs(maxZ - minZ);

        spriteBox = new GameObject("SpawnRangeSpriteBox");
        spriteBox.transform.position = new Vector3(centerX, y, centerZ);

        var sr = spriteBox.AddComponent<SpriteRenderer>();
        sr.sprite = boxSprite;

        // ������ �����
        sr.color = new Color(0, 1, 0, 0.16f);

        float scaleX = sizeX / sr.sprite.bounds.size.x;
        float scaleZ = sizeZ / sr.sprite.bounds.size.y;
        spriteBox.transform.localScale = new Vector3(scaleX, 50, scaleZ);
        spriteBox.transform.rotation = Quaternion.Euler(90, 0, 0);

        spriteBox.SetActive(false);
    }

    public void Show()
    {
        if (spriteBox == null)
            CreateBox();
        if (spriteBox != null)
            spriteBox.SetActive(true);
    }

    public void Hide()
    {
        if (spriteBox != null)
            spriteBox.SetActive(false);
    }
}
