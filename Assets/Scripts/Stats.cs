/// <summary>
/// 캐릭터의 모든 스탯을 관리하는 struct.
/// </summary>
[System.Serializable]
public struct Stats
{
    /*
     [스탯 관련 변수]
     -Base: 장비 등에 의해 영향받지 않은 스탯의 기본값.
     
     -FixedModifier: 수치만큼 스탯을 추가.
      (예: fixedModifier = 30f => 30만큼 스탯 증가)

     -RateModifier: 수치에 해당하는 비율만큼 스탯을 추가. (합연산으로 적용)
      (예: rateModifier = 0.3f => Base의 30%만큼 스탯 증가)

     [스탯 계산 공식]
      최종값 = base * (1 + rateModifier) + fixedModifier
     */
    public float maxHpBase;
    public float maxHpFixedModifier;
    public float maxHpRateModifier;

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
        maxHpBase += other.maxHpBase;
        maxHpFixedModifier += other.maxHpFixedModifier;
        maxHpRateModifier += other.maxHpRateModifier;

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
        maxHpBase -= other.maxHpBase;
        maxHpFixedModifier -= other.maxHpFixedModifier;
        maxHpRateModifier -= other.maxHpRateModifier;

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

    public static Stats operator +(Stats left, Stats right)
    {
        left.Add(right);
        return left;
    }

    public static Stats operator -(Stats left, Stats right)
    {
        left.Sub(right);
        return left;
    }
}