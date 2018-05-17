using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject sender = null;
    private float speed = 0.0f;
    Vector3 direction = Vector3.zero;
    Quaternion orientation = Quaternion.identity;
    private int damage = 0;

    public void SetInfo(float _speed, int _damage, Vector3 _direction, GameObject _sender, Quaternion _orientation)
    {
        speed = _speed;
        damage = _damage;
        direction = _direction.normalized;
        sender = _sender;
        orientation = _orientation;
    }
	
	// Update is called once per frame
	void Update ()
    {
        MoveForward();

    }

    private void MoveForward()
    {
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = orientation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(sender != null)
        {
            Stats stats = other.gameObject.GetComponent<Stats>();

            if(stats != null)
            {
                stats.OnHit(sender, damage);

                Destroy(gameObject);
            }
        }
    }
}
