using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IRRemoteControlServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class IRRemoteController : ControllerBase
    {
        private TcpClient TcpClient { get; } = new TcpClient();
        private object Locker { get; } = new object();
        private IConfiguration Configuration { get; }

        public IRRemoteController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet("Control")]
        public async Task<IActionResult> Control(string operations)
        {
            byte sendStr;
            switch (operations)
            {
                case "tv_open":
                    sendStr = 49;
                    break;
                case "tv_change_input":
                    sendStr = 50;
                    break;
                case "tv_enter":
                    sendStr = 57;
                    break;
                case "speaker_open":
                    sendStr = 51;
                    break;
                case "volume_up":
                    sendStr = 52;
                    break;
                case "volume_down":
                    sendStr = 53;
                    break;
                case "switch_1":
                    sendStr = 54;
                    break;
                case "switch_2":
                    sendStr = 55;
                    break;
                case "switch_3":
                    sendStr = 56;
                    break;
                default:
                    return BadRequest(new { Error = "Invalid operations" });
            }

            if (!SendTCP(sendStr))
            {
                await Task.Delay(1000);
                if (!SendTCP(sendStr))
                {
                    return BadRequest(new { Error = "Operation send timeout" });
                }
            }

            return Ok();
        }

        private bool SendTCP(byte str)
        {
            TcpClient.SendTimeout = 1000;
            TcpClient.ReceiveTimeout = 1000;
            lock (Locker)
            {
                try
                {
                    var a = Configuration["BoardIP"];
                    if (!TcpClient.Connected)
                        if (!TcpClient.ConnectAsync(Configuration["BoardIP"], int.Parse(Configuration["BoardPort"]))
                            .Wait(1000))
                            throw new TimeoutException();
                    var stream = TcpClient.GetStream();
                    stream.WriteByte(str);
                    stream.WriteByte(10); // '\n'
                    return true;
                }
                catch
                {
                    if (TcpClient.Connected)
                        TcpClient.Close();
                }
            }

            return false;
        }
    }
}