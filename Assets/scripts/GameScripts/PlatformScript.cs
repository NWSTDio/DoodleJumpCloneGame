using System.Collections;
using UnityEngine;

public class PlatformScript : MonoBehaviour {
    [SerializeField] float destroyDelay = .1f;
    AudioSource source;
    [SerializeField] AudioClip destroy_snd;
    void Start() {
        source = GetComponent<AudioSource>();
        }
    public void Destroyer() {
        Destroy(GetComponent<BoxCollider2D>());
        StartCoroutine(_destroy());
        }
    IEnumerator _destroy() {
        if (Data.SOUND)
            source.PlayOneShot(destroy_snd);
        yield return new WaitForSeconds(destroyDelay);
        GetComponent<SpriteRenderer>().sprite = null;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        }
    }