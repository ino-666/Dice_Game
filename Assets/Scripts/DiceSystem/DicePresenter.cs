using System.Collections;
using UnityEngine;

public class DicePresenter : MonoBehaviour
{
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float fixDelay = 3f;

    public void SpawnAndRoll(int result)
    {
        GameObject dice = Instantiate(
            dicePrefab,
            spawnPoint.position,
            Random.rotation
        );

        Rigidbody rb = dice.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(Random.insideUnitSphere * 2f, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);

        StartCoroutine(FixDice(dice, result));
    }

    private IEnumerator FixDice(GameObject dice, int result)
    {
        yield return new WaitForSeconds(fixDelay);

        Rigidbody rb = dice.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        dice.transform.rotation = DiceRotationTable.Get(result);
    }
}
