using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlcApi.Entities;
using PlcApi.Exceptions;
using S7.Net;

namespace PlcApi.Services
{
    public class PlcCommunicationService :IPlcCommunicationService
    {
        public Dictionary<int,Plc> Plcs = new Dictionary<int, Plc>();

        public void AddPlc(int plcId, string ip, PlcModel model)
        {
            if (!Plcs.TryGetValue(plcId, out _))
                DeletePlc(plcId);

            Plc plc = new Plc(model.Cpu,ip,model.Rack,model.Slot);
            if(plc == null) 
            {
                throw new MyPlcException("Plc creation failed. Wrong Model or Ip.");
            }
            Plcs.Add(plcId, plc);
        }
        
        public void StartCommunication(int plcId)
        {
            if(Plcs.TryGetValue(plcId, out Plc plc))
            {
                plc.Open();
            }
            else
                throw new NotFoundException("Communication failed. Plc not found.");    //TU POPRAW XDDD
        }

        public Plc GetPlc(int plcId)
        {
            if(Plcs.TryGetValue(plcId, out Plc plc))
                return plc;
            throw new NotFoundException("Plc not found.");
        }

        public void StopCommunication(int plcId)
        {
            if (Plcs.TryGetValue(plcId, out Plc plc))
            {
                if (plc.IsConnected)
                    plc.Close();
            }
        }

        public void DeletePlc(int plcId)
        {
           if (Plcs.TryGetValue(plcId, out _))
            {
                StopCommunication(plcId);
                Plcs.Remove(plcId);
            }
        }
    }
}
