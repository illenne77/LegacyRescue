# LegacyRescue — 레거시 윈도우 프로그램 현대화 데모

## 프로젝트 목적

크몽 프리랜서 포트폴리오용 데모 프로젝트다. "구형 윈도우 프로그램을 기능을 깨뜨리지 않고 .NET 8로 현대화한다"는 서비스를 의뢰인(중소기업, 공장)에게 증명하는 것이 목표다.

의뢰인 코드를 공개할 수 없으므로 다음 흐름을 직접 재현한다.

1. 현장에서 흔히 보는 문제를 일부러 담은 **레거시 샘플 프로그램**을 만든다 (Before).
2. 그 프로그램을 분석해 **마이그레이션 진단 보고서**를 쓴다 (크몽 입문 상품의 견본).
3. **특성화 테스트(골든 마스터)**로 기존 동작을 고정한다.
4. 로직을 분리하고 **.NET 8 WPF + MVVM**으로 옮긴다 (After).
5. 전후 비교를 수치로 남긴다.

포트폴리오의 핵심 메시지는 "옮긴 뒤에도 결과가 100% 같다는 것을 테스트로 증명한다"이다.

## 개발자

- C/C++, C# 윈도우 개발 24년차.
- 기초 설명은 생략하고, 설계 판단의 근거와 트레이드오프 위주로 설명할 것.
- 응답은 한국어 존댓말. 코드 식별자·주석은 영어. 단, 레거시 샘플의 UI 문구는 한국어(국내 공장 프로그램 재현).

## 도메인: 소규모 공장 "생산 실적 관리"

- **작업지시**: 품목, 생산 라인, 계획 수량, 작업일, 교대(주간/야간)
- **실적 입력**: 작업지시별 양품 수량, 불량 수량, 불량 유형, 작업 시간
- **일별 리포트**: 라인·교대별 달성률, 불량률, 가동률, 합계
- **내보내기**: CSV (엑셀에서 여는 용도)

## 솔루션 구조

```
legacy/
  LegacyRescue.Legacy.WinForms/   # .NET Framework 4.8 WinForms — Before. 의도적 레거시 스타일
  LegacyRescue.Legacy.Calc/       # (7단계, 선택) C++ 계산 DLL
modern/
  src/
    LegacyRescue.Domain/          # net8.0 — 엔터티, 값 객체, 계산 규칙
    LegacyRescue.Application/     # net8.0 — 유스케이스 서비스, 리포트 생성
    LegacyRescue.Infrastructure/  # net8.0 — SQLite 저장소, CSV 내보내기, 설정
    LegacyRescue.Native/          # net8.0 — (7단계, 선택) C++ DLL P/Invoke 래퍼
    LegacyRescue.App/             # net8.0-windows — WPF + MVVM — After
  tests/
    LegacyRescue.Tests/           # net8.0 — 단위 테스트
    LegacyRescue.Characterization/# net8.0 — 골든 마스터 비교 테스트
data/
  schema.sql, seed.sql            # 샘플 DB를 재현하는 텍스트 스크립트 (DB 파일은 커밋하지 않음)
  golden/                         # 레거시가 출력한 기준 결과 (CSV/JSON, 커밋함)
docs/
  PLAN.md                         # 단계별 계획 (진행 상황 갱신)
  diagnosis-report.md             # 마이그레이션 진단 보고서
  before-after.md                 # 전후 비교 수치
```

## 레거시 샘플 작성 규칙 (Before 전용)

레거시 샘플은 "일부러 나쁘게" 만든다. 단, 실제로 정상 동작해야 하고, 각 문제는 진단 보고서에서 다룰 수 있도록 의도적으로 넣는다.

- 버튼 클릭 핸들러 안에서 SQL 문자열 직접 조립 (파라미터 미사용 구간 포함)
- 하나의 거대한 Form에 화면 로직·DB 접근·계산이 혼재
- 전역 `static` 상태, 하드코딩된 경로, INI 파일 설정
- UI 스레드에서 동기 DB 조회 (대량 데이터 조회 시 화면 멈춤)
- HiDPI 미대응 (`AutoScaleMode` 혼용, 고정 픽셀 좌표)
- 계산 규칙의 "숨은 특이점"을 최소 1~2개 포함한다. 예: `Math.Round` 기본값(은행가 반올림)에 의한 반올림 차이, 야간 교대의 날짜 경계 처리. 특성화 테스트가 이 특이점을 잡아내고, 진단 보고서에서 "유지할지 수정할지"를 의사결정 항목으로 다룬다.
- 레거시에 숨은 문제를 넣는 위치와 의도는 `docs/diagnosis-report.md`에만 기록하고, 레거시 코드 주석으로 정답을 노출하지 않는다.

## 현대화 설계 원칙 (After)

