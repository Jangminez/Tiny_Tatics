using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 해당 컴포넌트를 가지고 있는 게임 오브젝트 반환
                instance = (T)FindAnyObjectByType(typeof(T));

                if (instance == null)
                {
                    // 새로운 게임 오브젝트 생성 및 컴포넌트 추가
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    // 새로운 오브젝트의 컴포넌트 인스턴스에 지정
                    instance = obj.GetComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        // 인스턴스가 없을 때 해당 오브젝트로 설정
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }

        // 인스턴스가 존재한다면 현재 오브젝트 파괴
        else if (instance != null)
            Destroy(gameObject);
    }
}
