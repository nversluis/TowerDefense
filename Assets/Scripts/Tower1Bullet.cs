using UnityEngine;
using System.Collections;

public class Tower1Bullet : MonoBehaviour
{

    private GameObject target; //the gameobject the bullet is going towards
    private Vector2 position; // the startposition of the object
    private GameObject bullet;  // the prefab of the bullet
    public float speed;
    void Awake()
    {
        target = Tower1.targetObject;
    }

    //todo, damage the target
    //only triggers somehow when target is moving.
    void OnTriggerEnter(Collider col)
    {
        Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

        if (Vector3.Distance(gameObject.transform.position, target.transform.position) <= 1f)
        {
            Destroy(gameObject);
        }

    }
}
