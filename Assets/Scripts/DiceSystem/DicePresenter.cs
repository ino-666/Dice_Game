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

    public void SpawnAndRoll(int result)
    {
        // 前のサイコロ削除
        if (currentDice != null)
        {
            Destroy(currentDice);
        }

        currentDice = Instantiate(dicePrefab, spawnPoint.position, Quaternion.identity);

        // ★ 効果音（投げた瞬間）
        if (rollSE != null)
        {
            audioSource.PlayOneShot(rollSE);
        }

        var rb = currentDice.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        rb.AddForce(Random.insideUnitSphere * 2f, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);

        StartCoroutine(FixDice(currentDice, result));
    }

    private IEnumerator FixDice(GameObject dice, int result)
    {
        yield return new WaitForSeconds(fixDelay);

        Rigidbody rb = dice.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        dice.transform.rotation = DiceRotationTable.Get(result);
    }
}
