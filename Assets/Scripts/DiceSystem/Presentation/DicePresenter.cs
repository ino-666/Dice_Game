using System.Collections;
using UnityEngine;

public class DicePresenter : MonoBehaviour
{
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float fixDelay = 3f;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rollSE;

    private GameObject currentDice;  
    private Coroutine fixCoroutine;


    public void SpawnAndRoll(int result)
    {
        // 前のサイコロ削除
        if (currentDice != null)
        {
            if (fixCoroutine != null)
            {
                StopCoroutine(fixCoroutine);
                fixCoroutine = null;
            }
    
            Destroy(currentDice);
        }
    
        currentDice = Instantiate(dicePrefab, spawnPoint.position, Quaternion.identity);
    
        if (rollSE != null)
        {
            audioSource.PlayOneShot(rollSE);
        }
    
        var rb = currentDice.GetComponent<Rigidbody>();
        rb.isKinematic = false;
    
        rb.AddForce(new Vector3(0.5f, 1.0f, 0.3f) * 2f, ForceMode.Impulse);
        rb.AddTorque(new Vector3(5f, 8f, 3f), ForceMode.Impulse);
    
        fixCoroutine = StartCoroutine(FixDice(currentDice, result));
    }

    private IEnumerator FixDice(GameObject dice, int result)
    {
        yield return new WaitForSeconds(fixDelay);

        Rigidbody rb = dice.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        dice.transform.rotation = DiceRotationTable.Get(result);
    }
}
