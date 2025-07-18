using UnityEngine;

public class ManualCollisionSafeSpawner2D : ManualBasicSpawner2D
{



    protected override void PlaceSpawnable(BasicSpawnable2D spawnable2D)
    {

        var collisionSafeSpawnable = spawnable2D as CollisionSafeSpawnable2D;
        
        do
        {
            base.PlaceSpawnable(collisionSafeSpawnable);
        } while (collisionSafeSpawnable.IsCollided);
    }
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
