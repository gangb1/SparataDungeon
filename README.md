# 🧙 Sparta Dungeon - 텍스트 기반 C# RPG 게임

**Sparta Dungeon**은 콘솔 환경에서 작동하는 간단한 텍스트 기반 RPG 게임입니다.

---

## 📸 게임 화면 예시

![스크린샷 2025-04-17 202235](https://github.com/user-attachments/assets/99441a0f-15fb-430d-91a9-f46ba40b529d)

## 🚀 기능 요약

- ✅ 캐릭터 생성 (전사 / 궁수)
- ✅ 인벤토리 및 장비 시스템
- ✅ 상점에서 아이템 구매
- ✅ 던전 도전 및 보상
- ✅ HP 시스템 및 휴식 기능
- ✅ 게임 저장 / 불러오기

---

## 🛠️ 실행 방법

### 1. .NET SDK 설치
- [.NET SDK 다운로드](https://dotnet.microsoft.com/download)
- 설치 확인:  
  ```
  dotnet --version
  ```
### 2. 저장소 클론 또는 다운로드
```
git clone https://github.com/your-username/sparta-dungeon.git
cd sparta-dungeon
```

```
dotnet run
```
⚠️ Program.cs에 모든 클래스가 포함된 경우 이 파일만 프로젝트 내에 위치하면 됩니다.
추가적인 .csproj 파일 생성이 필요할 수도 있습니다.

### 💾 저장 파일 정보
게임은 save.json 파일로 저장됩니다.

저장 위치는 실행 디렉토리입니다.

📁 파일 구조 예시
## 📁 프로젝트 구조

```
SpartaDungeon/
├── SpartaDungeon.cs             # 메인 프로그램 및 전체 로직이 담긴 파일
├── save.json                    # 저장된 게임 데이터 파일
├── README.md                    # 프로젝트 설명서
├── .gitignore                   # Git 무시할 파일 목록
└── SpartaDungeon.sln           # 솔루션 파일 (있는 경우)
```


