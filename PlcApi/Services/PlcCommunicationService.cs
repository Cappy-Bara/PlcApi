using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Entities;
using S7.Net;

namespace PlcApi.Services
{
    public class PlcCommunicationService :IPlcCommunicationService
    {
        public List<MyPlc> PlcList = new List<MyPlc>();

        public bool AddPlc(int userId, string ip)
        {
            var plc = PlcList.FirstOrDefault(n => n.UserId == userId);
            if (plc != null)
                DeletePlc(userId);

            plc = new MyPlc(ip, userId);
            if(plc != null) 
            { 
                PlcList.Add(plc);
                return true;
            }
            return false;
        }
        
        public bool StartCommunication(int userId)
        {
            var plc = PlcList.FirstOrDefault(n => n.UserId == userId);
            if(plc != null) 
            {
                plc.Plc.Open();
                return plc.Plc.IsConnected;
            }
            return false;
        }

        public Plc GetPlc(int userId)
        {
            return PlcList.FirstOrDefault(n => n.UserId == userId).Plc;
        }

        public bool StopCommunication(int userId)
        {
            var plc = PlcList.FirstOrDefault(n => n.UserId == userId);
            if(plc.Plc.IsConnected)
            {
                plc.Plc.Close();
            }
            return false;
        }

        public bool DeletePlc(int userId)
        {
           var plc = PlcList.FirstOrDefault(n => n.UserId == userId);
           if(plc != null)
            {
                StopCommunication(userId);
                PlcList.Remove(plc);
                return true;
            }
            return false;
        }
    }
}
