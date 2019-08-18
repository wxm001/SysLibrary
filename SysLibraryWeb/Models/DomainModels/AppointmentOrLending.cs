namespace SysLibraryWeb.Models
{
    using System;

    //学生-书籍中间类
    public class AppointmentOrLending
    {
        public Book Book { get; set; }

        public string BookId { get; set; }

        public StudentInfo Student { get; set; }

        public string StudentId { get; set; }

        public DateTime? AppointingDateTime { get; set; } //区分书籍是借阅书籍还是预约书籍
    }
}