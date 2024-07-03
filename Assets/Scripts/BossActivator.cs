using UnityEngine;

public class BossActivator : MonoBehaviour
{

    public GameObject boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("BOSS ACTIVATED");
            boss.SetActive(true);
            boss.GetComponent<BossBehaviour>().player = other.GetComponent<PlayerController>();
            boss.GetComponent<BossBehaviour>().enabled = true;
            //GameEvents.ShowPlayerHealth.Invoke();
            //GameEvents.ShowBossHealth.Invoke();
        }
    }

}
