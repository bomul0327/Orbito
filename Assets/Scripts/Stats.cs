/// <summary>
/// 캐릭터의 모든 스탯을 관리하는 struct.
/// </summary>
[System.Serializable]
public struct Stats
{
    public float maxHPBase;
    public float maxHPFixedModifier;
    public float maxHPRateModifier;

    public float maxSpeedBase;
    public float maxSpeedFixedModifier;
    public float maxSpeedRateModifier;

    public float maxFuelBase;
    public float maxFuelFixedModifier;
    public float maxFuelRateModifier;

    public float fuelReductionRatioBase;
    public float fuelReductionRatioFixedModifier;
    public float fuelReductionRatioRateModifier;

    public float defenseBase;
    public float defenseFixedModifier;
    public float defenseRateModifier;


    public void Add(Stats other)
    {
        maxHPBase += other.maxHPBase;
        maxHPFixedModifier += other.maxHPFixedModifier;
        maxHPRateModifier += other.maxHPRateModifier;

        maxSpeedBase += other.maxSpeedBase;
        maxSpeedFixedModifier += other.maxSpeedFixedModifier;
        maxSpeedRateModifier += other.maxSpeedRateModifier;

        maxFuelBase += other.maxFuelBase;
        maxFuelFixedModifier += other.maxFuelFixedModifier;
        maxFuelRateModifier += other.maxFuelRateModifier;

        fuelReductionRatioBase += other.fuelReductionRatioBase;
        fuelReductionRatioFixedModifier += other.fuelReductionRatioFixedModifier;
        fuelReductionRatioRateModifier += other.fuelReductionRatioRateModifier;

        defenseBase += other.defenseBase;
        defenseFixedModifier += other.defenseFixedModifier;
        defenseRateModifier += other.defenseRateModifier;

    }

    public void Sub(Stats other)
    {
        maxHPBase -= other.maxHPBase;
        maxHPFixedModifier -= other.maxHPFixedModifier;
        maxHPRateModifier -= other.maxHPRateModifier;

        maxSpeedBase -= other.maxSpeedBase;
        maxSpeedFixedModifier -= other.maxSpeedFixedModifier;
        maxSpeedRateModifier -= other.maxSpeedRateModifier;

        maxFuelBase -= other.maxFuelBase;
        maxFuelFixedModifier -= other.maxFuelFixedModifier;
        maxFuelRateModifier -= other.maxFuelRateModifier;

        fuelReductionRatioBase -= other.fuelReductionRatioBase;
        fuelReductionRatioFixedModifier -= other.fuelReductionRatioFixedModifier;
        fuelReductionRatioRateModifier -= other.fuelReductionRatioRateModifier;

        defenseBase -= other.defenseBase;
        defenseFixedModifier -= other.defenseFixedModifier;
        defenseRateModifier -= other.defenseRateModifier;
    }

    public static Stats operator +(Stats a, Stats b)
    {
        a.Add(b);
        return a;
    }

    public static Stats operator -(Stats a, Stats b)
    {
        a.Sub(b);
        return a;
    }
}