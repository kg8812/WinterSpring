대규모 팀으로 개발한 2D 메트로베니아 액션 게임 프로젝트입니다.
개발 팀장을 맡아서 Unity를 기반으로 게임 시스템과 공용 프레임워크를 직접 설계하고 구현했습니다.

--- 

## 게임 다운로드

[게임 파일링크](https://github.com/kg8812/WinterSpring/releases/tag/Release)

<br/><br/>

---

## 프로젝트 정보

### 개발 기간

2023 ~ 2025 (약 2년)


### 개발 인원

개발 3명, 전체 팀원 약 20명


### 개발 환경

Unity, C#, Spine

---

## 플레이 영상


[게임 플레이](https://www.youtube.com/watch?v=Hv7472TJFvo)

<br/>

[보스전](https://www.youtube.com/watch?v=MNkDfnDJlTs)

<br/><br/>

---

## 담당 역할

프로젝트 초기 단계부터 참여하여 개발팀장으로써 개발 일정 및 팀관리, 게임 시스템과 구조 설계 및 구현을 담당했습니다.

### 주요 담당 영역

- 게임 핵심 시스템 설계 및 구현
- 플레이어 스킬 및 스킬트리 시스템
- 무기 및 장비 시스템
- 버프 시스템
- 몬스터 AI 행동트리
- 섹터 기반 맵 로딩 시스템
- 비개발자용 맵 제작 툴
- 게임 데이터 관리 시스템
- Spine 애니메이션 및 이펙트 적용 시스템

--- 

## 주요 시스템


### 플레이어 스킬 및 스킬트리

플레이어가 다양한 스킬을 선택하여 성장할 수 있는 스킬 트리 시스템을 구현했습니다.

- 스킬 scriptable 데이터 기반 설계

- 전략 패턴과 데코레이터 패턴을 이용한 스킬 쿨타임 및 효과 관리

- 방랑자, 데코레이터 패턴을 이용한 스킬트리 설계

<br/>

Scriptable 기반 스킬 데이터

<img width="995" height="789" alt="image" src="https://github.com/user-attachments/assets/4eebb1e9-05b8-45ef-aca1-e20760b979d6" />

<br/>
다른 팀원들이 쉽게 값을 수정할 수 있게 각 스킬 데이터를 Scriptable로 만들고 OdinSerailizer를 사용해 인스펙터를 깔끔하게 정리하여 구현하였습니다.
<br/>
<br/>

인게임 스킬트리

<img width="1502" height="873" alt="image" src="https://github.com/user-attachments/assets/eedc0c2b-8a05-4a6b-a5a4-d058aa8aa69f" />
<img width="1489" height="858" alt="image" src="https://github.com/user-attachments/assets/63f1c5db-7e39-40a1-ae47-b084a69843d7" />

<br/>
인게임에서 동적으로 스킬트리를 통해 스킬에 추가적인 효과를 추가,제거할 수 있게 구현하였습니다.
추가적인 효과를 가진 SkillTree 클래스를 인터페이스를 이용한 방랑자 패턴과 이벤트 형식으로 ActiveSkill에 붙일 수 있도록 구현하였으며, 
SkillDatas 클래스를 통해 관리됩니다.
<br/>
<br/>

### 무기 및 악세서리 시스템

다양한 무기와 악세서리의 효과들을 구현했으며
장착할 수 있도록 인벤토리, 무기 스파인 애니메이션 슬롯구조를 설계했습니다.

구현 기능

- 장비 스탯 및 효과 적용
- 테이블과 연동해 animator overrider를 이용한 무기별 다른 애니메이션 적용
- 무기 및 악세서리 인벤토리
- 무기 스파인 장착 시스템

무기 컴포넌트 구조 

<img width="436" height="757" alt="image" src="https://github.com/user-attachments/assets/61311ced-51b1-4f16-8ae1-2ac9a3107a5e" />


무기를 프리팹화하여 각종 데이터를 인스펙터에서 입력하고 수정할 수 있게 하였습니다.
<br/>
<br/>

무기 실제 적용

![Animation](https://github.com/user-attachments/assets/926decc2-14ed-4d25-9e8c-25039f4b8f38)

무기 스프라이트를 스파인 무기 슬롯에 장착시키면 스파인에서 작업한대로 애니메이션이 이렇게 실행이 됩니다.
무기를 장착하면 스파인 무기 슬롯에 해당 무기 스프라이트로 변경되도록 구조를 짰습니다.

<br/>
<br/>

### 보스몬스터 AI 행동트리

복잡한 몬스터 행동을 구현하기 위해 행동트리 기반 AI 툴을 제작했습니다.
기본 툴 제작 기반은 [[링크](https://youtu.be/nKpM98I7PeM)]를 참고했으며
기반 제작 이후 어레인지는 직접 했습니다.

구현 기능

- unity내 행동트리 제작 툴
- 노드 종류에 따른 각종 기능들 (점프, 플레이어 추적, 대쉬 등)
- 보스 패턴 관리

<br/>

<img width="1901" height="996" alt="image" src="https://github.com/user-attachments/assets/45f8277c-15aa-4a2d-a456-feda8c1fa79f" />

<br/>

보스 패턴 구현을 위해 수십가지의 기능 노드를 만들었고, 제가 아닌 타 팀원분들도 UI를 통해 작업할 수 있도록 최대한 기능을 추가하여 쉽게 만들었습니다.

실제로 만들어진 패턴은 위의 보스전 영상을 참고해주시면 감사하겠습니다.

<br/>
<br/>

### 섹터 기반 맵 동적 로딩 시스템

대규모 맵에서 성능개선을 위해 섹터 단위 동적 로딩 시스템을 구현했습니다.
플레이어의 현재 위치에 따라 주변 섹터만 로드하고 나머지는 언로드하도록 설계했습니다.

주요 기능

- 로딩 큐 시스템
- 섹터 로딩 관리용 매니저

<br/>

플레이어에 위치에 따라 Load와 UnLoad 되는 모습

![Animation](https://github.com/user-attachments/assets/36c31c25-6cab-4407-93f8-ea3e49288ad0)

<br/><br/>

### 섹터 제작 툴

레벨 디자이너(비 개발자)도 쉽게 맵을 제작할 수 있도록 유니티 에디터 기반 툴을 제작했습니다.

기능

- 섹터 단위 맵 제작
- 몬스터 및 오브젝트 배치
- scriptable로 섹터 데이터 자동 저장

인스펙터

<img width="444" height="797" alt="image" src="https://github.com/user-attachments/assets/61b477a2-5c68-4b08-8498-457136a649cc" />

<br/>

버튼 입력시 자동으로 데이터 생성

<img width="863" height="815" alt="image" src="https://github.com/user-attachments/assets/9cb42665-f321-43a7-a53f-f51b7ba1e323" />

섹터를 동적으로 로딩하다보니 섹터를 에디터에서 만든 그대로 다시 불러올 수 있도록 데이터를 따로 저장할 필요성이 있었습니다.
해당 기능을 통해 레벨 디자인분들도 복잡한 과정없이 섹터를 제작할 때 배치된 몬스터, 데이터를 수정하고 저장할 수 있도록 하였습니다.

<br/>

---

## 개발 과정에서 배운 점

이 프로젝트를 통해 다음과 같은 경험을 얻을 수 있었습니다.

- 게임 시스템 설계 경험
- 대규모 Unity 프로젝트 구조 관리
- 팀장으로써 개발 팀원 및 일정 관리
- 타 부서들과의 팀 프로젝트 협업 경험

또한 프로젝트에서 공용으로 사용할 수 있는 기능들을 분리하여
**Unity 프로토타입 프로젝트(Prototype)**로 리팩토링했습니다.

[프로토타입 링크](https://github.com/kg8812/Prototype)
