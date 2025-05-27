using System;
using UnityEngine;

namespace StageData
{
    [Serializable]
    public class StageData
    {
        // �������� ��ȣ, ���̺� ����, ���� ���� ���� �� ������, ����
        // ���� ���̺�� �������� �з��� ���? -> ����������ȣ, ���̺��ȣ, ���� ���� ���� �� ������, ����
        // ���߿� ���� ����ġ�� int�� ����

        public int stageNumber;
        public int waveNumber;
        public float gainMultiplier;
    }
}
