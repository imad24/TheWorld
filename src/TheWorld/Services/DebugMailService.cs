using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    public class DebugMailService :IMailService
    {
        public void SendEmail(string to, string @from, string subject, string message)
        {
           Debug.WriteLine($"Message sent from: {from} to:{to} subject:{subject}");
        }
    }
}
