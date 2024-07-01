using System.Collections;
using UnityEngine;

public class EnableIfMistake : MonoBehaviour
{

    public string mistakeName;
    public SpriteRenderer spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.GetComponent<SpriteRenderer>();
        
        StartCoroutine(Verify());
    }

    private IEnumerator Verify()
    {

        while (true)
        {
            if (GameManager.IsMistakeDone(mistakeName))
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
            
            yield return new WaitForSeconds(5);
        }
        
    }
}
