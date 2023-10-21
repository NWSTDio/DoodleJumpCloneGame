using UnityEngine;

public class DestroyerObjectsScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.tag.Equals("Player"))
            Destroy(collision.gameObject);
        }

    }