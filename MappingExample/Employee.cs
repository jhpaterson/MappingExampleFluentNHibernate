
namespace MappingExample
{
    public class Employee 
    {
        protected int employeeID;
        protected string name;
        protected string username;
        protected string phoneNumber;
        protected Address address;

        public virtual int EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        public virtual string Username
        {
            get { return username; }
            set { username = value; }
        }

        public virtual string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        public virtual Address Address
        {
            get { return address; }
            set { address = value; }
        }

        public Employee()
        {
        }

        public Employee(string name,
                        string username,
                        string phoneNumber)
        {
            this.name = name;
            this.username = username;
            this.phoneNumber = phoneNumber;
        }
    }
}
