using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 퍼즐에 사용할 Particle 객체를 관리한는 클래스
public class ParticleManager : SingletonMonobehaviour<ParticleManager>
{
    // 파티클 객체들
    public List<GameObject> Particles;
    // 퍼즐 색깔별 사용할 파티클 Object Pool
    public Dictionary<PuzzleColor,ObjectPoolStack<GameObject>> particles = new Dictionary<PuzzleColor, ObjectPoolStack<GameObject>>();


	void Start ()
    {
        ObjectPoolSetting();
    }

    // 파티클 생성, 삭제 방식을 Object Pool로 변경
    private void ObjectPoolSetting()
    {
        for (int i = 0; i < Particles.Count; i++)
        {
            GameObject particle = Particles[i];

            particles.Add((PuzzleColor)i, new ObjectPoolStack<GameObject>(5, () =>
            {
                GameObject obj = Instantiate(particle, this.transform);
                obj.SetActive(false);
                obj.name = particle.name;

                return obj;
            }));
        }
    }

    // 필요한 곳에 파티클을 생성시킴(활성화)
    public void ShowParticle(PuzzleColor type, Transform parent)
    {
        GameObject particle = particles[type].GetObject();
        particle.transform.parent = parent;
        particle.transform.localPosition = Vector3.zero;
        particle.SetActive(true);
    }

    // 파티클 시간이 다하면 삭제(비활성화)
    public void RemoveParticle(string type, GameObject particle)
    {
        PuzzleColor color = (PuzzleColor) Enum.Parse(typeof(PuzzleColor), type);
        particle.transform.parent = transform;
        particles[color].ReturnObject(particle);
        particle.SetActive(false);
    }
}
