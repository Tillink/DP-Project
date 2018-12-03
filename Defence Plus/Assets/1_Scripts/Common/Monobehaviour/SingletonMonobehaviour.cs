using UnityEngine;

// 하나만 존재해야 하는 Monobehaviour 클래스를 위한 부모클래스
public class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
{
    public static T Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = (T) this;
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnAwake()
    {

    }
}
