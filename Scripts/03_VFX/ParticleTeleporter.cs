using UnityEngine;

public class ParticleTeleporter : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Vector2 bounds = new Vector2(15f, 10f);

    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        if (targetCamera == null)
            targetCamera = Camera.main;

        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void LateUpdate()
    {
        int particleCount = ps.GetParticles(particles);

        Vector3 cameraPos = targetCamera.transform.position;

        for (int i = 0; i < particleCount; i++)
        {
            Vector3 particlePos = particles[i].position;

            if (particlePos.x < cameraPos.x - bounds.x)
            {
                particlePos.x = cameraPos.x + bounds.x;
            }
            else if (particlePos.x > cameraPos.x + bounds.x)
            {
                particlePos.x = cameraPos.x - bounds.x;
            }

            if (particlePos.y < cameraPos.y - bounds.y)
            {
                particlePos.y = cameraPos.y + bounds.y;
            }
            else if (particlePos.y > cameraPos.y + bounds.y)
            {
                particlePos.y = cameraPos.y - bounds.y;
            }

            particles[i].position = particlePos;
        }

        ps.SetParticles(particles, particleCount);
    }
}