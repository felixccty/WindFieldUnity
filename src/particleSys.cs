using System.Collections;
using UnityEngine;

public class particleSys : MonoBehaviour
{
    public ParticleSystem particles; // 引用粒子系统
    public float windStrength; // 风的强度

    private ParticleSystem.Particle[] m_Particles; // 存储粒子信息的数组

    void Start()
    {
        m_Particles = new ParticleSystem.Particle[particles.main.maxParticles]; // 初始化存储粒子信息的数组
    }

    void Update()
    {
        int numParticlesAlive = particles.GetParticles(m_Particles); // 获取当前活跃的粒子数量

        for (int i = 0; i < numParticlesAlive; i++)
        {
            // 根据风的方向和强度，更新粒子的位置
            m_Particles[i].position += Vector3.right * windStrength * Time.deltaTime;
        }

        particles.SetParticles(m_Particles, numParticlesAlive); // 更新粒子系统中的粒子信息
    }
}