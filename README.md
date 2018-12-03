# DP-Project (Defence Plus Project)
수학적 활동과 함께 재미 요소를 추가하여 어린아이들의 연산 능력 향상에 도움을 주기 위한 게임
<br/><br/>
- 개발 인원 : 4명
- 개발 기간 : 11.08~11.27 (20일)
- 개발 환경 : Unity, C#
<br/><br/><br/>

# 기획 
- 게임 장르 : 퍼즐 디펜스 (블록을 드래그하여 정중앙의 숫자를 맞추는 더하기 퍼즐 + 타워 디펜스)
- 게임 목적 : 퍼즐을 풀면 얻게되는 재화로 유닛을 소환하여 몬스터로부터 성을 지키면서 최대한 많은 점수를 얻는 것
- 시나리오 보드
<table>
  <tr>
<td><img src="https://user-images.githubusercontent.com/25303946/49354298-8e5a9b80-f705-11e8-93e8-5a4ea524972c.png" width="100" height="200"/></td>
<td><img src="https://user-images.githubusercontent.com/25303946/49354299-8e5a9b80-f705-11e8-853b-5e73cec2987a.png" width="300" height="200"/></td>
<td><img src="https://user-images.githubusercontent.com/25303946/49354302-8e5a9b80-f705-11e8-9327-ed747c37e0d1.png" width="250" height="200"/></td>
<td><img src="https://user-images.githubusercontent.com/25303946/49354303-8ef33200-f705-11e8-8937-d13055a66b60.png" width="100" height="200"/></td>
  </tr>
</table>
<br/><br/>

# 실행 스크린샷
<table>
  <tr>
<td><img src="https://user-images.githubusercontent.com/25303946/49354491-80594a80-f706-11e8-83f4-9c04e0b64bf9.png" width="150" height="300"/></td>
<td><img src="https://user-images.githubusercontent.com/25303946/49354501-89e2b280-f706-11e8-8047-69c6db5ccdb6.png" width="150" height="300"/></td>
<td><img src="https://user-images.githubusercontent.com/25303946/49354502-8bac7600-f706-11e8-9109-8542eda6ea8e.png" width="150" height="300"/></td>
<td><img src="https://user-images.githubusercontent.com/25303946/49354503-8fd89380-f706-11e8-9b95-0793c7491f5c.png" width="150" height="300"/></td>
<td><img src="https://user-images.githubusercontent.com/25303946/49354505-91a25700-f706-11e8-82c2-86485f8e200f.png" width="150" height="300"/></td>
  </tr>
</table>
<br/><br/>

# 구현 방식
- Manager Script를 통한 역할 분담 : Game, Score, Sound, Unit, Monster, Puzzle Manager
- Object Pool Pattern : 유닛, 몬스터, 퍼즐, 파티클에 대한 객체 생성, 삭제 방식을 Object pool로 대체
- 의존성 주입(Dependency Injection)을 이용한 스크립트 단일화
<td><img src="https://user-images.githubusercontent.com/25303946/49357406-86edbf00-f712-11e8-8ee2-e6fc20ab5b94.png" width="800" height="250"/></td>
<br/><br/><br/>

# 보완점 및 차후 계획
- 서버 구현
- 게임 확장 : 난이도 분리, 유닛과 몬스터 종류 추가, 아이템 추가 등의 확장 작업
- 원활한 단위테스트를 위한 Logic과 UI의 분리 작업
