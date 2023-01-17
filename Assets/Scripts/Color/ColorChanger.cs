using UnityEngine;
using System.Collections;

public class ColorChanger : MonoBehaviour
{
    public float destroySplashDelay = 0.1f;
    public float colorTargetObjectDelay = 0.1f;

    void HeartsFun(GameObject g)
    {   
        FindObjectOfType<BallHandler>().HeartsLow();
        int heartsNumber = PlayerPrefs.GetInt("hearts");

        if(heartsNumber == 0)
        {
            FindObjectOfType<BallHandler>().FailGame(); 
        }

    }

    IEnumerator ChangeColor(GameObject g)
    {
            yield return new WaitForSeconds(colorTargetObjectDelay);
            g.gameObject.GetComponent<MeshRenderer>().enabled = true;
            g.gameObject.GetComponent<MeshRenderer>().material.color = BallHandler.oneColor;
            Destroy(base.gameObject);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "red")
        {
            base.gameObject.GetComponent<Collider>().enabled = false;
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
            other.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;

            base.GetComponent<Rigidbody>().AddForce(Vector3.down * 50, ForceMode.Impulse);
            HeartsFun(other.gameObject);
            Destroy(base.gameObject);
        }
        else
        {
            base.gameObject.GetComponent<Collider>().enabled = false;
            base.gameObject.GetComponent<MeshRenderer>().enabled = false;

            GameObject instantiatedSplash = Instantiate(Resources.Load("splat-white") as GameObject);
            instantiatedSplash.GetComponent<SpriteRenderer>().material.color = BallHandler.oneColor;
            instantiatedSplash.transform.parent = other.gameObject.transform;
            Destroy(instantiatedSplash, destroySplashDelay);

            other.gameObject.name = "color";
            other.gameObject.tag = "red";

            StartCoroutine(ChangeColor(other.gameObject));
        }   
    }
}
