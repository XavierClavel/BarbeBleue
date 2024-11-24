
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public interface IGameValue
{
    public void applyOperation(string change);
}

public abstract class GameValue<T> : IGameValue
{
    protected T value;
    public virtual T getValue() => value;

    public void applyOperation(string change)
    {
        operationType operation;
        if (change.Last() == '%')
        {
            if (change.First() == '+') operation = operationType.multiply;
            else if (change.First() == '-') operation = operationType.divide;
            else throw new System.ArgumentException($"{change} operation failed to parse");

            change = change.RemoveFirst();
            change = change.RemoveLast();
            float percentage = Helpers.parseString<float>(change);
            if (operation == operationType.multiply) percentage = 1 + percentage * 0.01f;
            else percentage = 1 - percentage * 0.01f;
            change = percentage.ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            if (change.First() == '+')
            {
                operation = operationType.add;
                change = change.RemoveFirst();
            }
            else if (change.First() == '-')
            {
                operation = operationType.substract;
                change = change.RemoveFirst();
            }
            else operation = operationType.assignation;
        }
        applyOperation(operation, Helpers.parseString<T>(change));
    }

    protected abstract void applyOperation(operationType operation, T value);
}

public abstract class BonusGameValue<T> : GameValue<T>
{
    protected BonusValue bonusValue;
    protected abstract T applyBonus(BonusValue bonusValue);
    protected abstract T applyBonus(float multiplier, float additioner);

    public T applyBonus(List<BonusValue> bonuses)
    {
        
    }
}

public class GameValueInt : BonusGameValue<int>
{
    protected override void applyOperation(operationType operation, int change)
    {
        switch (operation)
        {
            case operationType.add:
                value += change;
                break;

            case operationType.substract:
                value -= change;
                break;

            case operationType.multiply:
                value *= change;
                break;

            case operationType.divide:
                value /= change;
                break;

            case operationType.assignation:
                value = change;
                break;

            default:
                throw new System.InvalidOperationException($"failed to execute operation {operation} with value {change}");
        }
    }
    
    

    public override int getValue()
    {
        return applyBonus(bonusValue);
    }

    public GameValueInt()
    {
        this.value = 0;
    }

    protected override int applyBonus(BonusValue bonusValue)
    {
        return (int)bonusValue.getMultiplier() * value + (int)bonusValue.getAdditioner();
    }
}

public class GameValueFloat : BonusGameValue<float>
{

    protected override void applyOperation(operationType operation, float change)
    {
        switch (operation)
        {
            case operationType.add:
                value += change;
                break;

            case operationType.multiply:
                value *= change;
                break;

            case operationType.assignation:
                value = change;
                break;

            default:
                throw new System.InvalidOperationException($"failed to execute operation {operation} with value {change}");
        }
    }

    public override float getValue()
    {
        return applyBonus(bonusValue);
    }
    
    public GameValueFloat()
    {
        this.value = 0f;
    }

    protected override float applyBonus(BonusValue bonusValue)
    {
        return bonusValue.getMultiplier() * value + bonusValue.getAdditioner();
    }
}

public class GameValueBool : GameValue<bool>
{
    protected override void applyOperation(operationType operation, bool change)
    {
        switch (operation)
        {
            case operationType.assignation:
                value = change;
                break;

            default:
                throw new System.InvalidOperationException($"failed to execute operation {operation} with value {change}");
        }
    }
    
    public GameValueBool()
    {
        this.value = false;
    }
}

public class GameValueVector2Int : GameValue<Vector2Int>
{
    protected override void applyOperation(operationType operation, Vector2Int change)
    {
        switch (operation)
        {
            case operationType.add:
                value += change;
                break;

            case operationType.substract:
                value -= change;
                break;

            case operationType.multiply:
                value *= change;
                break;

            case operationType.divide:
                value = new Vector2Int(value.x / change.x, value.y / change.y);
                break;

            case operationType.assignation:
                value = change;
                break;

            default:
                throw new System.InvalidOperationException($"failed to execute operation {operation} with value {change}");
        }
    }
    
    public GameValueVector2Int()
    {
        this.value = Vector2Int.zero;
    }
}

public class GameValueVector2 : GameValue<Vector2>
{
    protected override void applyOperation(operationType operation, Vector2 change)
    {
        switch (operation)
        {
            case operationType.add:
                value += change;
                break;

            case operationType.substract:
                value -= change;
                break;

            case operationType.multiply:
                value *= change;
                break;

            case operationType.divide:
                value = new Vector2(value.x / change.x, value.y / change.y);
                break;

            case operationType.assignation:
                value = change;
                break;

            default:
                throw new System.InvalidOperationException($"failed to execute operation {operation} with value {change}");
        }
    }
    
    public GameValueVector2()
    {
        this.value = Vector2.zero;
    }
}

public class GameValueString : GameValue<string>
{
    protected override void applyOperation(operationType operation, string change)
    {
        switch (operation)
        {
            case operationType.assignation:
                value = change;
                break;

            default:
                throw new System.InvalidOperationException($"failed to execute operation {operation} with value {change}");
        }
    }
    
    public GameValueString()
    {
        this.value = String.Empty;
    }
}

public class GameValueFlags : GameValue<List<string>>
{
    protected override void applyOperation(operationType operation, List<string> change)
    {
        switch (operation)
        {
            case operationType.add:
                value.AddList(change);
                break;

            case operationType.substract:
                value.RemoveList(change);
                break;

            case operationType.assignation:
                value = change.Copy();
                break;

            default:
                throw new System.InvalidOperationException($"failed to execute operation {operation} with value {change}");
        }
    }
    
    public GameValueFlags()
    {
        this.value = new List<string>();
    }
}