namespace SleekFlow.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
        public FieldAttribute(string exactName)
        {
            ExactName = exactName;
        }

        public string ExactName { get; private set; }
    }
}
