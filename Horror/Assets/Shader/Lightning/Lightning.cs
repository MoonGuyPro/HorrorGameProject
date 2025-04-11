using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public ParticleSystem lightningParticleSystem;
    public ParticleSystem flashParticleSystem;
    public ParticleSystem glowParticleSystem;
    public ParticleSystem flashOnImpactParticleSystem;
    public ParticleSystem sparksParticleSystem;

    public Color lightningColor;
    public Color flashColor;
    public AnimationCurve widthCurve;

    public float lightningLifespawn;
    public float flashLifespawn;
    public float glowLifespawn;
    public float sparksLifespawnMin;
    public float sparksLifespawnMax;
    public float widthLightning;

    public float space;
    
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        lightningParticleSystem.Stop();
        flashParticleSystem.Stop();
        flashOnImpactParticleSystem.Stop();
        glowParticleSystem.Stop();
        sparksParticleSystem.Stop();
        if (lightningParticleSystem != null)
        {
            var rend = lightningParticleSystem.GetComponent<ParticleSystemRenderer>();
            material = rend.material;      
            material.SetColor("_Color", lightningColor);
            var main = lightningParticleSystem.main;
            main.startLifetime = lightningLifespawn;
            main.duration = space + lightningLifespawn;
        }        
        if (flashParticleSystem != null)
        {
            var main = flashParticleSystem.main;
            main.startColor = flashColor;
            main.startLifetime = flashLifespawn;
            main.duration = space + lightningLifespawn;
        }    
        if (flashOnImpactParticleSystem != null)
        {
            var main = flashOnImpactParticleSystem.main;
            main.startColor = flashColor;
            main.startLifetime = flashLifespawn;
            main.startDelay = lightningLifespawn - flashLifespawn;
            main.duration = space + lightningLifespawn;
        }        
        if (glowParticleSystem != null)
        {
            var main = glowParticleSystem.main;
            main.startColor = flashColor;
            main.startLifetime = glowLifespawn;
            main.duration = space + lightningLifespawn;
        }      
        if (sparksParticleSystem != null)
        {
            var main = sparksParticleSystem.main;
            main.startColor = flashColor;
            main.startLifetime = new ParticleSystem.MinMaxCurve(sparksLifespawnMin, sparksLifespawnMax);//Random.Range(sparksLifespawnMin, sparksLifespawnMax);
            main.startDelay = (lightningLifespawn - flashLifespawn) - 1.5f; //TODO Fix the timing
            main.duration = space + lightningLifespawn;
        }
        lightningParticleSystem.Play();
        flashParticleSystem.Play();
        flashOnImpactParticleSystem.Play();
        glowParticleSystem.Play();
        sparksParticleSystem.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (lightningParticleSystem != null)
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[this.lightningParticleSystem.particleCount]; 
            lightningParticleSystem.GetParticles(particles);
            if(particles.Length > 0) {
                float X = widthCurve.Evaluate((lightningLifespawn-particles[0].remainingLifetime)/ lightningLifespawn);
                material.SetFloat("_X", widthLightning*X);
            }
        }
    }

    private void OnValidate()
    {
        lightningParticleSystem.Stop();
        flashParticleSystem.Stop();
        flashOnImpactParticleSystem.Stop();
        glowParticleSystem.Stop();
        sparksParticleSystem.Stop();
        if (lightningParticleSystem != null)
        {
            var rend = lightningParticleSystem.GetComponent<ParticleSystemRenderer>();
            material = rend.sharedMaterial;
            material.SetColor("_Color", lightningColor);
            var main = lightningParticleSystem.main;
            main.startLifetime = lightningLifespawn;
            main.duration = space + lightningLifespawn;
        }
        if (flashParticleSystem != null)
        {
            var main = flashParticleSystem.main;
            main.startColor = flashColor;
            main.startLifetime = flashLifespawn;
            main.duration = space + lightningLifespawn;
        }
        if (flashOnImpactParticleSystem != null)
        {
            var main = flashOnImpactParticleSystem.main;
            main.startColor = flashColor;
            main.startLifetime = flashLifespawn;
            main.startDelay = lightningLifespawn - flashLifespawn;
            main.duration = space + lightningLifespawn;
        }
        if (glowParticleSystem != null)
        {
            var main = glowParticleSystem.main;
            main.startColor = flashColor;
            main.startLifetime = glowLifespawn;
            main.duration = space + lightningLifespawn;
        }
        if (sparksParticleSystem != null)
        {
            var main = sparksParticleSystem.main;
            main.startColor = flashColor;
            main.startLifetime = new ParticleSystem.MinMaxCurve(sparksLifespawnMin, sparksLifespawnMax);//Random.Range(sparksLifespawnMin, sparksLifespawnMax);
            main.startDelay = (lightningLifespawn - flashLifespawn) - 1.5f;
            main.duration = space + lightningLifespawn;
        }
        lightningParticleSystem.Play();
        flashParticleSystem.Play();
        flashOnImpactParticleSystem.Play();
        glowParticleSystem.Play();
        sparksParticleSystem.Play();
    }
}
