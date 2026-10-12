using System.Data;
using System.Globalization;

namespace bt_4._7;

public abstract class ManagementForm : Form
{
    private readonly DataGridView _grid = new();
    private readonly Func<DataTable> _createData;
    private readonly Action<string> _notifyParent;

    protected ManagementForm(string title, Func<DataTable> createData, Action<string> notifyParent)
    {
        Text = title;
        Width = 850;
        Height = 500;
        StartPosition = FormStartPosition.CenterParent;
        _createData = createData;
        _notifyParent = notifyParent;

        var toolbar = new ToolStrip();
        var addButton = new ToolStripButton("Thêm mới");
        addButton.Click += (_, _) => AddRow();
        toolbar.Items.Add(addButton);

        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.DataSource = _createData();
        _grid.ContextMenuStrip = CreateContextMenu();

        Controls.Add(_grid);
        Controls.Add(toolbar);
    }

    protected abstract object[] CreateNewRow(int nextId);

    private ContextMenuStrip CreateContextMenu()
    {
        var menu = new ContextMenuStrip();
        menu.Items.Add("Chỉnh sửa dòng", null, (_, _) => EditSelectedRow());
        menu.Items.Add("Xóa dòng", null, (_, _) => DeleteSelectedRow());
        menu.Items.Add("Làm mới dữ liệu", null, (_, _) => RefreshData());
        menu.Items.Add("Xem chi tiết", null, (_, _) => ShowDetails());
        menu.Opening += (_, e) => e.Cancel = _grid.CurrentRow is null;
        return menu;
    }

    private void AddRow()
    {
        if (_grid.DataSource is not DataTable table)
            return;

        table.Rows.Add(CreateNewRow(table.Rows.Count + 1));
        _notifyParent($"Đã thêm dữ liệu trong {Text}.");
    }

    private void DeleteSelectedRow()
    {
        if (_grid.CurrentRow?.DataBoundItem is not DataRowView rowView)
            return;

        rowView.Row.Delete();
        rowView.Row.Table.AcceptChanges();
        _notifyParent($"Đã xóa dòng trong {Text}.");
    }

    private void EditSelectedRow()
    {
        if (_grid.CurrentRow?.DataBoundItem is not DataRowView rowView)
            return;

        var columns = rowView.Row.Table.Columns.Cast<DataColumn>().ToArray();
        var fields = new List<TextBox>(columns.Length);
        using var dialog = new Form
        {
            Text = "Chỉnh sửa dòng",
            FormBorderStyle = FormBorderStyle.FixedDialog,
            StartPosition = FormStartPosition.CenterParent,
            MinimizeBox = false,
            MaximizeBox = false,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = columns.Length + 1,
            Padding = new Padding(12)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        foreach (var column in columns)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(new Label
            {
                Text = column.ColumnName,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(3, 7, 12, 3)
            }, 0, fields.Count);
            var field = new TextBox
            {
                Width = 280,
                Text = Convert.ToString(rowView[column.ColumnName], CultureInfo.CurrentCulture) ?? string.Empty,
                Margin = new Padding(3)
            };
            fields.Add(field);
            layout.Controls.Add(field, 1, fields.Count - 1);
        }

        var buttons = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            Dock = DockStyle.Fill
        };
        var saveButton = new Button { Text = "Lưu", AutoSize = true };
        var cancelButton = new Button { Text = "Hủy", AutoSize = true, DialogResult = DialogResult.Cancel };
        saveButton.Click += (_, _) =>
        {
            try
            {
                var values = columns.Select((column, index) => column.DataType == typeof(string)
                    ? fields[index].Text
                    : Convert.ChangeType(fields[index].Text, column.DataType, CultureInfo.CurrentCulture)
                        ?? DBNull.Value).ToArray();

                for (var index = 0; index < columns.Length; index++)
                    rowView[columns[index].ColumnName] = values[index];

                dialog.DialogResult = DialogResult.OK;
                dialog.Close();
            }
            catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
            {
                MessageBox.Show(dialog, "Giá trị nhập không hợp lệ. Vui lòng kiểm tra lại.",
                    "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        };
        buttons.Controls.Add(cancelButton);
        buttons.Controls.Add(saveButton);
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(buttons, 0, columns.Length);
        layout.SetColumnSpan(buttons, 2);
        dialog.Controls.Add(layout);
        dialog.CancelButton = cancelButton;

        if (dialog.ShowDialog(this) == DialogResult.OK)
            _notifyParent($"Đã cập nhật dòng trong {Text}.");
    }

    private void RefreshData()
    {
        _grid.DataSource = _createData();
        _notifyParent($"Đã làm mới dữ liệu trong {Text}.");
    }

    private void ShowDetails()
    {
        if (_grid.CurrentRow?.DataBoundItem is not DataRowView rowView)
            return;

        var details = string.Join(Environment.NewLine,
            rowView.Row.Table.Columns.Cast<DataColumn>()
                .Select(column => $"{column.ColumnName}: {rowView[column.ColumnName]}"));
        MessageBox.Show(this, details, "Chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}

public sealed class StudentForm : ManagementForm
{
    public StudentForm(Action<string> notifyParent)
        : base("Quản lý Sinh viên", CreateStudents, notifyParent)
    {
    }

    protected override object[] CreateNewRow(int nextId) =>
        [nextId, $"SV{nextId:000}", "Sinh viên mới", "Chưa cập nhật"];

    private static DataTable CreateStudents()
    {
        var table = new DataTable();
        table.Columns.Add("ID", typeof(int));
        table.Columns.Add("Mã sinh viên", typeof(string));
        table.Columns.Add("Họ tên", typeof(string));
        table.Columns.Add("Lớp", typeof(string));
        table.Rows.Add(1, "SV001", "Nguyễn Văn An", "CNTT1");
        table.Rows.Add(2, "SV002", "Trần Thị Bình", "CNTT2");
        return table;
    }
}

public sealed class ProductForm : ManagementForm
{
    public ProductForm(Action<string> notifyParent)
        : base("Quản lý Sản phẩm", CreateProducts, notifyParent)
    {
    }

    protected override object[] CreateNewRow(int nextId) =>
        [nextId, $"SP{nextId:000}", "Sản phẩm mới", 0m];

    private static DataTable CreateProducts()
    {
        var table = new DataTable();
        table.Columns.Add("ID", typeof(int));
        table.Columns.Add("Mã sản phẩm", typeof(string));
        table.Columns.Add("Tên sản phẩm", typeof(string));
        table.Columns.Add("Đơn giá", typeof(decimal));
        table.Rows.Add(1, "SP001", "Bàn phím", 450000m);
        table.Rows.Add(2, "SP002", "Chuột không dây", 250000m);
        return table;
    }
}
