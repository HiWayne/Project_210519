using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using GameBasic;

// entity 数据实体

// 登录
public class UserEntity
{
    // 用户id
    public int id;
    // 用户名
    public string name;
    // 用户头像
    public string avatar;
    // 用户简介
    public string desc;
    // 创建时间
    public DateTime create_time;
}

// 课程
public class SubjectEntity
{
    // 课程id
    public int id;
    // 课程名
    public string name;
    // 选课码
    public string code;
}

// 实验
public class ExperimentEntity
{
    // 实验id
    public int id;
    // 实验名称
    public string name;
    // 对应课程id
    public int subject;
    // 实验说明
    public string desc;
    // 实验目的
    public string purpose;
    // 实验要求
    public string require;
}

// 成绩、次数
public class GradeAndCountEntity
{
    // id
    public int id;
    // 该信息所属的用户id
    public int user;
    // 该信息所属的实验id
    public int experiment;
    // 已实验次数
    public int count;
    // 最高分数
    public float score;
}

public class Response<T>
{
    public int status;
    public T data;
    public string message;
}

public class Request : MonoBehaviour
{
    private string protocol = "http";
    private string host = "127.0.0.1";
    private string port = "3000";
    private string url = "";
    private bool timeout = false;
    private float time = 3.0f;
    // Start is called before the first frame update 
    // IEnumerator Start()
    // {
    // }

    public Request()
    {
        // 初始化origin，包括：协议、主机地址和端口
        url = protocol + "://" + host + ":" + port;
    }

    // 根据用户名、密码登录
    public Response<UserEntity> login(string name, string password)
    {
        Response<UserEntity> response = post<UserEntity>("/api/user/login", "{\"name\":\"" + name + "\",\"password\":\"" + password + "\"}");
        return response;
    }

    // 根据选课码获取课程信息
    public Response<SubjectEntity> getSubjectByCode(string code)
    {
        Response<SubjectEntity> response = get<SubjectEntity>("/api/subject/find?code=" + code);
        return response;
    }

    // 根据课程id获取实验信息列表
    public Response<ExperimentEntity[]> getExperimentsListBySubject(int subjectId)
    {
        Response<ExperimentEntity[]> response = get<ExperimentEntity[]>("/api/experiments/find/by/subject?id=" + subjectId);
        return response;
    }

    // 根据用户id、实验id，查询某一实验某位用户的成绩、次数相关信息
    public Response<GradeAndCountEntity> getGradeAndCount(int user, int experiment)
    {
        Response<GradeAndCountEntity> response = get<GradeAndCountEntity>("/api/user/grade/find?user=" + user + "&experiment=" + experiment);
        return response;
    }

    // 增加某个用户的某一实验的次数（用户id, 实验id）
    public Response<bool> addCount(int user, int experiment)
    {
        string payloadJson = "{\"user\":" + user + ",\"experiment\":" + experiment + "}";
        Response<bool> response = post<bool>("/api/user/count/add", payloadJson);
        return response;
    }

    // 提交某个用户的某一实验的分数（用户id, 实验id, 当前分数)，后端自己会判断是否是最高分数从而选择是否更新
    public Response<bool> setGrade(int user, int experiment, float score)
    {
        string payloadJson = "{\"user\":" + user + ",\"experiment\":" + experiment + ",\"score\":" + score + "}";
        Response<bool> response = post<bool>("/api/user/grade/set", payloadJson);
        return response;
    }

    // 设置超时取消等待
    private void setTimeout(float time)
    {
        Invoke("changeTimeout", time);
    }

    private void changeTimeout()
    {
        timeout = true;
    }

    // get请求
    private Response<T> get<T>(string path)
    {
        UnityWebRequest request = UnityWebRequest.Get(url + path);
        Debug.Log("get: " + url + path);
        request.SendWebRequest();
        return receiveResponse<T>(request);
    }

    // post请求
    private Response<T> post<T>(string path, string json)
    {
        Debug.Log("post: " + url + path + "  json: " + json);
        UnityWebRequest request = new UnityWebRequest(url + path, "POST");
        byte[] payloadJson = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(payloadJson);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SendWebRequest();
        return receiveResponse<T>(request);
    }

    // 同步阻塞处理返回
    private Response<T> receiveResponse<T>(UnityWebRequest requestInstance)
    {
        timeout = false;
        setTimeout(time);
        while (requestInstance.downloadHandler.text == "" && !timeout && !requestInstance.isNetworkError)
        {
            // wait
        }
        if (timeout)
        {
            Debug.Log("request timeout 3s!");
            Response<T> responseWithTimeout = new Response<T>();
            responseWithTimeout.status = 3;
            responseWithTimeout.data = default(T);
            responseWithTimeout.message = "请求超时";
            return responseWithTimeout;
        }
        if (requestInstance.isNetworkError)
        {
            Debug.Log("request error: " + UnityWebRequest.Result.ConnectionError.ToString() + "!");
            Response<T> responseWithNetworkError = new Response<T>();
            responseWithNetworkError.status = 3;
            responseWithNetworkError.data = default(T);
            responseWithNetworkError.message = UnityWebRequest.Result.ConnectionError.ToString();
            return responseWithNetworkError;
        }
        Debug.Log("Received: " + requestInstance.downloadHandler.text);
        Response<T> response = jsonParse<Response<T>>(requestInstance.downloadHandler.text);
        return response;

    }

    // 解析json
    private T jsonParse<T>(string json) where T : class
    {
        //JsonData table = AnalysisJson.Analy<JsonData>(text);
        T obj = JsonConvert.DeserializeObject<T>(json);
        //  T obj = JsonConvert.DeserializeObject<T>(json);
        return obj;
    }
}