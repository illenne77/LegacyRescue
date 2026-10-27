using System.Globalization;
using System.Text;

namespace LegacyRescue.DbTool;

/// <summary>
/// Generates data/seed.sql deterministically: same seed, same file, byte for byte.
/// Random volume data is mixed with hand-placed edge cases that the reports must handle.
/// </summary>
internal sealed class SeedGenerator
{
    private const int RandomSeed = 20260901;
    private static readonly DateOnly FirstDay = new(2026, 9, 1);
    private static readonly DateOnly LastDay = new(2026, 9, 30);
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    private const string DayShift = "주간";
    private const string NightShift = "야간";

    private sealed record ItemDef(string Code, string Name, string Unit, string Line);

    private sealed record LineDef(string Code, int MinPlan, int MaxPlan, int Step);

    private static readonly ItemDef[] Items =
    [
        new("BRK-A100", "브래킷 A형", "EA", "L1"),
        new("BRK-B200", "브래킷 B형", "EA", "L1"),
        new("PNL-S10", "측면 패널", "EA", "L1"),
        new("FRM-W30", "용접 프레임", "EA", "L2"),
        new("FRM-W40", "보강 프레임", "EA", "L2"),
        new("HSG-C50", "하우징 커버", "EA", "L3"),
        new("ASM-M70", "모터 마운트 조립품", "SET", "L3"),
        new("ASM-H80", "핸들 조립품", "SET", "L3"),
    ];

    private static readonly (string Code, string Name)[] DefectCodes =
    [
        ("D01", "치수불량"),
        ("D02", "외관불량"),
        ("D03", "용접불량"),
        ("D04", "조립불량"),
        ("D99", "기타"),
    ];

    private static readonly LineDef[] Lines =
    [
        new("L1", 800, 1200, 50),
        new("L2", 200, 400, 10),
        new("L3", 300, 600, 10),
    ];

    private readonly Random _rng = new(RandomSeed);
    private readonly List<WorkOrderRow> _orders = [];
    private readonly List<ResultRow> _results = [];

    private sealed record WorkOrderRow(
        int Id, string OrderNo, string ItemCode, string LineCode, int PlanQty,
        DateOnly WorkDate, string Shift, string? Remark, string CreatedAt);

    private sealed record ResultRow(
        int WorkOrderId, int GoodQty, int DefectQty, string? DefectCode,
        string StartTime, string EndTime, string EnteredAt);

    public string Generate()
    {
        // Night shift of the day before the period: its output is produced on 09-01.
        AddEdgeCase(new DateOnly(2026, 8, 31), "L2", NightShift, "FRM-W30", 300,
            [(290, 6, "D03", "20:10", "07:40")], remark: null);

        for (var day = FirstDay; day <= LastDay; day = day.AddDays(1))
        {
            if (day.DayOfWeek == DayOfWeek.Sunday)
            {
                continue;
            }

            foreach (var line in Lines)
            {
                foreach (var shift in new[] { DayShift, NightShift })
                {
                    if (TryAddEdgeCaseSlot(day, line.Code, shift))
                    {
                        continue;
                    }

                    AddRandomSlot(day, line, shift);
                }
            }
        }

        return Render();
    }

    /// <summary>
    /// Hand-placed slots. Each one replaces the random orders of its line/shift/day so that
    /// the daily aggregate equals the values below exactly. Ratios use dyadic fractions
    /// (e.g. 62.5 %, 95.125 %) so they are exact in binary floating point and land on a rounding midpoint.
    /// </summary>
    private bool TryAddEdgeCaseSlot(DateOnly day, string line, string shift)
    {
        switch (day.Day, line, shift)
        {
            // Achievement exactly on a midpoint at 0 / 1 / 2 decimals.
            case (2, "L3", DayShift):
                AddEdgeCase(day, line, shift, "HSG-C50", 400, [(250, 0, null, "08:00", "17:00")]);   // 62.5 %
                return true;
            case (3, "L2", DayShift):
                AddEdgeCase(day, line, shift, "FRM-W40", 400, [(381, 3, "D03", "08:05", "18:30")]);  // 95.25 %
                return true;
            case (4, "L1", NightShift):
                AddEdgeCase(day, line, shift, "PNL-S10", 800, [(761, 9, "D01", "20:00", "06:40")]);  // 95.125 %
                return true;
            case (5, "L1", DayShift):
                AddEdgeCase(day, line, shift, "BRK-A100", 1000, [(1025, 0, null, "08:00", "19:30")]); // 102.5 %
                return true;

            // Defect rate on a midpoint: 1 of 80 = 1.25 %, 3 of 400 = 0.75 %.
            case (7, "L2", NightShift):
                AddEdgeCase(day, line, shift, "FRM-W30", 100, [(79, 1, "D03", "20:00", "04:00")]);
                return true;
            case (8, "L3", NightShift):
                AddEdgeCase(day, line, shift, "ASM-M70", 400, [(397, 3, "D04", "20:00", "07:00")]);
                return true;

            // Planned quantity 0 (unplanned sample production).
            case (9, "L1", DayShift):
                AddEdgeCase(day, line, shift, "BRK-B200", 0, [(40, 2, "D02", "13:00", "15:00")], remark: "샘플 생산");
                return true;

            // Line stopped: order issued, a result row with zero quantities.
            case (10, "L2", DayShift):
                AddEdgeCase(day, line, shift, "FRM-W40", 300, [(0, 0, null, "08:00", "08:40")], remark: "설비 고장");
                return true;

            // Order issued but no result entered at all.
            case (11, "L3", DayShift):
                AddEdgeCase(day, line, shift, "ASM-H80", 350, [], remark: "자재 미입고");
                return true;

            // Night shift split around midnight; the second entry is entirely on the next calendar day.
            case (14, "L1", NightShift):
                AddEdgeCase(day, line, shift, "BRK-A100", 900,
                    [(430, 5, "D01", "20:00", "23:50"), (455, 4, "D01", "00:30", "07:50")]);
                return true;

            // Saturday night shift running into Sunday (a non-working day).
            case (26, "L2", NightShift):
                AddEdgeCase(day, line, shift, "FRM-W30", 280, [(270, 4, "D03", "20:00", "06:00")]);
                return true;

            // Last night of the month running into October.
            case (30, "L3", NightShift):
                AddEdgeCase(day, line, shift, "HSG-C50", 500,
                    [(240, 3, "D02", "20:00", "23:59"), (262, 2, "D02", "00:00", "07:30")]);
                return true;

            default:
                return false;
        }
    }

