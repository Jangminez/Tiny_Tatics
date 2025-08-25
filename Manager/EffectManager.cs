using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    private readonly Dictionary<string, ParticleSystem> _cache = new();

    [Header("Optional: Assign manually if not using Camera.main")]
    [SerializeField] private Camera uiCamera;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayEffect(string path, Vector3 position, Quaternion? rotation = null, Transform parent = null)
    {
        if (!_cache.TryGetValue(path, out ParticleSystem prefab))
        {
            prefab = Resources.Load<ParticleSystem>(path);
            if(prefab == null)
            {
                return;
            }
            _cache[path] = prefab;
        }

        ParticleSystem particleSystem = Instantiate(prefab, position, rotation ?? Quaternion.identity, parent);
        particleSystem.Play();

        Destroy(particleSystem.gameObject, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
    }

    public void PlayUIEffect(string path, Vector2 screenPosition)
    {
        if(uiCamera == null)
        {
            return;
        }

        Vector3 worldPos = uiCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
        PlayEffect(path, worldPos);
    }
}
