namespace CrudSamplesTwo.Dtos
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }

        private decimal? salary;
        public decimal? Salary
        {
            get => salary;
            set
            {
                if (value <= 0) throw new ArgumentException("Salary cannot be negative.");
                salary = value;
            }
        }
    }
}
