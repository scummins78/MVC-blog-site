using System.ComponentModel;

namespace ValueInjecterExtensions.SmartConvention
{
    public class SmartMatchInfo
    {
        public PropertyDescriptor SourceProp { get; set; }
        public PropertyDescriptor TargetProp { get; set; }
        public object Source { get; set; }
        public object Target { get; set; }
    }
}
