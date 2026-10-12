namespace bt_4._7
{
    public partial class Form1 : Form
    {
        private readonly ToolStripStatusLabel _userStatus = new("Người dùng: Admin");
        private readonly ToolStripStatusLabel _messageStatus = new("Sẵn sàng");
        private readonly ToolStripStatusLabel _timeStatus = new();
        private readonly System.Windows.Forms.Timer _clock = new() { Interval = 1000 };

        public Form1()
        {
            InitializeComponent();
            Text = "Bài 4.7 - Quản lý đa cửa sổ";
            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;

            var menu = CreateMenu();
            var toolbar = CreateToolbar();
            var statusStrip = CreateStatusStrip();
            MainMenuStrip = menu;
            Controls.Add(statusStrip);
            Controls.Add(toolbar);
            Controls.Add(menu);

            _clock.Tick += (_, _) => _timeStatus.Text = DateTime.Now.ToString("HH:mm:ss");
            _clock.Start();
            FormClosed += (_, _) => _clock.Dispose();
        }

        private MenuStrip CreateMenu()
        {
            var menu = new MenuStrip();
            var systemMenu = new ToolStripMenuItem("Hệ thống");
            systemMenu.DropDownItems.Add("Đăng xuất", null, (_, _) => LogOut());
            systemMenu.DropDownItems.Add("Thoát", null, (_, _) => Close());

            var catalogMenu = new ToolStripMenuItem("Danh mục");
            catalogMenu.DropDownItems.Add("Quản lý Sinh viên", null, (_, _) => OpenChild<StudentForm>());
            catalogMenu.DropDownItems.Add("Quản lý Sản phẩm", null, (_, _) => OpenChild<ProductForm>());

            var windowMenu = new ToolStripMenuItem("Cửa sổ");
            windowMenu.DropDownItems.Add("Cascade", null, (_, _) => LayoutMdi(MdiLayout.Cascade));
            windowMenu.DropDownItems.Add("Tile Horizontal", null, (_, _) => LayoutMdi(MdiLayout.TileHorizontal));
            windowMenu.DropDownItems.Add("Tile Vertical", null, (_, _) => LayoutMdi(MdiLayout.TileVertical));
            windowMenu.DropDownItems.Add(new ToolStripSeparator());
            menu.MdiWindowListItem = windowMenu;

            menu.Items.AddRange([systemMenu, catalogMenu, windowMenu]);
            return menu;
        }

        private ToolStrip CreateToolbar()
        {
            var toolbar = new ToolStrip();
            var students = new ToolStripButton("SV | Sinh viên")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                ToolTipText = "Mở quản lý Sinh viên"
            };
            students.Click += (_, _) => OpenChild<StudentForm>();

            var products = new ToolStripButton("SP | Sản phẩm")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                ToolTipText = "Mở quản lý Sản phẩm"
            };
            products.Click += (_, _) => OpenChild<ProductForm>();

            toolbar.Items.AddRange([students, products]);
            return toolbar;
        }

        private StatusStrip CreateStatusStrip()
        {
            var statusStrip = new StatusStrip();
            _messageStatus.Spring = true;
            _messageStatus.TextAlign = ContentAlignment.MiddleLeft;
            _timeStatus.Text = DateTime.Now.ToString("HH:mm:ss");
            statusStrip.Items.AddRange([_userStatus, new ToolStripStatusLabel("|"), _messageStatus, _timeStatus]);
            return statusStrip;
        }

        private void OpenChild<TForm>() where TForm : ManagementForm
        {
            var child = MdiChildren.OfType<TForm>().FirstOrDefault();
            if (child is null)
            {
                child = (TForm)Activator.CreateInstance(typeof(TForm), (Action<string>)UpdateStatus)!;
                child.MdiParent = this;
                child.Show();
                UpdateStatus($"Đã mở {child.Text}.");
                return;
            }

            child.Activate();
            child.BringToFront();
            UpdateStatus($"Đã kích hoạt {child.Text}.");
        }

        private void UpdateStatus(string message) => _messageStatus.Text = message;

        private void LogOut()
        {
            var result = MessageBox.Show(this, "Bạn có muốn đăng xuất không?", "Đăng xuất",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            _userStatus.Text = "Người dùng: Chưa đăng nhập";
            UpdateStatus("Đã đăng xuất.");
        }
    }
}
