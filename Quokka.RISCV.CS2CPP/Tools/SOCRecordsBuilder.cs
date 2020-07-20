using Quokka.RISCV.CS2CPP.CodeModels.CPP;
using Quokka.RISCV.Integration.Generator.SOC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.CS2CPP.Tools
{
    public class SOCRecordsBuilder
    {
        public List<SOCRecord> ToSOCRecords(uint seg, IEnumerable<SOCResourceCPPModel> source)
        {
            var socRecords = new List<SOCRecord>();

            foreach (var d in source)
            {
                var segment = d.Address;
                if (segment == 0)
                {
                    segment = seg;
                    seg++;
                }

                var templatesMap = new Dictionary<Type, string>()
                {
                    { typeof(byte), "memory8" },
                    { typeof(sbyte), "memory8" },
                    { typeof(ushort), "memory16" },
                    { typeof(short), "memory16" },
                    { typeof(int), "memory32" },
                    { typeof(uint), "memory32" },
                };

                if (d.Length > 0 && !templatesMap.ContainsKey(d.Type))
                {
                    throw new Exception($"No template found for {d.Type}");
                }

                var template = d.Length > 0 ? templatesMap[d.Type] : "register";

                var rec = new SOCRecord()
                {
                    SegmentBits = 12,
                    SoftwareName = d.Name,
                    HardwareName = d.Name,
                    DataType = d.Type,
                    Depth = (uint)d.Length,
                    Segment = segment,
                    Template = template,
                };

                socRecords.Add(rec);
            }

            return socRecords;
        }
    }
}
