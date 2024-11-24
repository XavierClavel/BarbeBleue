
public class BonusValue
{
    private float multiplicativeBonus = 1f;
    private float additiveBonus = 0f;

    private float multiplicativeBoost = 1f;
    private float additiveBoost = 0f;

    public void addAdditiveBonus(float value)
    {
        additiveBonus += value;
    }

    public void addMultiplicativeBonus(float value)
    {
        multiplicativeBonus *= value;
    }

    public float getMultiplier() => multiplicativeBonus * multiplicativeBoost;
    public float getAdditioner() => additiveBonus + additiveBoost;
}