    private void AddRandomSlot(DateOnly day, LineDef line, string shift)
    {
        var lineItems = Items.Where(i => i.Line == line.Code).ToArray();
        var orderCount = _rng.NextDouble() < 0.25 ? 2 : 1;

        for (var n = 0; n < orderCount; n++)
        {
            var item = lineItems[_rng.Next(lineItems.Length)];
            var plan = RandomPlan(line, shift, orderCount);
            var id = AddOrder(day, line.Code, shift, item.Code, plan, remark: null);

            // ~3 % of orders never get a result entered.
            if (_rng.NextDouble() < 0.03)
            {
                continue;
            }

            var slices = orderCount == 1 ? FullShiftSlices(shift) : HalfShiftSlices(shift, n);
            var produced = (int)Math.Round(plan * (0.85 + (_rng.NextDouble() * 0.23)));
            var defectRate = _rng.NextDouble() < 0.2 ? 0.0 : _rng.NextDouble() * 0.04;
            var defects = (int)Math.Round(produced * defectRate);
            var good = produced - defects;

            var goodLeft = good;
            var defectLeft = defects;
            for (var s = 0; s < slices.Count; s++)
            {
                var last = s == slices.Count - 1;
                var g = last ? goodLeft : goodLeft / 2;
                var d = last ? defectLeft : defectLeft / 2;
                goodLeft -= g;
                defectLeft -= d;
                var code = d > 0 ? RandomDefectCode(line.Code) : null;
                AddResult(id, day, shift, g, d, code, slices[s].Start, slices[s].End);
            }
        }
    }

    private int RandomPlan(LineDef line, string shift, int ordersInSlot)
    {
        var steps = (line.MaxPlan - line.MinPlan) / line.Step;
        var plan = line.MinPlan + (_rng.Next(steps + 1) * line.Step);
        if (shift == NightShift)
        {
            plan = plan * 9 / 10 / line.Step * line.Step;
        }

        return ordersInSlot == 1 ? plan : plan / 2 / line.Step * line.Step;
    }

    private string RandomDefectCode(string line)
    {
        var typical = line switch
        {
            "L1" => "D01",
            "L2" => "D03",
            _ => "D04",
        };
        var r = _rng.NextDouble();
        return r < 0.6 ? typical : r < 0.85 ? "D02" : "D99";
    }

    private List<(TimeOnly Start, TimeOnly End)> FullShiftSlices(string shift)
    {
        var start = ShiftStart(shift).AddMinutes(_rng.Next(0, 4) * 5);
        var end = ShiftStart(shift).AddHours(9).AddMinutes(_rng.Next(0, 31) * 5);

        // Some full-shift orders are entered in two parts (before/after the meal break).
        if (_rng.NextDouble() < 0.3)
        {
            var breakStart = ShiftStart(shift).AddHours(4).AddMinutes(_rng.Next(0, 7) * 5);
            return [(start, breakStart), (breakStart.AddMinutes(60), end)];
        }

        return [(start, end)];
    }

    private List<(TimeOnly Start, TimeOnly End)> HalfShiftSlices(string shift, int half)
    {
        var shiftStart = ShiftStart(shift);
        return half == 0
            ? [(shiftStart.AddMinutes(_rng.Next(0, 4) * 5), shiftStart.AddHours(4).AddMinutes(_rng.Next(0, 7) * 5))]
            : [(shiftStart.AddHours(5).AddMinutes(_rng.Next(0, 4) * 5), shiftStart.AddHours(10).AddMinutes(_rng.Next(0, 19) * 5))];
    }

