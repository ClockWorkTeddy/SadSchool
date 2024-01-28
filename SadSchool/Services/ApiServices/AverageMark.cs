namespace SadSchool.Services.ApiServices
{
    public class AverageMark
    {
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public double MarkValue { get; set; }

        public override string ToString()
        {
            if (MarkValue == Double.NaN)
                return base.ToString();
            else
                return String.Format("{0:N2}", MarkValue);
        }
    }
}
