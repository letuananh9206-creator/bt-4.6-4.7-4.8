namespace bt_4._8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            studentsMenuItem.Click += (_, _) => OpenStudentsForm();
        }

        public void UpdateLoadingProgress(int percentage)
        {
            progressStatusLabel.Text = $"Tiến trình tải dữ liệu: {percentage}%";
        }

        public void SetLoadingStatus(string status)
        {
            progressStatusLabel.Text = status;
        }

        private void OpenStudentsForm()
        {
            foreach (Form child in MdiChildren)
            {
                if (child is StudentForm)
                {
                    child.Activate();
                    return;
                }
            }

            var studentsForm = new StudentForm
            {
                MdiParent = this
            };
            studentsForm.Show();
        }
    }
}
