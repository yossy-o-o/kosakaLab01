using UnityEngine;

public class enemy : MonoBehaviour
{
    private float hp = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        hp -= 1;

    }

}
