using System;
using UnityEngine;

namespace StageData
{
    [Serializable]
    public class StageData
    {
        // 스테이지 번호, 웨이브 개수, 등장 몬스터 유형 및 마리수, 배율
        // 만약 웨이브랑 스테이지 분류의 경우? -> 스테이지번호, 웨이브번호, 등장 몬스터 유형 및 마리수, 배율
        // 나중에 골드랑 경험치는 int로 변경

        public int stageNumber;
        public int waveNumber;
        public float gainMultiplier;
    }
}
