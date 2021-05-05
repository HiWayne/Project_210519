using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ArrySort
{
    // ð������
    public static void BubbleSort(ref int[] store)
    {
        int temp;

        for (int i = 0, length = store.Length - 1; i < length; i++)
        {
            for (int j = 0; j < length - i; j++)
            {
                if (store[j] > store[j + 1])
                {
                    temp = store[j + 1];
                    store[j + 1] = store[j];
                    store[j] = temp;
                }
            }
        }
    }

    // ѡ������
    public static void SelectSort(ref int[] store)
    {
        int minIndex;
        int temp;

        for (int i = 0, length = store.Length; i < length; i++)
        {
            minIndex = i;

            // ����ǰ��array[j]��array[minIndex]���Ƚϣ����array[j]��С�����滻min�ĵ�ǰ����
            for (int j = i + 1; j < length; j++)
            {
                if (store[minIndex] > store[j])
                {
                    minIndex = j;
                }
            }
            // ���ڶ���forѭ�����ʱ��array[minIndex]�д洢�ľ��ǵ�ǰ��СԪ��
            // ��array[minIndex]��array[i]����
            temp = store[i];
            store[i] = store[minIndex];
            store[minIndex] = temp;
        }
    }

    public static void QuickSort(ref int[] store, int startIndex, int storeCount)
    {
        if (startIndex >= storeCount)
            return;

        int i = startIndex;
        int j = storeCount - 1;
        int middle = store[(startIndex + storeCount) / 2];
        int temp;

        while (true)
        {
            // ��middle����ҵ�һ����middle���ֵ
            while (i < storeCount && store[i] < middle)
                i++;
            // ��middle�ұ��ҵ�һ����middleС��ֵ
            while (j > 0 && store[j] > middle)
                j--;
            // ��i=jʱ,middle��߶��Ǳ�middleС����,�ұ߶��Ǳ�middle���;����ѭ��
            if (i == j)
                break;

            temp = store[i];
            store[i] = store[j];
            store[j] = temp;

            // �������ֵ���,�ҵ���middle,Ϊ���������ѭ��,j--
            if (store[i] == store[j])
                j--;
        }

        QuickSort(ref store, startIndex, i);
        QuickSort(ref store, i + 1, storeCount);
    }
}