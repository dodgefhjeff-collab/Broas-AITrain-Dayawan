using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace WaterStation.Client
{
    /// <summary>
    /// Modbus TCP 从站服务
    /// </summary>
    public class AIMonitorTcp : IDisposable
    {
        private readonly object _syncRoot = new object();
        private readonly ModbusSlaveRegisterService _registerService;
        private readonly AIMonitor _encoder = new AIMonitor();

        private TcpListener _listener;
        private CancellationTokenSource _cts;
        private Task _acceptTask;
        private int _port;
        private int _unitId;

        public bool IsRunning { get; private set; }
        public int Port => _port;
        public int UnitId => _unitId;

        public AIMonitorTcp(ModbusSlaveRegisterService registerService)
        {
            _registerService = registerService ?? throw new ArgumentNullException(nameof(registerService));
        }

        public bool Start(int port, int unitId)
        {
            lock (_syncRoot)
            {
                if (IsRunning)
                    return true;

                try
                {
                    _port = port;
                    _unitId = unitId;
                    _cts = new CancellationTokenSource();
                    _listener = new TcpListener(IPAddress.Any, port);
                    _listener.Start();
                    IsRunning = true;
                    _acceptTask = Task.Run(() => AcceptLoopAsync(_cts.Token));
                    Global.Logger.Info($"Modbus TCP 从站已启动，端口 {port}，站号 {unitId}");
                    return true;
                }
                catch (Exception ex)
                {
                    Global.Logger.Error(ex, $"Modbus TCP 从站启动失败，端口 {port}");
                    StopInternal();
                    return false;
                }
            }
        }

        public void Stop()
        {
            lock (_syncRoot)
            {
                StopInternal();
            }
        }

        private void StopInternal()
        {
            try
            {
                _cts?.Cancel();
            }
            catch (Exception ex)
            {
                Global.Logger.Warn(ex, "取消 Modbus TCP 接受循环时发生异常");
            }

            try
            {
                _listener?.Stop();
            }
            catch (Exception ex)
            {
                Global.Logger.Warn(ex, "停止 Modbus TCP 监听器时发生异常");
            }

            IsRunning = false;
            _listener = null;
            _cts?.Dispose();
            _cts = null;
            Global.Logger.Info("Modbus TCP 从站已停止");
        }

        private async Task AcceptLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync(token).ConfigureAwait(false);
                    _ = Task.Run(() => HandleClientAsync(client, token), token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    if (!token.IsCancellationRequested)
                        Global.Logger.Warn(ex, "Modbus TCP 接受客户端连接时发生异常");
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            string remote = client.Client.RemoteEndPoint?.ToString() ?? "unknown";
            try
            {
                using (client)
                using (NetworkStream stream = client.GetStream())
                {
                    while (!token.IsCancellationRequested && client.Connected)
                    {
                        byte[] request = await ReadFullRequestAsync(stream, token).ConfigureAwait(false);
                        if (request == null)
                            break;

                        byte[] response = BuildTcpResponse(request);
                        if (response != null)
                            await stream.WriteAsync(response, 0, response.Length, token).ConfigureAwait(false);
                    }
                }
            }
            catch (IOException)
            {
                // 客户端断开连接
            }
            catch (OperationCanceledException)
            {
                // 服务停止
            }
            catch (Exception ex)
            {
                Global.Logger.Warn(ex, $"Modbus TCP 处理客户端 {remote} 时发生异常");
            }
        }

        private static async Task<byte[]> ReadFullRequestAsync(NetworkStream stream, CancellationToken token)
        {
            byte[] header = await ReadExactAsync(stream, 6, token).ConfigureAwait(false);
            if (header == null)
                return null;

            int length = (header[4] << 8) | header[5];
            if (length <= 0 || length > 260)
                return null;

            byte[] body = await ReadExactAsync(stream, length, token).ConfigureAwait(false);
            if (body == null)
                return null;

            byte[] request = new byte[6 + length];
            Array.Copy(header, request, 6);
            Array.Copy(body, 0, request, 6, length);
            return request;
        }

        private static async Task<byte[]> ReadExactAsync(NetworkStream stream, int count, CancellationToken token)
        {
            byte[] buffer = new byte[count];
            int offset = 0;
            while (offset < count)
            {
                int read = await stream.ReadAsync(buffer, offset, count - offset, token).ConfigureAwait(false);
                if (read == 0)
                    return null;
                offset += read;
            }
            return buffer;
        }

        private byte[] BuildTcpResponse(byte[] request)
        {
            if (request.Length < 12)
                return null;

            ushort transactionId = (ushort)((request[0] << 8) | request[1]);
            ushort protocolId = (ushort)((request[2] << 8) | request[3]);
            if (protocolId != 0)
                return null;

            byte unitId = request[6];
            if (unitId != _unitId)
                return null;

            byte functionCode = request[7];
            ushort startAddress = _encoder.Convert16ToUInt16(new byte[] { request[8], request[9] });
            ushort count = _encoder.Convert16ToUInt16(new byte[] { request[10], request[11] });

            byte errorType = _registerService.ValidateFc04Request(functionCode, startAddress, count);
            byte[] pdu;
            if (errorType != 0)
            {
                pdu = ModbusSlaveRegisterService.BuildFc04ExceptionResponse(unitId, errorType);
            }
            else
            {
                _registerService.RefreshFromGlobal(_encoder);
                pdu = _registerService.BuildFc04Response(unitId, startAddress, count);
            }

            byte[] response = new byte[6 + pdu.Length];
            response[0] = (byte)(transactionId >> 8);
            response[1] = (byte)transactionId;
            response[2] = 0;
            response[3] = 0;
            response[4] = (byte)(pdu.Length >> 8);
            response[5] = (byte)pdu.Length;
            Array.Copy(pdu, 0, response, 6, pdu.Length);
            return response;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
