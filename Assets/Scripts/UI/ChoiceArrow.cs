using UnityEngine;
using UnityEngine.UI;

public class ChoiceArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] choices;
    [SerializeField] private AudioClip moveArrow;
    [SerializeField] private AudioClip choiceSelected;
    private int currentPos;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        currentPos = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            Select();
    }

    private void ChangePosition(int _change)
    {
        currentPos += _change;

        if (_change != 0)
            SoundManager.instance.PlaySoundEffect(moveArrow);

        if (currentPos < 0)
            currentPos = choices.Length - 1;
        else if (currentPos > choices.Length - 1)
            currentPos = 0;

        rectTransform.position = new Vector3(rectTransform.position.x, choices[currentPos].position.y, 0);
    }

    private void Select()
    {
        SoundManager.instance.PlaySoundEffect(choiceSelected);
        choices[currentPos].GetComponent<Button>().onClick.Invoke();
    }
}
