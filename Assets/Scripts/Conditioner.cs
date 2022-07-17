using UnityEngine;
using TMPro;

public class Conditioner : MonoBehaviour
{
    public Condition condition;
    public int value;
    public TMP_Text expressionText;
    private string _expression;

    #region Unity Event

    private void Start()
    {
        _expression = $"{ConditionToExpression(condition)} {value.ToString()}";
        if (expressionText) expressionText.text = _expression;
    }

    #endregion

    public bool Evaluate(int otherValue)
    {
        return Evaluate(condition, otherValue, value);
    }

    public bool Evaluate(Condition condition, int value1, int value2)
    {
        switch (condition)
        {
            case Condition.Greater:
            return value1 > value2;

            case Condition.GreaterEqual:
            return value1 >= value2;

            case Condition.Less:
            return value1 < value2;

            case Condition.LessEqual:
            return value1 <= value2;

            case Condition.Equal:
            return value1 == value2;

            default:
            return false;
        }
    }

    public string ConditionToExpression(Condition condition)
    {
        switch (condition)
        {
            case Condition.Greater:
            return ">";

            case Condition.GreaterEqual:
            return ">=";

            case Condition.Less:
            return "<";

            case Condition.LessEqual:
            return "<=";

            case Condition.Equal:
            return "==";

            default:
            return "";
        }
    }
}