    private static TimeOnly ShiftStart(string shift) => shift == DayShift ? new TimeOnly(8, 0) : new TimeOnly(20, 0);

    private void AddEdgeCase(
        DateOnly day, string line, string shift, string itemCode, int plan,
        (int Good, int Defect, string? Code, string Start, string End)[] results, string? remark = null)
    {
        var id = AddOrder(day, line, shift, itemCode, plan, remark);
        foreach (var r in results)
        {
            AddResult(id, day, shift, r.Good, r.Defect, r.Code,
                TimeOnly.ParseExact(r.Start, "HH:mm", Inv), TimeOnly.ParseExact(r.End, "HH:mm", Inv));
        }
    }

    private int AddOrder(DateOnly day, string line, string shift, string itemCode, int plan, string? remark)
    {
        var id = _orders.Count + 1;
        var seq = _orders.Count(o => o.WorkDate == day && o.LineCode == line && o.Shift == shift) + 1;
        var orderNo = string.Format(Inv, "WO-{0:yyMMdd}-{1}{2}-{3:00}", day, line, shift == DayShift ? "D" : "N", seq);
        var createdAt = day.AddDays(-1).ToDateTime(new TimeOnly(16, 0).AddMinutes(_rng.Next(0, 60)));
        _orders.Add(new WorkOrderRow(id, orderNo, itemCode, line, plan, day, shift, remark,
            createdAt.ToString("yyyy-MM-dd HH:mm:ss", Inv)));
        return id;
    }

    private void AddResult(int orderId, DateOnly day, string shift, int good, int defect, string? code, TimeOnly start, TimeOnly end)
    {
        // Operators enter results right after the slice ends; night slices ending in the morning
        // (or starting after midnight) are entered on the next calendar day.
        var crossesMidnight = shift == NightShift && (end < start || start < new TimeOnly(12, 0));
        var enteredDay = crossesMidnight ? day.AddDays(1) : day;
        var enteredAt = enteredDay.ToDateTime(end).AddMinutes(_rng.Next(3, 25));
        _results.Add(new ResultRow(orderId, good, defect, code,
            start.ToString("HH:mm", Inv), end.ToString("HH:mm", Inv), enteredAt.ToString("yyyy-MM-dd HH:mm:ss", Inv)));
    }

    private string Render()
    {
        var sb = new StringBuilder();
        sb.AppendLine("-- Generated by LegacyRescue.DbTool gen-seed. Do not edit by hand; regenerate instead.");
        sb.AppendLine(Inv, $"-- Period: {FirstDay:yyyy-MM-dd} ~ {LastDay:yyyy-MM-dd} (Sundays off), random seed {RandomSeed}.");
        sb.AppendLine();
        sb.AppendLine("BEGIN TRANSACTION;");
        sb.AppendLine();

        foreach (var i in Items)
        {
            sb.AppendLine(Inv, $"INSERT INTO Item (ItemCode, ItemName, Unit) VALUES ({Q(i.Code)}, {Q(i.Name)}, {Q(i.Unit)});");
        }

        sb.AppendLine();
        foreach (var (code, name) in DefectCodes)
        {
            sb.AppendLine(Inv, $"INSERT INTO DefectCode (Code, Name) VALUES ({Q(code)}, {Q(name)});");
        }

        sb.AppendLine();
        foreach (var o in _orders)
        {
            sb.AppendLine(Inv,
                $"INSERT INTO WorkOrder (WorkOrderId, OrderNo, ItemCode, LineCode, PlanQty, WorkDate, Shift, Remark, CreatedAt) VALUES ({o.Id}, {Q(o.OrderNo)}, {Q(o.ItemCode)}, {Q(o.LineCode)}, {o.PlanQty}, {Q(o.WorkDate.ToString("yyyy-MM-dd", Inv))}, {Q(o.Shift)}, {Q(o.Remark)}, {Q(o.CreatedAt)});");
        }

        sb.AppendLine();
        var resultId = 1;
        foreach (var r in _results.OrderBy(r => r.EnteredAt, StringComparer.Ordinal).ThenBy(r => r.WorkOrderId))
        {
            sb.AppendLine(Inv,
                $"INSERT INTO ProductionResult (ResultId, WorkOrderId, GoodQty, DefectQty, DefectCode, StartTime, EndTime, EnteredAt) VALUES ({resultId++}, {r.WorkOrderId}, {r.GoodQty}, {r.DefectQty}, {Q(r.DefectCode)}, {Q(r.StartTime)}, {Q(r.EndTime)}, {Q(r.EnteredAt)});");
        }

        sb.AppendLine();
        sb.AppendLine("COMMIT;");
        return sb.ToString();
    }

    private static string Q(string? value) => value is null ? "NULL" : "'" + value.Replace("'", "''", StringComparison.Ordinal) + "'";
}
