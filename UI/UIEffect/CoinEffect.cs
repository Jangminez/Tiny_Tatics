using UnityEngine;

public class CoinEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem particleEffect;

    public void PlayEffect()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        particleEffect.Play();
        Destroy(gameObject, 1f);
    }
}
