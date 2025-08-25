using UnityEngine;
using UnityEngine.Playables;

public class IntroEndHandler : MonoBehaviour
{
    [SerializeField] PlayableDirector director;

    void Start()
    {
        director.stopped += OnIntroEnd;
    }

    void OnIntroEnd(PlayableDirector director)
    {
        LoadSceneManager.Instance.LoadSceneAsync("MainScene");
    }
}
