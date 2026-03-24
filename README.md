# ✨lumos-RPG
RPG 게임에 자주 사용되는 코드 패키지 

<br>

### ℹ️의존성
* [ lumos-core ](https://github.com/lumos5934/lumos-core)


<br>
<br>


## ℹ️기능

* [Stat](#Stat)
* [Vital](#Vital)
* [UnitEffect](#UnitEffect)
* [Buff](#Buff)

<br>
<br>

---

### Stat

<table>
  <tr>
    <td><b>Stat<b></td>
      <td>개별 능력치(HP, 공격력 등)를 담당하며, 수정자들을 합산하여 최종 값을 계산</td>
  </tr>
        <tr>
    <td><b>StatModifier<b></td>
      <td>능력치를 얼마나, 어떤 방식(Flat, Percent)으로 변화시킬지 정의하는 구조체</td>
  </tr>
       <tr>
    <td><b>StatHandler<b></td>
      <td>여러 개의 Stat을 ID 기반으로 묶어 관리하며, 특정 소스(아이템 등)의 모든 수정을 일괄 제거하는 기능을 제공</td>
  </tr>
</table>

<br>
<br>

**계산**

개별 능력치(HP, 공격력 등)를 담당하며, 수정자들을 합산하여 최종 값을 계산. 수치는 성능 최적화를 위해 Dirty Flag 패턴을 사용하며,

* **Flat** (100): 기본값에 고정 수치를 더함. (예: 공격력 +10)
* **PercentAdd** (200): 고정 수치들이 합산된 후, 기본값에 곱해짐. (예: 10% + 10% = 20% 증가)
* **PercentMult** (300): 최종 결과값에 독립적으로 곱해짐. (복리 적용)

<br>
<br>

**Stat 생성 및 Modifier 적용**

```cs

// 공격력 스탯 생성 (기본값 100)
var atkStat = new Stat(100f);

// 장비 아이템 소스
object sword = new object();

// 공격력 +10 (Flat) 및 공격력 10% 증가 (PercentAdd) 추가
atkStat.Add(new StatModifier(10f, StatModType.Flat, sword));
atkStat.Add(new StatModifier(0.1f, StatModType.PercentAdd, sword));

Debug.Log($"최종 공격력: {atkStat.Value}"); // (100 + 10) * 1.1 = 121


```

<br>
<br>

**StatHandler를 통한 일괄 관리**

```cs

StatHandler handler = new StatHandler();

// 스탯 등록 (ID: 1 = Strength)
handler.Register(1, new Stat(50f));

// 아이템 장착 해제 시 해당 아이템(source)에서 온 모든 능력치 수정 제거
handler.RemoveAllFromSource(sword);

```

<br>
<br>

---

### Vital

<table>
  <tr>
    <td><b>Vital<b></td>
      <td>단일 자원(HP 등)의 현재치(Current)를 관리하며, 최대치와의 비율(Ratio)을 제공</td>
  </tr>
        <tr>
    <td><b>VitalHandler<b></td>
      <td>여러 개의 Vital을 ID 기반으로 관리하고, 특정 자원에 수치를 적용(Apply)하거나 초기화하는 기능</td>
  </tr>
</table>

<br>

Vital은 Stat 객체를 참조하며, 참조 중인 Stat의 Value가 변경되면 Vital은 이벤트를 수신하여 자신의 최대치를 갱신. 최대치가 줄어들어 현재치가 최대치를 초과하게 되면, 자동으로 현재치를 새 최대치에 맞춰 조절.

<br>

**Vital 생성**

```cs

// 1. 최대 HP가 될 Stat 생성
var hpMaxStat = new Stat(100f);

// 2. Stat을 기반으로 Vital 생성
var hpVital = new Vital(hpMaxStat);

// 3. 이벤트 구독
hpVital.OnValueChanged += (cur, max) => Debug.Log($"HP 변경: {cur} / {max}");
hpVital.OnEmpty += () => Debug.Log("캐릭터 사망!");

// 데미지 입힘 (-20)
hpVital.Apply(-20f); 

// 아이템 장착으로 최대 HP Stat이 150으로 증가하면?
hpMaxStat.SetBaseValue(150f); // Vital의 Max도 자동으로 150으로 인지함

```

<br>

**VitalHandler**

```cs

VitalHandler handler = new VitalHandler();

// ID 1번에 HP 등록
handler.Register(1, new Vital(new Stat(100f)));

// 포션 사용 (ID 1번 자원을 50 회복)
handler.Apply(1, 50f);

// 즉사 혹은 부활 처리
handler.SetEmpty(1); // 0으로
handler.SetFull(1);  // 최대치로

```

<br>
<br>

---

### UnitEffect
단위 유닛(Unit) 간의 모든 효과 적용 프로세스를 관리하며, 효과 계산부터 최종 연출까지의 파이프라인을 담당.

<table>
  <tr>
    <td><b>UnitEffectContext<b></td>
      <td>효과 적용에 필요한 모든 정보(Source, Target, HitPoint, Flags 등)를 담는 데이터 컨테이너</td>
  </tr>
        <tr>
    <td><b>UnitEffect<b></td>
      <td>적용될 실제 수치와 속성(Fire, Direct, Negative 등)을 정의하는 데이터 구조체</td>
  </tr>
      <tr>
    <td><b>IUnitEffectModifier<b></td>
      <td>Apply 직전에 수치를 변경하는 로직</td>
  </tr>
      <tr>
    <td><b>IUnitEffectFeedback<b></td>
      <td>수치가 적용된 후 실행되는 연출 로직</td>
  </tr>
      <tr>
    <td><b>UnitEffectSystem<b></td>
      <td>위 구성 요소들을 조합하여 프로세스를 실행하고 Context 객체를 풀링 관리</td>
  </tr>
</table>


<br>
<br>

**Flow**

1.`UnitEffectContext`를 생성하여 공격자, 대상, 효과 목록을 관리
2.등록된 `IUnitEffectModifier`들을 실행
3.최종 계산된 수치가 대상의 Vital 시스템에 반영
4.등록된 IUnitEffectFeedback이 실행


<br>
<br>

**Modifier와 Feedback 등록**

```cs

public class DefenseModifier : IUnitEffectModifier
{
    public int Priority => 10;
    public void Modify(UnitEffectContext ctx)
    {
        foreach (var effect in ctx.Effects)
        {
            if (effect.IsNegative) 
                effect.Value -= ctx.Target.Defense; // 단순화된 예시
        }
    }
}

UnitEffectSystem.RegisterModifier(new DefenseModifier());

```

<br>

**효과 실행**

```cs

// 1. 효과 데이터 준비
var damageEffect = new UnitEffect {
    VitalTypeID = 1, // HP
    Value = 50f,
    IsNegative = true,
    AttributeFlags = (int)Element.Fire
};

// 2. 컨텍스트 생성 및 실행
var effects = new List<UnitEffect> { damageEffect };
var ctx = UnitEffectSystem.GetContext(attacker, target, effects);

ctx.HitPoint = target.transform.position;
UnitEffectSystem.Apply(ctx); // 이 시점에 Modifier -> Vital 반영 -> Feedback 순차 실행

```

<br>
<br>

---

### Buff

유닛에게 일정 시간 동안 지속되는 효과를 부여하고 관리하는 시스템. Stack 시스템과 Interval 기반의 틱(Tick) 처리를 지원.

<table>
  <tr>
    <td><b>BaseBuff<b></td>
      <td>지속시간, 실행 간격, 중첩 수치를 관리</td>
  </tr>
        <tr>
    <td><b>BuffManager<b></td>
      <td>게임 내 모든 활성화된 버프를 모니터링하며, 매 프레임 타이머를 갱신하고, 만료된 버프를 자동으로 제거</td>
  </tr>
</table>

<br>
<br>

**Flow**

* `OnApply`: 버프가 처음 적용될 때 1회 실행 (예: 스탯 증가, 이펙트 생성)
* `OnTick`: 설정된 Interval마다 반복 실행 (예: 1초마다 독 데미지)
* `OnStack`: 동일한 버프가 다시 적용될 때 실행 (예: 중첩 수 증가, 지속시간 초기화)
* `OnRemove`: 버프가 만료되거나 제거될 때 실행 (예: 증가했던 스탯 복구)

<br>
<br>

**도트 데미지 예시**

```cs
public class PoisonBuff : BaseBuff
{
    protected override void OnApply() => Debug.Log("독에 걸렸습니다!");
    
    protected override void OnTick()
    {
        // 1초마다 타겟에게 데미지 적용 (UnitEffectSystem 연동 가능)
        var context = UnitEffectSystem.GetContext(null, Target, Effects);
        UnitEffectSystem.Apply(context);
    }

    protected override void OnStack() => Timer = Duration; // 중첩 시 시간 초기화
    protected override void OnRemove() => Debug.Log("독이 해제되었습니다.");
}
```

```cs
public class CombatTest : MonoBehaviour
{
    [SerializeField] private BaseBuff _poisonPrefab;

    public void Hit(IUnit target)
    {
        var buffManager = Services.Get<BuffManager>();
        
        // 새로운 버프 인스턴스 생성 후 타겟에게 부여
        // (기존에 같은 ID의 버프가 있다면 자동으로 AddStack 실행)
        buffManager.Add(target, Instantiate(_poisonPrefab));
    }
}
```

<br>
<br>
