using System;
using System.Collections.Generic;
using System.Text;

namespace Quokka.RISCV.Integration.DTO
{
    public class BaseRequest
    {

    }

    public class BaseResponse
    {

    }

    public class InvokeRequest : BaseRequest
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
        public FSSnapshot Source { get; set; } = new FSSnapshot();
        public List<ToolchainOperation> Operations { get; set; } = new List<ToolchainOperation>();
        public List<FileRule> ResultRules { get; set; } = new List<FileRule>();
        public ExtensionClasses ExtensionClasses { get; set; } = new ExtensionClasses();
    }

    public class InvokeResponse : BaseResponse
    {
        public Guid CorrelationId { get; set; }

        public FSSnapshot Result { get; set; } = new FSSnapshot();
    }
}
