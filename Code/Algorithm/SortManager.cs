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

    // 流程图对象
    public FCGroup[] fCGroups;

    // 代码对象
    public GameObject[] codeObjs;

    // 
    public List<InfoItem> infoItems;
    List<int> infoItemValues;

    // 实验说明
    public Text descText;

    // 执行按钮
    public Button startExpBtn;
    public Button singleLineRunBtn;
    public Button autoRunBtn;
    public float taskDuration;

    // 当前实验信息UI
    public Text expCountText;
    public Text remainExpCountText;
    public Text maxScoreText;

    // 任务相关
    TaskRunner taskRunner;
    Queue<Task> tasks;
    Task currentTask;

    [System.Serializable]
    public struct FCGroup
    {
        public FlowChartElement[] flowChartElements;

        public void SetHightlight()
        {
            for (int i = 0, length = flowChartElements.Length; i < length; i++)
            {
                flowChartElements[i].SetHightlight();
            }
        }

        public void SetNormal()
        {
            for (int i = 0, length = flowChartElements.Length; i < length; i++)
            {
                flowChartElements[i].SetNormal();
            }
        }
    }

    private void Start()
    {
        // 入口(StartMenu)选中的index
        int sortIndex = UIMain.Instance.CurrentSortIndex;
        // 全局数据
        PlayerData currentPlayer = DataBase.Instance.currentPlayer;
        // 设置实验说明
        descText.text = currentPlayer.expsData[sortIndex].data.desc;

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
                            // 相关元素变红
                            infoItems[index1].SetHightlight();
                            infoItems[index2].SetHightlight();
                            fCGroups[0].SetHightlight();
                            codeObjs[0].SetActive(true);
                            codeObjs[1].SetActive(infoItems[index1].Value > infoItems[index2].Value);

                            if (infoItems[index1].Value > infoItems[index2].Value)
                            {
                                tmpRoot = infoItems[index1].transform.parent;

                                fCGroups[1].SetHightlight();

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
                            fCGroups[0].SetNormal();
                            fCGroups[1].SetNormal();
                            codeObjs[0].SetActive(false);
                            codeObjs[1].SetActive(false);
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

                        if (infoItemValues[index3] > infoItemValues[index2])
                        {
                            minIndex = index2;
                        }
                        task2.onStart = t =>
                        {
                            // 相关元素变红
                            infoItems[index2].SetHightlight();
                            infoItems[index3].SetHightlight();
                            fCGroups[0].SetHightlight();
                            codeObjs[0].SetActive(true);
                        };
                        task2.onStop = t =>
                        {
                            // 相关元素还原
                            infoItems[index2].SetNormal();
                            infoItems[index3].SetNormal();
                            fCGroups[0].SetNormal();
                            codeObjs[0].SetActive(false);
                        };

                        tasks.Enqueue(task2);
                    }

                    if (minIndex != index1)
                    {
                        // 交换位置
                        Task task1 = new Task(taskDuration);

                        tmpValue = infoItemValues[minIndex];
                        infoItemValues[minIndex] = infoItemValues[i];
                        infoItemValues[i] = tmpValue;
                        task1.onStart = t =>
                        {
                            // 相关元素变红
                            infoItems[index1].SetHightlight();
                            infoItems[minIndex].SetHightlight();
                            fCGroups[1].SetHightlight();
                            codeObjs[1].SetActive(true);

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
                            fCGroups[1].SetNormal();
                            codeObjs[1].SetActive(false);
                        };

                        tasks.Enqueue(task1);
                    }
                }
                break;
            case SortType.QuickSort:
                CreateQuickSortTask(0, infoItems.Count);

                Task disActObj = new Task();
                disActObj.onStart = t =>
                {
                    for (int i = 0, length = codeObjs.Length; i < length; i++)
                    {
                        codeObjs[i].SetActive(false);
                    }
                };
                tasks.Enqueue(disActObj);
                break;
            default:
                break;
        }
    }

    void CreateQuickSortTask(int startIndex, int elementCount)
    {
        if (startIndex >= elementCount)
            return;

        int i = startIndex;
        int j = elementCount - 1;
        int middleIndex = (startIndex + elementCount) / 2;
        int middle = infoItemValues[middleIndex];

        while (true)
        {
            // 在middle左边找到一个比middle大的值
            while (i < elementCount && infoItemValues[i] < middle)
                i++;
            // 在middle右边找到一个比middle小的值
            while (j > 0 && infoItemValues[j] > middle)
                j--;

            int tmpIndex1 = i;
            int tmpIndex2 = j;
            int tmpMiddleIndex = (startIndex + elementCount) / 2;
            if (tmpIndex1 != tmpMiddleIndex)
            {
                Task task1 = new Task(taskDuration * 0.5f);
                task1.onStart = t =>
                {
                    infoItems[tmpIndex1].SetHightlight();
                    infoItems[tmpMiddleIndex].SetHightlight();
                    fCGroups[0].SetHightlight();

                    codeObjs[0].SetActive(true);
                };
                task1.onStop = t =>
                {
                    infoItems[tmpIndex1].SetNormal();
                    infoItems[tmpMiddleIndex].SetNormal();
                    fCGroups[0].SetNormal();

                    codeObjs[0].SetActive(false);
                };
                tasks.Enqueue(task1);
            }

            if (tmpIndex2 != tmpMiddleIndex)
            {
                Task task2 = new Task(taskDuration * 0.5f);
                task2.onStart = t =>
                {
                    infoItems[tmpIndex2].SetHightlight();
                    infoItems[tmpMiddleIndex].SetHightlight();
                    fCGroups[1].SetHightlight();

                    codeObjs[0].SetActive(true);
                };
                task2.onStop = t =>
                {
                    infoItems[tmpIndex2].SetNormal();
                    infoItems[tmpMiddleIndex].SetNormal();
                    fCGroups[1].SetNormal();

                    codeObjs[0].SetActive(false);
                };
                tasks.Enqueue(task2);
            }

            // 当i=j时,middle左边都是比middle小的数,右边都是比middle大的;跳出循环
            if (i == j)
                break;

            // 交换数据位置
            int tmpValue = infoItemValues[i];
            infoItemValues[i] = infoItemValues[j];
            infoItemValues[j] = tmpValue;

            // 交换位置任务
            Task task3 = new Task(taskDuration);
            Transform tmpRoot;
            InfoItem tmpIT;
            task3.onStart = t =>
            {
                // 标红
                infoItems[tmpIndex1].SetHightlight();
                infoItems[tmpIndex2].SetHightlight();
                codeObjs[1].SetActive(true);
                fCGroups[2].SetHightlight();

                tmpRoot = infoItems[tmpIndex1].transform.parent;

                // 变更父物体
                infoItems[tmpIndex1].transform.SetParent(infoItems[tmpIndex2].transform.parent);
                infoItems[tmpIndex2].transform.SetParent(tmpRoot);

                // 交换位置动画
                infoItems[tmpIndex1].transform.DOLocalMoveX(0, taskDuration - AnimDelayDuration - 0.1f).SetDelay(AnimDelayDuration);
                infoItems[tmpIndex2].transform.DOLocalMoveX(0, taskDuration - AnimDelayDuration - 0.1f).SetDelay(AnimDelayDuration);

                // 交换数据
                tmpIT = infoItems[tmpIndex1];
                infoItems[tmpIndex1] = infoItems[tmpIndex2];
                infoItems[tmpIndex2] = tmpIT;
            };
            task3.onStop = t =>
            {
                // 相关元素还原
                infoItems[tmpIndex1].SetNormal();
                infoItems[tmpIndex2].SetNormal();
                codeObjs[1].SetActive(false);
                fCGroups[2].SetNormal();
            };
            tasks.Enqueue(task3);

            // 递归标红
            Task task4 = new Task(taskDuration * 0.5f);
            task4.onStart = t =>
            {
                codeObjs[2].SetActive(true);
                fCGroups[3].SetHightlight();
            };
            task4.onStop = t =>
            {
                codeObjs[2].SetActive(false);
                fCGroups[3].SetNormal();
            };
            tasks.Enqueue(task4);

            // 如果两个值相等,且等于middle,为避免进入死循环,j--
            if (infoItemValues[i] == infoItemValues[j])
                j--;
        }

        CreateQuickSortTask(startIndex, i);
        CreateQuickSortTask(i + 1, elementCount);
    }

    public void UpdateUI(int expCount, float expMaxScore)
    {
        this.expCountText.text = expCount.ToString();
        this.remainExpCountText.text = (DataBase.ExpMaxCount - expCount).ToString();
        this.maxScoreText.text = string.Format("{0:0}", expMaxScore);
    }
}