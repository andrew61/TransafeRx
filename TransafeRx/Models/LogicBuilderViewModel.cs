using System.Web.Mvc;

namespace TransafeRx.Models
{
    public class LogicBuilderViewModel
    {
        public string Expression { get; set; }
        public SelectList ItemTypeSelectList { get; set; }
        public int ConditionId { get; set; }

        public int ConditionValue { get; set; }
    }
}