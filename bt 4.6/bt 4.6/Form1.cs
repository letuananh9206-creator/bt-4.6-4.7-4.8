using System.ComponentModel;

namespace bt_4._6
{
    public partial class Form1 : Form
    {
        private readonly BindingList<Student> students = new();
        private readonly BindingSource bindingSource = new();
        private readonly DataGridView studentGrid = new();
        private readonly TextBox studentIdTextBox = new();
        private readonly TextBox fullNameTextBox = new();
        private readonly TextBox ageTextBox = new();
        private readonly ComboBox genderComboBox = new();
        private readonly PictureBox avatarPictureBox = new();
        private readonly Button chooseAvatarButton = new();
        private readonly Button addButton = new();
        private readonly Button deleteButton = new();
        private readonly Button updateButton = new();
        private readonly Button exportButton = new();
        private Image? displayedAvatar;

        public Form1()
        {
            InitializeComponent();
            components!.Add(bindingSource);
            Text = "Quản lý sinh viên";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1180, 760);
            MinimumSize = new Size(980, 650);
            BackColor = Color.FromArgb(243, 246, 250);
            Font = new Font("Segoe UI", 9F);

            BuildInterface();
            ConfigureBindings();
        }

        private void BuildInterface()
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(16)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 78));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66));

            var header = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 64, 111),
                Padding = new Padding(20, 10, 20, 8)
            };
            header.Controls.Add(new Label
            {
                Text = "QUẢN LÝ SINH VIÊN",
                Dock = DockStyle.Top,
                Height = 32,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold)
            });
            header.Controls.Add(new Label
            {
                Text = "Quản lý hồ sơ, ảnh đại diện và xuất danh sách",
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(220, 231, 244),
                Font = new Font("Segoe UI", 9F)
            });

            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                Size = new Size(1100, 560),
                SplitterDistance = 660,
                Panel1MinSize = 430,
                Panel2MinSize = 340,
                BackColor = Color.FromArgb(243, 246, 250)
            };

            var listPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(16)
            };
            var listTitle = new Label
            {
                Text = "DANH SÁCH SINH VIÊN",
                Dock = DockStyle.Top,
                Height = 38,
                ForeColor = Color.FromArgb(30, 64, 111),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            ConfigureGrid();
            listPanel.Controls.Add(studentGrid);
            listPanel.Controls.Add(listTitle);
            splitContainer.Panel1.Controls.Add(listPanel);

            var editorPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(18)
            };
            var editorLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6
            };
            editorLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
            editorLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
            editorLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
            editorLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
            editorLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
            editorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            editorLayout.Controls.Add(new Label
            {
                Text = "THÔNG TIN SINH VIÊN",
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(30, 64, 111),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);
            AddEditorField(editorLayout, 1, "Mã sinh viên", studentIdTextBox);
            AddEditorField(editorLayout, 2, "Họ và tên", fullNameTextBox);
            AddEditorField(editorLayout, 3, "Tuổi", ageTextBox);
            AddEditorField(editorLayout, 4, "Giới tính", genderComboBox);

            var avatarPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(0, 8, 0, 0)
            };
            avatarPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            avatarPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            avatarPictureBox.Dock = DockStyle.Fill;
            avatarPictureBox.BackColor = Color.FromArgb(246, 248, 251);
            avatarPictureBox.BorderStyle = BorderStyle.FixedSingle;
            avatarPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            avatarPictureBox.Margin = new Padding(2, 2, 2, 10);
            chooseAvatarButton.Text = "Chọn ảnh đại diện...";
            chooseAvatarButton.Dock = DockStyle.Fill;
            StyleButton(chooseAvatarButton, Color.FromArgb(229, 238, 249), Color.FromArgb(30, 64, 111));
            avatarPanel.Controls.Add(avatarPictureBox, 0, 0);
            avatarPanel.Controls.Add(chooseAvatarButton, 0, 1);
            editorLayout.Controls.Add(avatarPanel, 0, 5);
            editorPanel.Controls.Add(editorLayout);
            splitContainer.Panel2.Controls.Add(editorPanel);

            var actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(0, 10, 0, 0)
            };
            exportButton.Text = "Xuất CSV";
            updateButton.Text = "Cập nhật";
            deleteButton.Text = "Xóa sinh viên";
            addButton.Text = "+ Thêm sinh viên";
            StyleButton(exportButton, Color.FromArgb(30, 64, 111), Color.White);
            StyleButton(updateButton, Color.FromArgb(37, 132, 91), Color.White);
            StyleButton(deleteButton, Color.FromArgb(252, 235, 235), Color.FromArgb(176, 53, 53));
            StyleButton(addButton, Color.FromArgb(229, 238, 249), Color.FromArgb(30, 64, 111));
            actions.Controls.Add(exportButton);
            actions.Controls.Add(updateButton);
            actions.Controls.Add(deleteButton);
            actions.Controls.Add(addButton);

            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(splitContainer, 0, 1);
            layout.Controls.Add(actions, 0, 2);
            Controls.Add(layout);
        }

        private void ConfigureGrid()
        {
            studentGrid.Dock = DockStyle.Fill;
            studentGrid.BackgroundColor = Color.White;
            studentGrid.BorderStyle = BorderStyle.None;
            studentGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            studentGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            studentGrid.ColumnHeadersHeight = 42;
            studentGrid.EnableHeadersVisualStyles = false;
            studentGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(236, 241, 247);
            studentGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(44, 57, 73);
            studentGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            studentGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 234, 250);
            studentGrid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 46, 66);
            studentGrid.DefaultCellStyle.Padding = new Padding(5, 3, 5, 3);
            studentGrid.RowTemplate.Height = 36;
            studentGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            studentGrid.AllowUserToAddRows = false;
            studentGrid.AllowUserToDeleteRows = false;
            studentGrid.AutoGenerateColumns = false;
            studentGrid.ReadOnly = true;
            studentGrid.MultiSelect = false;
            studentGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            studentGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Student.StudentId),
                HeaderText = "Mã SV",
                FillWeight = 75
            });
            studentGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Student.FullName),
                HeaderText = "Họ và tên",
                FillWeight = 145
            });
            studentGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Student.Age),
                HeaderText = "Tuổi",
                FillWeight = 50
            });
            studentGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Student.Gender),
                HeaderText = "Giới tính",
                FillWeight = 85
            });
        }

        private static void AddEditorField(TableLayoutPanel layout, int row, string caption, Control editor)
        {
            var fieldPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0, 2, 0, 2)
            };
            fieldPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
            fieldPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            fieldPanel.Controls.Add(new Label
            {
                Text = caption,
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(86, 101, 119),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);

            editor.Dock = DockStyle.Fill;
            editor.Margin = new Padding(0, 1, 0, 2);
            if (editor is TextBox textBox)
            {
                textBox.BorderStyle = BorderStyle.FixedSingle;
                textBox.Font = new Font("Segoe UI", 10F);
            }
            else if (editor is ComboBox comboBox)
            {
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            fieldPanel.Controls.Add(editor, 0, 1);
            layout.Controls.Add(fieldPanel, 0, row);
        }

        private static void StyleButton(Button button, Color backColor, Color foreColor)
        {
            button.AutoSize = false;
            button.Size = new Size(142, 42);
            button.Margin = new Padding(7, 0, 0, 0);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
        }

        private void ConfigureBindings()
        {
            genderComboBox.Items.AddRange(["Nam", "Nữ"]);
            bindingSource.DataSource = students;
            studentGrid.DataSource = bindingSource;

            studentIdTextBox.DataBindings.Add(nameof(TextBox.Text), bindingSource, nameof(Student.StudentId), true, DataSourceUpdateMode.OnPropertyChanged);
            fullNameTextBox.DataBindings.Add(nameof(TextBox.Text), bindingSource, nameof(Student.FullName), true, DataSourceUpdateMode.OnPropertyChanged);

            var ageBinding = new Binding(nameof(TextBox.Text), bindingSource, nameof(Student.Age), true, DataSourceUpdateMode.OnPropertyChanged);
            ageBinding.Parse += (_, e) => e.Value = int.TryParse(e.Value as string, out var age) ? age : 0;
            ageTextBox.DataBindings.Add(ageBinding);
            ageTextBox.KeyPress += (_, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            genderComboBox.DataBindings.Add(nameof(ComboBox.SelectedItem), bindingSource, nameof(Student.Gender), true, DataSourceUpdateMode.OnPropertyChanged);

            var avatarBinding = new Binding(nameof(PictureBox.Image), bindingSource, nameof(Student.AvatarPath), true, DataSourceUpdateMode.Never);
            avatarBinding.Format += FormatAvatar;
            avatarPictureBox.DataBindings.Add(avatarBinding);

            addButton.Click += (_, _) => AddStudent();
            deleteButton.Click += (_, _) => DeleteStudent();
            updateButton.Click += (_, _) => UpdateStudent();
            exportButton.Click += (_, _) => ExportCsv();
            chooseAvatarButton.Click += (_, _) => ChooseAvatar();
            FormClosed += (_, _) => displayedAvatar?.Dispose();
        }

        private void AddStudent()
        {
            var student = new Student();
            students.Add(student);
            bindingSource.Position = students.Count - 1;
            studentIdTextBox.Focus();
        }

        private void DeleteStudent()
        {
            if (bindingSource.Current is not Student student)
            {
                return;
            }

            var result = MessageBox.Show(this,
                $"Bạn có chắc muốn xóa sinh viên {student.FullName}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                bindingSource.RemoveCurrent();
            }
        }

        private void UpdateStudent()
        {
            ValidateChildren();
            bindingSource.EndEdit();

            if (bindingSource.Current is not Student student)
            {
                MessageBox.Show(this, "Vui lòng chọn hoặc thêm sinh viên trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var studentId = student.StudentId.Trim();
            var fullName = student.FullName.Trim();
            if (studentId.Length == 0 || fullName.Length == 0 || student.Age <= 0)
            {
                MessageBox.Show(this, "Mã sinh viên, họ tên và tuổi lớn hơn 0 là bắt buộc.", "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (students.Any(item => !ReferenceEquals(item, student)
                && string.Equals(item.StudentId.Trim(), studentId, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(this, "Mã sinh viên đã tồn tại.", "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                studentIdTextBox.Focus();
                return;
            }

            student.StudentId = studentId;
            student.FullName = fullName;
            bindingSource.ResetCurrentItem();
            MessageBox.Show(this, "Thông tin sinh viên đã được cập nhật.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ChooseAvatar()
        {
            if (bindingSource.Current is not Student student)
            {
                MessageBox.Show(this, "Vui lòng chọn hoặc thêm sinh viên trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialog = new OpenFileDialog
            {
                Title = "Chọn ảnh đại diện",
                Filter = "Tệp hình ảnh|*.jpg;*.jpeg;*.png;*.bmp|Tất cả các tệp|*.*",
                CheckFileExists = true,
                Multiselect = false
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            using var image = LoadAvatarImage(dialog.FileName);
            if (image is null)
            {
                MessageBox.Show(this, "Không thể đọc tệp hình ảnh đã chọn.", "Ảnh không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            student.AvatarPath = dialog.FileName;
            bindingSource.ResetCurrentItem();
        }

        private void FormatAvatar(object? sender, ConvertEventArgs e)
        {
            var image = LoadAvatarImage(e.Value as string);
            var previousImage = displayedAvatar;
            displayedAvatar = image;
            e.Value = image;
            if (previousImage is not null && !ReferenceEquals(previousImage, image))
            {
                previousImage.Dispose();
            }
        }

        private static Image? LoadAvatarImage(string? path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return null;
            }

            try
            {
                using var source = Image.FromFile(path);
                return new Bitmap(source);
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (OutOfMemoryException)
            {
                return null;
            }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                return null;
            }
        }

        private void ExportCsv()
        {
            ValidateChildren();
            bindingSource.EndEdit();

            using var dialog = new SaveFileDialog
            {
                Title = "Xuất danh sách sinh viên",
                Filter = "Tệp CSV (*.csv)|*.csv",
                DefaultExt = "csv",
                AddExtension = true,
                OverwritePrompt = true,
                FileName = "DanhSachSinhVien.csv"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                using var writer = new StreamWriter(dialog.FileName, false, new System.Text.UTF8Encoding(true));
                writer.WriteLine("StudentId,FullName,Age,Gender,AvatarPath");
                foreach (var student in students)
                {
                    writer.WriteLine(string.Join(",",
                        EscapeCsv(student.StudentId),
                        EscapeCsv(student.FullName),
                        EscapeCsv(student.Age.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                        EscapeCsv(student.Gender),
                        EscapeCsv(student.AvatarPath)));
                }

                MessageBox.Show(this, "Đã xuất danh sách sinh viên.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException exception)
            {
                MessageBox.Show(this, $"Không thể ghi tệp CSV: {exception.Message}", "Lỗi xuất tệp", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show(this, $"Không có quyền ghi tệp CSV: {exception.Message}", "Lỗi xuất tệp", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string EscapeCsv(string? value) => $"\"{(value ?? string.Empty).Replace("\"", "\"\"")}\"";
    }
}
