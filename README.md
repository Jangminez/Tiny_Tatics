# Tiny Tactics
---
전략과 타이밍이 승리를 결정하는 캐주얼 카드 배틀 게임

⭐️ How to Play: Intro Scene에서 플레이시작

<img width="200" alt="스크린샷 2025-06-18 오후 8 09 22" src="https://github.com/user-attachments/assets/0b9f1fbd-ba18-41ac-af27-dfb7a534d9d9" />

---

## Game Features

### 카드 소환 시스템
- 각 유닛 카드는 공격 방식, 체력, 마나 코스트가 다름
- 드래그 앤 드롭 방식으로 맵에 배치
- 마나는 전투 중 시간에 따라 자동 충전
- 카드는 강화될수록 **⭐ 별 개수**로 시각화됨
- 카드에 따라 Enemy/Building 등 공격하는 대상이 다름
  
<img width="200" alt="스크린샷 2025-06-18 오후 8 11 14" src="https://github.com/user-attachments/assets/33a07596-d12e-4403-9a07-d97752cb94a2" />  <img width="200" alt="스크린샷 2025-06-18 오후 8 15 10" src="https://github.com/user-attachments/assets/1f9754a1-adc1-46c1-b87b-6144c261b39c" />

---

### 전투 시스템
- 목표: 제한 시간 내 좀비 성 파괴
- 유닛은 AI Navigation을 통해 길을 찾고 자동으로 타겟팅된 적과 교전
- 맵은 랜덤으로 로드되고 매판 새로운 전략 요구
- 전투 종료 후 보유한 카드 중 1개 랜덤 강화
  
<img width="200" alt="스크린샷 2025-06-18 오후 8 15 10" src="https://github.com/user-attachments/assets/2d693592-2c43-4c50-8a8e-be4a56656c9b" />  <img width="200" alt="스크린샷 2025-06-18 오후 8 11 26" src="https://github.com/user-attachments/assets/44d2d03b-53d3-4a54-8ea2-3e12ee1468cc" />

---

### 상점 시스템
- 유닛 카드 및 유물 구매 가능
- 유물은 전투 전반에 영향을 주는 패시브 능력 제공
- 골드를 소모하여 새로운 카드 또는 유물 획득
  
<img width="200" alt="스크린샷 2025-06-18 오후 8 12 01" src="https://github.com/user-attachments/assets/89cd1801-7b41-43d6-93cb-c73819fc4c41" />  <img width="200" alt="스크린샷 2025-06-18 오후 8 12 12" src="https://github.com/user-attachments/assets/9e59db36-ca1c-41d8-83c8-f7f4f8bf0343" />

---

### 카드 강화 시스템
- 골드를 통해 카드 강화하여 스탯 향상
- 강화 후 카드 아래의 '별'이 증가됨
  
<img width="200" alt="스크린샷 2025-06-18 오후 8 16 26" src="https://github.com/user-attachments/assets/4eab83f0-2087-426b-b967-a1299854cca9" />  <img width="200" alt="스크린샷 2025-06-18 오후 8 16 17" src="https://github.com/user-attachments/assets/07591d3e-9bea-4a3c-8630-cf80688223e2" />

---

### 스테이지 및 루트 맵
- 각기 다른 전투/상점 노드가 존재하는 분기형 스테이지
    
<img width="200" alt="스크린샷 2025-06-18 오후 8 13 23" src="https://github.com/user-attachments/assets/46e31188-d86a-41fa-be9f-03865f1e03d2" />

---

### 설정 시스템
- 배경음, 효과음 개별 조절 가능
  
<img width="200" alt="스크린샷 2025-06-18 오후 8 15 40" src="https://github.com/user-attachments/assets/6580b008-c68c-4f45-aeb5-acd6d403e678" />


---

## Tech Stack

| 항목           | 내용                          |
|----------------|-------------------------------|
| 언어            | C#                            |
| 엔진            | Unity                         |
| 타겟 플랫폼     | Android, iOS (예정)           |
| 리소스 관리     | JSON                          |

---

## 👨‍💻 프로젝트 팀 구성 및 역할

| 이름      | 역할 | 담당 업무                            |
| ------- | -- | -------------------------------- |
| **이수명** | 팀장 | 유닛 소환 카드 시스템, 사운드, UI 이미지        |
| **이창주** | 팀원 | 데이터 관리 (플레이어 유닛, 적, 유물), 이펙트 추가  |
| **백성은** | 팀원 | UI 관련 기능 (상점, 카드 업그레이드, 전투 UI 등) |
| **장유성** | 팀원 | 유닛 FSM 및 애니메이션, 전투 시스템           |
| **장민제** | 팀원 | 스테이지 프리셋, 노드식 맵 구성, 인트로 제작       |

---
