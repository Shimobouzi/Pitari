
using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Globalization;

public class PicoSensorHub : MonoBehaviour
{
    /* ────── Inspector ────── */
    [Header("Serial")] public string portName = "COM5";
    public int baudRate = 9600;

    /* ────── 公開データ ────── */
    public Vector3 mpu1Acc, mpu1Gyro;
    public Vector3 mpu2Acc, mpu2Gyro;
    public Vector3 leftAcc, rightAcc;
    public Vector3 AvgGyro => (mpu1Gyro + mpu2Gyro) * 0.5f;


    /* スナップショット */
    public MpuFrame latest { get; private set; }
    public event Action<MpuFrame> OnFrame;

    /* ────── 内部 ────── */
    SerialPort sp; Thread rxThread; volatile bool running;
    readonly ConcurrentQueue<string> q = new();

    static readonly Regex accR = new(
        @"^\[MPU(?<id>[12])\]\s+GYR\s+X=(?<x>[-+]?\d*\.?\d+)\s+Y=(?<y>[-+]?\d*\.?\d+)\s+Z=(?<z>[-+]?\d*\.?\d+)",
        RegexOptions.Compiled);

    static readonly Regex gyrR = new(
    @"^\[MPU(?<id>[12])\]\s+ACC\s+X=(?<x>[-+]?\d*\.?\d+)\s+Y=(?<y>[-+]?\d*\.?\d+)\s+Z=(?<z>[-+]?\d*\.?\d+)",
    RegexOptions.Compiled);

    //static readonly Regex accR = new(
    //@"^\[MPU(?<id>[12])\]\s+ACC\s+X=(?<x>[-+]?\d*\.?\d+)\s+Y=(?<y>[-+]?\d*\.?\d+)\s+Z=(?<z>[-+]?\d*\.?\d+)",
    //RegexOptions.Compiled);
    //static readonly Regex gyrR = new(
    //    @"^\[MPU(?<id>[12])\]\s+GYR\s+X=(?<x>[-+]?\d*\.?\d+)\s+Y=(?<y>[-+]?\d*\.?\d+)\s+Z=(?<z>[-+]?\d*\.?\d+)",
    //    RegexOptions.Compiled);

    readonly bool[,] got = new bool[2, 2];   // [id-1, 0=本当のGYR 1=本当のACC]
    readonly Vector3[,] tmp = new Vector3[2, 2];

    /* ───────────────────────── */

    void Start()
    {
        sp = new SerialPort(portName, baudRate) { NewLine = "\n", ReadTimeout = 200, DtrEnable = true, RtsEnable = true };
        sp.Open();
        running = true;
        rxThread = new Thread(ReadLoop) { IsBackground = true };
        rxThread.Start();
    }

    void ReadLoop()
    {
        while (running)
        {
            try { q.Enqueue(sp.ReadLine().TrimEnd('\r', '\n')); }
            catch (TimeoutException) { }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
        }
    }

    void Update()
    {
        while (q.TryDequeue(out string line))
        {
            //ACC 行を kind=1、GYR 行を kind=0 に逆転 
            if (Parse(line, accR, 1) || Parse(line, gyrR, 0))
            {
                if (got[0, 0] && got[0, 1] && got[1, 0] && got[1, 1])
                {
                    //配列インデックスも左右入れ替え
                    mpu1Acc = tmp[0, 1]; mpu1Gyro = tmp[0, 0];
                    mpu2Acc = tmp[1, 1]; mpu2Gyro = tmp[1, 0];
                    leftAcc = mpu1Acc;
                    rightAcc = mpu2Acc;

                    latest = new MpuFrame
                    {
                        timestamp = Time.time,
                        mpu1 = new MpuData(mpu1Acc, mpu1Gyro),
                        mpu2 = new MpuData(mpu2Acc, mpu2Gyro)
                    };
                    OnFrame?.Invoke(latest);
                    Array.Clear(got, 0, got.Length);
                }
            }
        }
    }

    bool Parse(string line, Regex re, int kind)
    {
        var m = re.Match(line);
        if (!m.Success) return false;

        int id = int.Parse(m.Groups["id"].Value, CultureInfo.InvariantCulture) - 1;
        tmp[id, kind] = new Vector3(
            float.Parse(m.Groups["x"].Value, CultureInfo.InvariantCulture),
            float.Parse(m.Groups["y"].Value, CultureInfo.InvariantCulture),
            float.Parse(m.Groups["z"].Value, CultureInfo.InvariantCulture));
        got[id, kind] = true;
        return true;
    }

    void OnDestroy()
    {
        running = false;
        try
        {
            if (rxThread?.IsAlive == true) rxThread.Join();
            if (sp != null && sp.IsOpen) sp.Close();
        }
        catch { }
    }
}


[Serializable]
public struct MpuData
{
    public Vector3 acc, gyro;
    public MpuData(Vector3 a, Vector3 g) { acc = a; gyro = g; }

    public float AccMagnitude => acc.magnitude;
    public float Pitch => Mathf.Atan2(acc.y, Mathf.Sqrt(acc.x * acc.x + acc.z * acc.z)) * Mathf.Rad2Deg;
    public float Roll => Mathf.Atan2(-acc.x, acc.z) * Mathf.Rad2Deg;
}

[Serializable]
public struct MpuFrame
{
    public float timestamp;
    public MpuData mpu1, mpu2;

    public float AvgPitch => (mpu1.Pitch + mpu2.Pitch) * 0.5f;
}
