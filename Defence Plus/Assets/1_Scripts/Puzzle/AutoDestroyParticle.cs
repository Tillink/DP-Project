using UnityEngine;

// 파티클이 시간이 지나면 자연스레 Manager에게 반환되도록 함
public class AutoDestroyParticle : MonoBehaviour 
{
    // When Active true
    void OnEnable()
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        Invoke("ReturnParticle", particle.main.duration);
    }

    // When Active false
    void OnDisable()
    {
        CancelInvoke("ReturnParticle");
    }
    
    // 파티클을 비활성화 처리
    private void ReturnParticle()
    {
        string particleType = gameObject.name.Replace("Effects", "");
        ParticleManager.Instance.RemoveParticle(particleType, this.gameObject);
    }
}
