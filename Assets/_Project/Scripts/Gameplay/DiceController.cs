using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceController : MonoBehaviour
{
    [SerializeField] private GameObject diceGameObject;
    [SerializeField] private float rollDuration = 1f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeIntensity = 0.1f;

    private Transform diceParentTransform;
    private Vector3 originalPosition;
    private bool isRolling = false;
    private Coroutine rollDiceCoroutine = null;
    private int diceResult = -1;

    public event Action<int> OnRollFinishedOrCanceled;

    private void Start()
    {
        diceParentTransform = diceGameObject.transform.parent;
        originalPosition = diceParentTransform.position;
    }
    public void RollDice()
    {
        // Allow the player to "spam button" to be faster, and still roll dice and stack results
        if (isRolling)
        {
            isRolling = false;
            StopCoroutine(rollDiceCoroutine);
            SetFinalFace(diceResult);
            OnRollFinishedOrCanceled?.Invoke(diceResult);
        }

        // Can be replaced with API endpoint call / server call, to securise gambling games
        diceResult = Random.Range(1, 7);

        rollDiceCoroutine = StartCoroutine(RollDiceProcess());
    }

    public void DisplayDice(bool value)
    {
        diceGameObject.SetActive(value);
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator RollDiceProcess()
    {
        isRolling = true;

        float elapsedTime = 0f;
        while (elapsedTime < rollDuration)
        {
            RotateRandomly();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetFinalFace(diceResult);
        isRolling = false;

        OnRollFinishedOrCanceled?.Invoke(diceResult);
    }

    private void RotateRandomly()
    {
        diceParentTransform.Rotate(Vector3.up, Random.Range(100f, 500f) * Time.deltaTime);

        diceGameObject.transform.Rotate(
            Random.Range(400f, 600f) * Time.deltaTime,
            Random.Range(400f, 600f) * Time.deltaTime,
            Random.Range(400f, 600f) * Time.deltaTime
        );
    }

    private void SetFinalFace(int face)
    {
        diceParentTransform.rotation = Quaternion.identity;

        switch (face)
        {
            case 1:
                diceGameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
                break;
            case 2:
                diceGameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case 3:
                diceGameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case 4:
                diceGameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 5:
                diceGameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case 6:
                diceGameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
                break;
            default:
                break;
        }
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            diceParentTransform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeIntensity;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        diceParentTransform.position = originalPosition;
    }
}
