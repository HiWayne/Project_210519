using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ArrySort
{
    // 冒泡排序
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

    // 选择排序
    public static void SelectSort(ref int[] store)
    {
        int minIndex;
        int temp;

        for (int i = 0, length = store.Length; i < length; i++)
        {
            minIndex = i;

            // 将当前的array[j]与array[minIndex]作比较，如果array[j]更小，则替换min的当前索引
            for (int j = i + 1; j < length; j++)
            {
                if (store[minIndex] > store[j])
                {
                    minIndex = j;
                }
            }
            // 当第二个for循环完成时，array[minIndex]中存储的就是当前最小元素
            // 将array[minIndex]与array[i]交换
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
            // 在middle左边找到一个比middle大的值
            while (i < storeCount && store[i] < middle)
                i++;
            // 在middle右边找到一个比middle小的值
            while (j > 0 && store[j] > middle)
                j--;
            // 当i=j时,middle左边都是比middle小的数,右边都是比middle大的;跳出循环
            if (i == j)
                break;

            temp = store[i];
            store[i] = store[j];
            store[j] = temp;

            // 如果两个值相等,且等于middle,为避免进入死循环,j--
            if (store[i] == store[j])
                j--;
        }

        QuickSort(ref store, startIndex, i);
        QuickSort(ref store, i + 1, storeCount);
    }
}