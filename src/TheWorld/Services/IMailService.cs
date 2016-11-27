using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    public interface IMailService
    {
        void SendEmail(string to, string from, string subject,string message);
    }
}
