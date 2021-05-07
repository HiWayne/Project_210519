using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBasic;
using UnityEngine.UI;
using DG.Tweening;

public class SortManager : MonoBehaviour
{
    const float AnimDelayDuration = 0.5f;

    public SortType sortType;

    public List<InfoItem> infoItems;
    List<int> infoItemValues;

    // 执行按钮
    public Button startExpBtn;
    public Button singleLineRunBtn;
    public Button autoRunBtn;
    public float taskDuration;

    // 任务相关
    TaskRunner taskRunner;
    Queue<Task> tasks;
    Task currentTask;

    private void Start()
    {
        infoItemValues = new List<int>(infoItems.Count);

        for (int i = 0, length = infoItems.Count; i < length; i++)
            infoItemValues.Add(infoItems[i].Init());

        taskRunner = new TaskRunner();

        #region 按钮注册事件
        startExpBtn.onClick.AddListener(() =>
        {
            // 已经创建任务
            if (tasks != null)
                return;

            CreateTasks();
        });
        singleLineRunBtn.onClick.AddListener(() =>
        {
            // 结束当前的任务
            //if (currentTask != null)
            //    currentTask.Stop();

            // 如果没有创建任务或者队列没有任务
            if (tasks == null || tasks.Count <= 0 || (currentTask != null && currentTask.Status == TaskStatus.Running))
                return;

            // 取出任务
            currentTask = tasks.Dequeue();

            // 执行任务
            taskRunner.Add(currentTask);
        });
        autoRunBtn.onClick.AddListener(() =>
        {
            // 如果没有创建任务或者队列没有任务
            if (tasks == null || tasks.Count <= 0)
                return;

            // 如果当前没有任务或者当前任务已结束，则取出第一个任务
            if (currentTask == null || currentTask.Status == TaskStatus.End)
            {
                currentTask = tasks.Dequeue();
                taskRunner.Add(currentTask);
            }

            while (tasks.Count != 0)
            {
                Task tmp = tasks.Peek();

                currentTask.onStop += t => taskRunner.Add(tmp);

                currentTask = tasks.Dequeue();
            }
        });
        #endregion
    }

    private void Update()
    {
        taskRunner.Update();
    }

    void CreateTasks()
    {
        Transform tmpRoot;
        InfoItem tmpIT;
        tasks = new Queue<Task>();

        switch (sortType)
        {
            case SortType.BubbleSort:
                for (int i = 0, length = infoItems.Count - 1; i < length; i++)
                {
                    for (int j = 0; j < length - i; j++)
                    {
                        int index1 = j;
                        int index2 = j + 1;
                        Task task = new Task(taskDuration);

                        task.onStart = t =>
                        {
                            // 相关元素变红
                            infoItems[index1].SetHightlight();
                            infoItems[index2].SetHightlight();

                            if (infoItems[index1].Value > infoItems[index2].Value)
                            {
                                tmpRoot = infoItems[index1].transform.parent;

                                // 变更父物体
                                infoItems[index1].transform.SetParent(infoItems[index2].transform.parent);
                                infoItems[index2].transform.SetParent(tmpRoot);

                                // 交换位置动画
                                infoItems[index1].transform.DOLocalMoveX(0, taskDuration - AnimDelayDuration - 0.1f).SetDelay(AnimDelayDuration);
                                infoItems[index2].transform.DOLocalMoveX(0, taskDuration - AnimDelayDuration - 0.1f).SetDelay(AnimDelayDuration);

                                // 交换数据
                                tmpIT = infoItems[index1];
                                infoItems[index1] = infoItems[index2];
                                infoItems[index2] = tmpIT;
                            }
                        };
                        task.onStop = t =>
                        {
                            // 相关元素还原
                            infoItems[index1].SetNormal();
                            infoItems[index2].SetNormal();
                        };

                        tasks.Enqueue(task);
                    }
                }
                break;
            case SortType.SelectSort:
                for (int i = 0, length = infoItems.Count; i < length; i++)
                {
                    int minIndex = i;
                    int index1 = i;

                    // 寻找最小值位置
                    for (int j = i + 1; j < length; j++)
                    {
                        int index2 = j;
                        int index3 = minIndex;
                        Task task2 = new Task(taskDuration * 0.5f);

                        if (infoItems[index3].Value > infoItems[index2].Value)
                            minIndex = index2;
                        task2.onStart = t =>
                        {
                            // 相关元素变红
                            infoItems[index2].SetHightlight();
                            infoItems[index3].SetHightlight();
                        };
                        task2.onStop = t =>
                        {
                            // 相关元素还原
                            infoItems[index2].SetNormal();
                            infoItems[index3].SetNormal();
                        };

                        tasks.Enqueue(task2);
                    }

                    if (minIndex != index1)
                    {
                        // 交换位置
                        Task task1 = new Task(taskDuration);

                        task1.onStart = t =>
                        {
                            // 相关元素变红
                            infoItems[index1].SetHightlight();
                            infoItems[minIndex].SetHightlight();

                            tmpRoot = infoItems[index1].transform.parent;

                            // 变更父物体
                            infoItems[index1].transform.SetParent(infoItems[minIndex].transform.parent);
                            infoItems[minIndex].transform.SetParent(tmpRoot);

                            // 交换位置动画
                            infoItems[index1].transform.DOLocalMoveX(0, taskDuration - AnimDelayDuration - 0.1f).SetDelay(AnimDelayDuration);
                            infoItems[minIndex].transform.DOLocalMoveX(0, taskDuration - AnimDelayDuration - 0.1f).SetDelay(AnimDelayDuration);

                            // 交换数据
                            tmpIT = infoItems[index1];
                            infoItems[index1] = infoItems[minIndex];
                            infoItems[minIndex] = tmpIT;
                        };
                        task1.onStop = t =>
                        {
                            // 相关元素还原
                            infoItems[index1].SetNormal();
                            infoItems[minIndex].SetNormal();
                        };

                        tasks.Enqueue(task1);
                    }
                }
                break;
            case SortType.QuickSort:
                break;
            default:
                break;
        }
    }
}