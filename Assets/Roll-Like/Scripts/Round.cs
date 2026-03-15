using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Round : MonoBehaviour
{
    [Header("Round Stats")]
    public int StartRollCount = 3;
    public int BaseScore = 5;
    public int DifficultyDelta = 2;
    public int Difficulty = 0;
    [SerializeField]
    [Tooltip("Time in seconds after die comes to rest for the roll to count.")]
    private float postRollDelay;
    [Space]
    [SerializeField]
    [TextArea]
    private string winMessage;
    [SerializeField]
    [TextArea]
    private string loseMessage;
    [Space]

    [Space]
    [SerializeField]
    private UnityEvent startRound;
    [SerializeField]
    private UnityEvent resetRoll;
    [SerializeField]
    private UnityEvent endRound;

    [Header("Unity Set Up")]
    [SerializeField]
    [Tooltip("The Die script on the Die prefab. Should only be on in scene EVER")]
    private Die theDie;
    [SerializeField]
    [Tooltip("TMProTextMesh to display current Round Score")]
    private TextMeshProUGUI roundScoreDisplay;
    [SerializeField]
    [Tooltip("TMProTextMesh to display current Round Goal")]
    private TextMeshProUGUI goalDisplay;
    [SerializeField]
    [Tooltip("TMProTextMesh that appears when winning Round")]
    private TextMeshProUGUI resultsDisplay;

    private Rigidbody _dieRB;
    private int _rollCount = 0;
    private float[] _baseValuesRolled;
    private float[] _rollScores;

    private float _roundScore = 0;
    private float _goal;

    private void Awake()
    {
        // Grab Rigidbody component
        _dieRB = theDie.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        startRound.Invoke();
    }

    /**
     * Function to begin a new round
     */
    public void StartRound()
    {
        // Turn off result display
        resultsDisplay.gameObject.SetActive(false);

        // Initialize new round trackers
        _baseValuesRolled = new float[StartRollCount];
        _rollScores = new float[StartRollCount];

        // Set initial values
        _rollCount = 0;
        _roundScore = 0;
        _goal = BaseScore + DifficultyDelta * Difficulty;

        // Display initial score and goal
        DisplayScoreAndGoal();

        // Make sure die is snapped
        resetRoll.Invoke();
    }

    /**
     * Publically accessible way to try and evaluate a roll
     */
    public void TryRoll()
    {
        StartCoroutine(CheckForRoll());
    }

    /**
     * Main logic for each individual roll
     */
    private IEnumerator CheckForRoll()
    {
        // Wait for die to come to rest.
        while (!_dieRB.IsSleeping())
        {
            yield return 0;
        }

        // The die has now come to rest. Parse it
        if (!theDie.ParseRoll(out Side sideRolled))
        {
            // Inconclusive parse reset roll to try again.
            resetRoll.Invoke();
        }
        else
        {
            yield return new WaitForSeconds(postRollDelay);
            SuccessfulRoll(sideRolled);
        }
    }

    /**
     * Logic to close out an individual roll
     * 
     * @param sideRolled: the Side that was successfully rolled
     */
    private void SuccessfulRoll(Side sideRolled)
    {
        // Access BaseValue
        float baseValue = sideRolled.BaseValue;

        Debug.Log(baseValue);

        // Make the die display the side
        theDie.DisplaySide((int)baseValue - 1);

        // Store base value rolled
        _baseValuesRolled[_rollCount] = baseValue;

        // Store roll score
        _rollScores[_rollCount] = sideRolled.EvaluateSideScore();

        // Update score for this round and display
        _roundScore = SumRolls(_rollScores);
        DisplayRoundScore();

        // Increment roll count
        _rollCount++;

        // Check for end of Round
        if(_rollCount == StartRollCount)
        {
            endRound.Invoke();
            return;
        }

        // Get ready for another roll
        resetRoll.Invoke();
    }

    /**
     * Function that gets called at the end of the round to determine win or lose
     */
    public void EndRound()
    {
        int moneyDiff = (int)_roundScore - (int)_goal;

        // Win Condition
        if (moneyDiff >= 0)
        {
            resultsDisplay.text = winMessage + moneyDiff.ToString();
        }
        else
        {
            resultsDisplay.text = loseMessage + moneyDiff.ToString();
        }

        resultsDisplay.gameObject.SetActive(true);
        Money.Instance.Delta(moneyDiff);
    }

    /**
     * Sums the numbers in an array
     * 
     * @param array: the float array to sum
     */
    private float SumRolls(float[] array)
    {
        float sum = 0;

        foreach (float num in array)
        {
            sum += num;
        }

        return sum;
    }

    /**
     * Displays Round Score in TextMeshProUGUI
     */
    private void DisplayRoundScore()
    {
        roundScoreDisplay.text = _roundScore.ToString();
    }

    /**
     * Displays Round Goal in TextMeshProUGUI
     */
    private void DisplayGoal()
    {
        goalDisplay.text = _goal.ToString();
    }

    /**
     * Handles both above functions at once
     */
    private void DisplayScoreAndGoal()
    {
        DisplayRoundScore();
        DisplayGoal();
    }

    public void RaiseDifficultyAndStart(int difficultyDelta)
    {
        Difficulty += difficultyDelta;

        StartRound();
    }
}
