using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;

public class Login : MonoBehaviour
{
    public InputField nameInput;
    public InputField passwordInput;
    public InputField codeInput;
    public Text prompt;
    public Request request;

    public DataBase dataBasePf;

    void Start()
    {
        if (DataBase.Instance == null)
            Instantiate(dataBasePf);

        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        prompt.text = "";
        if (nameInput.text == "" || passwordInput.text == "" || codeInput.text == "")
        {
            if (nameInput.text == "")
            {
                prompt.text = "用户名不能为空";
            }
            else if (passwordInput.text == "")
            {
                prompt.text = "密码不能为空";
            }
            else if (codeInput.text == "")
            {
                prompt.text = "选课码不能为空";
            }
            return;
        }
        else
        {
            prompt.text = "";
            // 登录，获得用户信息
            Response<UserEntity> user = request.login(nameInput.text, passwordInput.text);
            if (user.status != 1)
            {
                prompt.text = user.message;
                return;
            }
            // 根据选课码获得课程信息
            Response<SubjectEntity> subject = request.getSubjectByCode(codeInput.text);
            if (subject.status != 1)
            {
                prompt.text = subject.message;
                return;
            }
            // 根据课程id获得其下实验信息列表（数组）
            Response<ExperimentEntity[]> experiments = request.getExperimentsListBySubject(subject.data.id);
            if (experiments.status != 1)
            {
                prompt.text = experiments.message;
                return;
            }

            // 获取实验次数和最高分数
            // Response<GradeAndCountEntity> grade0 = request.getGradeAndCount(user.data.id, experiments.data[0].id);
            // Response<GradeAndCountEntity> grade1 = request.getGradeAndCount(user.data.id, experiments.data[1].id);
            // Response<GradeAndCountEntity> grade2 = request.getGradeAndCount(user.data.id, experiments.data[2].id);
            GradeAndCountEntity[] gradeAndCountList = new GradeAndCountEntity[3];
            for (int i = 0; i < gradeAndCountList.Length; i++)
            {
                Response<GradeAndCountEntity> grade = request.getGradeAndCount(user.data.id, experiments.data[i].id);
                if (grade.status != 1)
                {

                    prompt.text = grade.message;
                    return;
                }
                gradeAndCountList[i] = grade.data;
            }

            PlayerData playerData = new PlayerData();
            ExpData[] expsData = new ExpData[3];
            for (int i = 0; i < expsData.Length; i++)
            {
                expsData[i].data = experiments.data[i];
                expsData[i].expCount = gradeAndCountList[i].count;
                expsData[i].maxScore = gradeAndCountList[i].score;
            }
            playerData.id = user.data.id;
            playerData.expsData = expsData;
            DataBase.Instance.Init(playerData);
            AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
            return;


            // 实验次数+1
            // Response<bool> setGrade = request.addCount(user.data.id, experiments.data[0].id);
            // 获取实验次数和最高分数
            // Response<GradeAndCountEntity> grade2 = request.getGradeAndCount(user.data.id, experiments.data[0].id);
            // Debug.Log(setGrade.data.ToString());
            // Debug.Log("新次数" + grade2.data.count);

            // 提交实验分数
            // Response<bool> setGrade2 = request.setGrade(user.data.id, experiments.data[0].id, 86);
            // 获取实验次数和最高分数
            // Response<GradeAndCountEntity> grade3 = request.getGradeAndCount(user.data.id, experiments.data[0].id);
            // Debug.Log(setGrade2.data.ToString());
            // Debug.Log("新分数" + grade3.data.score);
        };
    }

    // On quit

}
