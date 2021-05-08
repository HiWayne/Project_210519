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

    // 
    public List<InfoItem> infoItems;
    List<int> infoItemValues;

    // �������
    public GameObject[] codeObjs;

    // ִ�а�ť
    public Button startExpBtn;
    public Button singleLineRunBtn;
    public Button autoRunBtn;
    public float taskDuration;

    // �������
    TaskRunner taskRunner;
    Queue<Task> tasks;
    Task currentTask;

    private void Start()
    {
        infoItemValues = new List<int>(infoItems.Count);

        for (int i = 0, length = infoItems.Count; i < length; i++)
            infoItemValues.Add(infoItems[i].Init());

        taskRunner = new TaskRunner();

        #region ��ťע���¼�
        startExpBtn.onClick.AddListener(() =>
        {
            // �Ѿ���������
            if (tasks != null)
                return;

            CreateTasks();
        });
        singleLineRunBtn.onClick.AddListener(() =>
        {
            // ������ǰ������
            //if (currentTask != null)
            //    currentTask.Stop();

            // ���û�д���������߶���û������
            if (tasks == null || tasks.Count <= 0 || (currentTask != null && currentTask.Status == TaskStatus.Running))
                return;

            // ȡ������
            currentTask = tasks.Dequeue();

            // ִ������
            taskRunner.Add(currentTask);
        });
        autoRunBtn.onClick.AddListener(() =>
        {
            // ���û�д���������߶���û������
            if (tasks == null || tasks.Count <= 0)
                return;

            // �����ǰû��������ߵ�ǰ�����ѽ�������ȡ����һ������
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
        int tmpValue;
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
                            // ���Ԫ�ر��
                            infoItems[index1].SetHightlight();
                            infoItems[index2].SetHightlight();
                            codeObjs[0].SetActive(true);
                            codeObjs[1].SetActive(infoItems[index1].Value > infoItems[index2].Value);

                            if (infoItems[index1].Value > infoItems[index2].Value)
                            {
                                tmpRoot = infoItems[index1].transform.parent;

                                // ���������
                                infoItems[index1].transform.SetParent(infoItems[index2].transform.parent);
                                infoItems[index2].transform.SetParent(tmpRoot);

                                // ����λ�ö���
                                infoItems[index1].transform.DOLocalMoveX(0, taskDuration - AnimDelayDuration - 0.1f).SetDelay(AnimDelayDuration);
                                infoItems[index2].transform.DOLocalMoveX(0, taskDuration - AnimDelayDuration - 0.1f).SetDelay(AnimDelayDuration);

                                // ��������
                                tmpIT = infoItems[index1];
                                infoItems[index1] = infoItems[index2];
                                infoItems[index2] = tmpIT;
                            }
                        };
                        task.onStop = t =>
                        {
                            // ���Ԫ�ػ�ԭ
                            infoItems[index1].SetNormal();
                            infoItems[index2].SetNormal();
                            codeObjs[0].SetActive(false);
                            codeObjs[1].SetActive(false);
                        };

                        tasks.Enqueue(task);
                    }
                }

                // ������������
                //Task tmpTask = new Task();
                //tmpTask.onStart = t => codeObjs[0].SetActive(false);
                //tasks.Enqueue(tmpTask);
                break;
            case SortType.SelectSort:
                for (int i = 0, length = infoItems.Count; i < length; i++)
                {
                    int minIndex = i;
                    int index1 = i;

                    // Ѱ����Сֵλ��
                    for (int j = i + 1; j < length; j++)
                    {
                        int index2 = j;
                        int index3 = minIndex;
                        Task task2 = new Task(taskDuration * 0.5f);

                        if (infoItemValues[index3] > infoItemValues[index2])
                        {
                            minIndex = index2;
                        }
                        task2.onStart = t =>
                        {
                            // ���Ԫ�ر��
                            infoItems[index2].SetHightlight();
                            infoItems[index3].SetHightlight();
                            codeObjs[0].SetActive(true);
                        };
                        task2.onStop = t =>
                        {
                            // ���Ԫ�ػ�ԭ
                            infoItems[index2].SetNormal();
                            infoItems[index3].SetNormal();
                            codeObjs[0].SetActive(false);
                        };

                        tasks.Enqueue(task2);
                    }

                    if (minIndex != index1)
                    {
                        // ����λ��
                        Task task1 = new Task(taskDuration);

                        tmpValue = infoItemValues[minIndex];
                        infoItemValues[minIndex] = infoItemValues[i];
                        infoItemValues[i] = tmpValue;
                        task1.onStart = t =>
                        {
                            // ���Ԫ�ر��
                            infoItems[index1].SetHightlight();
                            infoItems[minIndex].SetHightlight();
                            codeObjs[1].SetActive(true);

                            tmpRoot = infoItems[index1].transform.parent;

                            // ���������
                            infoItems[index1].transform.SetParent(infoItems[minIndex].transform.parent);
                            infoItems[minIndex].transform.SetParent(tmpRoot);

                            // ����λ�ö���
                            infoItems[index1].transform.DOLocalMoveX(0, taskDuration - AnimDelayDuration - 0.1f).SetDelay(AnimDelayDuration);
                            infoItems[minIndex].transform.DOLocalMoveX(0, taskDuration - AnimDelayDuration - 0.1f).SetDelay(AnimDelayDuration);

                            // ��������
                            tmpIT = infoItems[index1];
                            infoItems[index1] = infoItems[minIndex];
                            infoItems[minIndex] = tmpIT;
                        };
                        task1.onStop = t =>
                        {
                            // ���Ԫ�ػ�ԭ
                            infoItems[index1].SetNormal();
                            infoItems[minIndex].SetNormal();
                            codeObjs[1].SetActive(false);
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