1. **동작 보존이 최우선**: 특성화 테스트가 모두 통과하기 전에는 기능 개선이나 리팩터링 범위를 넓히지 않는다. 특이점 수정은 진단 보고서의 결정에 따라 별도 커밋으로 하고, 골든 파일 갱신 사유를 남긴다.
2. **계층 분리**: Domain은 어떤 인프라(DB, 파일, UI)도 참조하지 않는다. App은 Application 서비스만 사용한다.
3. **MVVM**: CommunityToolkit.Mvvm 사용. 코드비하인드에는 순수 뷰 로직만 둔다.
4. **비동기 I/O**: DB·파일 작업은 `async`로, UI 멈춤이 없어야 한다.
5. **설정**: INI → `appsettings.json`. 레거시 INI를 읽어 변환하는 마이그레이션 도구를 제공한다(의뢰인 현장 이전 시나리오).
6. **DB 호환**: 레거시와 같은 SQLite 스키마를 그대로 읽는다. 스키마 변경은 마이그레이션 스크립트로만 한다.
7. **HiDPI**: Per-Monitor V2 대응.

## 특성화 테스트 방식

- 레거시 앱에 숨김 명령줄 옵션(예: `--export-golden <db> <outdir>`)을 두어, 고정된 시드 DB로부터 일별 리포트와 CSV 결과를 `data/golden/`에 출력한다. (레거시 원래 기능 외 유일한 추가 코드이며, 실제 현장에서도 "테스트용 출력 지점만 최소로 추가"하는 기법임을 문서에 설명한다.)
- 골든 파일 생성은 Windows에서 레거시 앱을 실행해 한다.
- `LegacyRescue.Characterization`은 같은 시드 DB로 신규 로직을 실행해 골든 파일과 비교한다. 이 테스트는 `net8.0`이므로 클라우드(Linux)에서도 실행된다.

## 기술 스택

- Before: .NET Framework 4.8, WinForms, System.Data.SQLite, INI 파일
- After: .NET 8, WPF, CommunityToolkit.Mvvm, Microsoft.Data.Sqlite, Dapper, Microsoft.Extensions.Hosting/Logging/Configuration
- 테스트: xUnit
- 배포: 단일 파일 self-contained exe

## 작업 환경 분담 (중요)

이 저장소는 Claude Code 클라우드 세션(Linux)과 로컬 Windows 세션 양쪽에서 작업한다.

| 프로젝트 | 대상 프레임워크 | 작업 위치 |
|---|---|---|
| Legacy.WinForms | `net48` | 로컬 Windows (실행·골든 파일 생성) |
| Legacy.Calc (C++) | MSVC | 로컬 Windows |
| Domain / Application / Infrastructure | `net8.0` | 클라우드 가능 |
| Tests / Characterization | `net8.0` | 클라우드 가능 |
| Native | `net8.0` | 코드는 클라우드, 실행 검증은 로컬 |
| App (WPF) | `net8.0-windows` | 로컬 Windows |

- 클라우드에서는 WinForms·WPF 실행, C++ 빌드를 할 수 없다. 해당 작업은 가능한 범위에서 코드 작성·컴파일 확인까지만 하고, 실행 검증은 PLAN.md에 "로컬 검증 필요"로 표시한다.
- `net48` 프로젝트는 `Microsoft.NETFramework.ReferenceAssemblies`를 참조해 Linux에서도 컴파일 확인이 가능하도록 한다. Windows 전용 `net8.0-windows` 프로젝트는 `EnableWindowsTargeting=true`.
- 작업 시작 시 현재 환경(OS)을 먼저 확인한다.

## 공개 저장소 주의사항

이 저장소는 public이다.

- 이전·현 직장의 코드, 실제 고객 프로그램, 내부 노하우를 옮기지 않는다. 레거시 샘플은 전부 새로 작성한 가상의 프로그램이다.
- 실존 회사명·제품명을 샘플 데이터에 쓰지 않는다.
- DB 파일(`*.db`)은 커밋하지 않고 `data/*.sql`로 재현한다. 골든 파일은 커밋한다.
- 비밀값을 커밋하지 않는다.

## 작업 방식

- `docs/PLAN.md`의 단계를 순서대로 진행한다. 한 번에 한 단계만.
- 각 단계 시작 전 해당 단계의 설계(파일 목록, 주요 타입)를 짧게 제시하고 확인을 받는다.
- 각 단계 종료 시: 빌드 성공, 테스트 통과, `docs/PLAN.md` 체크박스 갱신, 단계 단위 git 커밋.

## 자주 쓰는 명령

```
dotnet build modern/LegacyRescue.Modern.sln
dotnet test modern/tests/LegacyRescue.Characterization
dotnet test modern/tests/LegacyRescue.Tests
dotnet run --project modern/src/LegacyRescue.App
```
