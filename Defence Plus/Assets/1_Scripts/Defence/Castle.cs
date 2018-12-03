using UnityEngine;

public class Castle : MonoBehaviour
{
    //성에 닿으면 몬스터를 없애고 라이프를 깎아줌
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Monster"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.TakeLife();
        }
    }
}
