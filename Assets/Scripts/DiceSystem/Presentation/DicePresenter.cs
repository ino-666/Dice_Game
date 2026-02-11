using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicePresenter : MonoBehaviour
{
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float fixDelay = 3f;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rollSE;

    // 複数のサイコロを管理するリストに変更
    private List<GameObject> activeDiceList = new List<GameObject>(); 

    // 新しいロールの前に古いサイコロをすべて削除するメソッド
    public void ClearOldDice()
    {
        foreach (var dice in activeDiceList)
        {
            if (dice != null) Destroy(dice);
        }
        activeDiceList.Clear();
    }

    // 引数にインデックスを追加して、出現位置を少しずつずらす
    public void SpawnAndRoll(int result, int index)
    {
        // 少し横にずらして生成（重なり防止）
        Vector3 offset = new Vector3(index * 0.8f, 0, 0); 
        GameObject dice = Instantiate(dicePrefab, spawnPoint.position + offset, Quaternion.identity);
        activeDiceList.Add(dice);

        if (rollSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(rollSE);
        }

        var rb = dice.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        // 勢いも少しランダムに散らす
        rb.AddForce(new Vector3(Random.Range(-0.5f, 0.5f), 1.0f, 0.3f) * 2f, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.Range(-5f, 5f), 8f, Random.Range(-5f, 5f)), ForceMode.Impulse);

        StartCoroutine(FixDice(dice, result));
    }

    private IEnumerator FixDice(GameObject dice, int result)
    {
        yield return new WaitForSeconds(fixDelay);
        if (dice == null) yield break;

        Rigidbody rb = dice.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        dice.transform.rotation = DiceRotationTable.Get(result);
    }
}