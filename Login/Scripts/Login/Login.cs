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
        // for test
        DataBase.Instance.Init(new PlayerData() { expsData = new ExpData[3] });
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
        return;

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
            // 登录
            Response<UserEntity> user = request.login(nameInput.text, passwordInput.text);
            Debug.Log("user: " + user.data.ToString());
            // 根据选课码获得课程
            Response<SubjectEntity> subject = request.getSubjectByCode(codeInput.text);
            Debug.Log("subject: " + subject.data.ToString());
            // 根据课程获得实验信息列表（数组）
            Response<ExperimentEntity[]> experiments = request.getExperimentsList(subject.data.id);
            Debug.Log("实验: " + experiments.data.ToString());

            // 获取实验次数和最高分数

            // Response<GradeAndCountEntity> grade = request.getGradeAndCount(user.data.id, experiments.data[0].id);
            // Debug.Log(grade.data.ToString());
            // Debug.Log("次数" + grade.data.count);
            // Debug.Log("分数" + grade.data.score);

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
            return;
        };
    }

    // On quit

}
