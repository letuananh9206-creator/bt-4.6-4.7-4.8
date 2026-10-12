namespace bt_4._8
{
    public sealed class StudentForm : Form
    {
        private readonly Button loadButton;
        private readonly ProgressBar progressBar;
        private readonly Label percentageLabel;
        private readonly DataGridView studentsGrid;
        private readonly StudentDataService dataService = new();

        public StudentForm()
        {
            Text = "Danh sách sinh viên";
            Width = 800;
            Height = 500;
            MinimumSize = new Size(600, 350);
            StartPosition = FormStartPosition.CenterParent;

            loadButton = new Button
            {
                AutoSize = true,
                Text = "Nạp dữ liệu sinh viên"
            };
            loadButton.Click += async (_, _) => await LoadStudentsAsync();

            progressBar = new ProgressBar
            {
                Minimum = 0,
                Maximum = 100,
                Width = 300,
                Height = 24,
                Style = ProgressBarStyle.Continuous
            };

            percentageLabel = new Label
            {
                AutoSize = true,
                Text = "0%",
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(0, 5, 0, 0)
            };

            var controlsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(8),
                WrapContents = false
            };
            controlsPanel.Controls.Add(loadButton);
            controlsPanel.Controls.Add(progressBar);
            controlsPanel.Controls.Add(percentageLabel);

            studentsGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            Controls.Add(studentsGrid);
            Controls.Add(controlsPanel);
        }

        private async Task LoadStudentsAsync()
        {
            loadButton.Enabled = false;
            progressBar.Value = 0;
            percentageLabel.Text = "0%";
            studentsGrid.DataSource = null;

            if (MdiParent is Form1 parent)
            {
                parent.SetLoadingStatus("Đang tải dữ liệu: 0%");
            }

            try
            {
                IProgress<int> progress = new Progress<int>(percentage =>
                {
                    progressBar.Value = percentage;
                    percentageLabel.Text = $"{percentage}%";
                    if (MdiParent is Form1 mdiParent)
                    {
                        mdiParent.UpdateLoadingProgress(percentage);
                    }
                });

                var students = await dataService.LoadStudentsAsync(progress);
                studentsGrid.DataSource = students;
                if (MdiParent is Form1 mdiParent)
                {
                    mdiParent.SetLoadingStatus($"Tải hoàn tất: {students.Count} sinh viên");
                }

                MessageBox.Show(this, $"Đã tải thành công {students.Count} sinh viên.", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                if (MdiParent is Form1 mdiParent)
                {
                    mdiParent.SetLoadingStatus("Tải dữ liệu thất bại");
                }

                MessageBox.Show(this, $"Không thể tải dữ liệu sinh viên: {exception.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loadButton.Enabled = true;
            }
        }
    }
}
