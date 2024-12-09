using Model;
using System.ComponentModel;
using System.Xml.Serialization;


namespace View
{
    /// <summary>
    /// ������� ����� ������� �������� 
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// ���� ��� ���������� �������
        /// ������������ ������� ��������
        /// </summary>
        private BindingList<ExerciseBase> _calloriesList =
            new BindingList<ExerciseBase>();

        /// <summary>
		/// ��������������� ���� ��� ���������� �������.
		/// </summary>
		private BindingList<ExerciseBase> _filteredExerciseList;

        /// <summary>
		///  ���� ��� �������� ��������� ����� DataForm.
		/// </summary>
		private bool _isDataFormOpen = false;

        /// <summary>
        ///  ���� ��� �������� ��������� ����� FindForm.
        /// </summary>
        private bool _isFindFormOpen = false;

        /// <summary>
        /// ���� ��� �������� ��������� �������.
        /// </summary>
        private bool _isFiltered = false;

        /// <summary>
		/// ���� ��� ���������� � �������� �����.
		/// </summary>
		private readonly XmlSerializer _serializerXml =
            new XmlSerializer(typeof(BindingList<ExerciseBase>));

        /// <summary>
        /// �������, ������������� ��� ���������� ������
        /// </summary>
        public event EventHandler DataUpdated;

        /// <summary>
        /// ����������� ������� �����
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            FillingDataGridView(_calloriesList);

            _buttonAddCallories.Click += AddCalloriesButtonClick;

            _buttonDelete.Click += ClickDeleteElementButton;

            _buttonFillterCallories.Click += FillterButtonClick;

            _buttonFillterCallories.Click += ShowIfFiltered;

            _buttonOpenCallories.Click += OpenFile;

            _buttonSaveCallories.Click += SaveFile;

#if DEBUG
            _buttonAddRandom.Click += ClickRandomButton;

#endif
        }

        /// <summary>
        /// ����� ���������� ������� ����������� ������� 
        /// </summary>
        /// <param name="calloriesList">������ �������� ExerciseBase,
        /// ���������� ������ ��� ����������� � �������</param>
        private void FillingDataGridView(BindingList<ExerciseBase> calloriesList)
        {
            _dataControlCallories.DataSource = null; 
            _dataControlCallories.DataSource = calloriesList;

            _dataControlCallories.AllowUserToResizeColumns = false;
            _dataControlCallories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _dataControlCallories.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _dataControlCallories.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _dataControlCallories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        /// <summary>
        /// ���������� ������ ����������
        /// </summary>
        /// <param name="sender">�������</param>
        /// <param name="exerciseBase">������ � �������</param>
        private void CancelCallories(object sender, EventArgs exerciseBase)
        {
            CalloriesAddedEventArgs addedEventArgs =
               exerciseBase as CalloriesAddedEventArgs;

            _calloriesList.Remove(addedEventArgs?.ExerciseBase);
        }

        /// <summary>
        /// ������ ������� �� ������ "�������� ��������� ������".
        /// </summary>
        /// <param name="sender">�������</param>
        /// <param name="e">������ � �������</param>
        private void ClickRandomButton(object sender, EventArgs e)
        {
            _calloriesList.Add(RandomCallories.GetRandomExercise());
        }

        /// <summary>
        /// ����� ������� �� ������ "��������"
        /// </summary>
        /// <param name="sender">�������</param>
        /// <param name="e">������ � �������</param>
        private void AddCalloriesButtonClick(object sender, EventArgs e)
        {
            if (!_isDataFormOpen)
            {
                _isDataFormOpen = true;
                DeactivateElements();

                DataForm DataForm = new DataForm();
                DataForm.FormClosed += (s, args) =>
                {
                    _isDataFormOpen = false;
                    DeactivateElements();
                };
                DataForm.CalloriesAdded += AddedCallories;
                DataForm.CalloriesCancel += CancelCallories;
                DataForm.Show();
            }
        }

        /// <summary>
        /// ���������� ���������� ������ � ������ calloriesList
        /// </summary>
        /// <param name="sender">������, ������� ������ �������</param>
        /// <param name="exerciseBase">��������� �������, ������� �������� 
        /// ������, ������������ ��� ������ �������</param>
        private void AddedCallories(object sender, EventArgs exerciseBase)
        {
            CalloriesAddedEventArgs addedEventArgs = exerciseBase
                as CalloriesAddedEventArgs;
            _calloriesList.Add(addedEventArgs?.ExerciseBase);
        }

        /// <summary>
        /// ����� ����������� ��������� 
        /// </summary>
        private void DeactivateElements()
        {
            _buttonAddCallories.Enabled = !_isFiltered && !_isDataFormOpen;
            _buttonFillterCallories.Enabled = !_isFindFormOpen && !_isDataFormOpen;
            _buttonSaveCallories.Enabled = !_isFiltered;
            _buttonOpenCallories.Enabled = !_isFiltered;
#if DEBUG
            _buttonAddRandom.Enabled = !_isFiltered;
#endif
        }


        /// <summary>
        /// ����� ��� ������ "�������"
        /// </summary>
        /// <param name="sender">�������</param>
        /// <param name="e">������ � �������</param>
        private void ClickDeleteElementButton(object sender, EventArgs e)
        {
            var currentList = _isFiltered ? _filteredExerciseList : _calloriesList;
            var itemsToRemove = new List<ExerciseBase>();
            foreach (DataGridViewRow row in _dataControlCallories.SelectedRows)
            {
                if (row.DataBoundItem is ExerciseBase element)
                {
                    itemsToRemove.Add(element);
                }
            }

            foreach (var item in itemsToRemove)
            {
                currentList.Remove(item);
                if (_isFiltered)
                {
                    _calloriesList.Remove(item);
                }
            }
            DataUpdated?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
		/// ����� ������� �� ������ "��������� ������"
		/// </summary>
		/// <param name="sender">�������</param>
		/// <param name="e">������ � �������</param>
		private void FillterButtonClick(object sender, EventArgs e)
        {
            if (_isFindFormOpen) return;

            _isFindFormOpen = true;
            FilterForm filterForm = new FilterForm(_calloriesList);

            filterForm.ApplyFilter += (filteredItems) =>
            {
                if (filteredItems == _calloriesList) 
                {
                    _isFiltered = false;
                    FillingDataGridView(_calloriesList);
                }
                else
                {
                    _filteredExerciseList = filteredItems;
                    _isFiltered = true;
                    FillingDataGridView(_filteredExerciseList);
                }

                DeactivateElements(); 
            };

            filterForm.FormClosed += (s, e) =>
            {
                _isFindFormOpen = false;
                DeactivateElements(); 
            };

            filterForm.Show();
        }


        /// <summary>
        /// �����, �������� ��������� ������ ��� ����������
        /// </summary>
        /// <param name="sender">�������</param>
        /// <param name="e">������ � �������</param>
        private void ShowIfFiltered(object sender, EventArgs e)
        {
            _buttonFillterCallories.Enabled = !_isFiltered;
        }

        /// <summary>
		/// ���������� ���������� ������
		/// </summary>
		/// <param name="sender">�������</param>
		/// <param name="transportList">������ � �������</param>
		private void FilteredExersice(object sender, EventArgs exerciseList)
        {
            CalloriesFilterEventArgs filterEventArgs =
                 exerciseList as CalloriesFilterEventArgs;

            _filteredExerciseList = filterEventArgs?.FilteredCalloriesList;
            _isFiltered = true;
            DeactivateElements();
            FillingDataGridView(_filteredExerciseList);
        }

        /// <summary>
        /// ����� ��������� ������� �� ������ "�������� ������" �� ������ �����
        /// </summary>
        /// <param name="sender">�������</param>
        /// <param name="e">������ � �������</param>
        private void ResetFilter(object sender, EventArgs e)
        {
            _isFiltered = false;
            FillingDataGridView(_calloriesList);
            DeactivateElements();
        }

        /// <summary>
		/// ����� ��� �������� ������ �� �����
		/// </summary>
		/// <param name="sender">�������</param>
		/// <param name="e">������ � �������</param>
		private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "����� (*.tran)|*.tran|��� ����� (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            string filePath = openFileDialog.FileName.ToString();

            try
            {
                using (var file = new StreamReader(filePath))
                {
                    _calloriesList =
                        (BindingList<ExerciseBase>)_serializerXml.Deserialize(file);
                }

                _dataControlCallories.DataSource = _calloriesList;
                MessageBox.Show("���� ������� ��������.",
                    "�������� ���������",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("�� ������� ��������� ����.\n" +
                   "���� �������� ��� �� ������������� �������.",
                   "������",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
		/// ����� ��� ���������� ������ � ����
		/// </summary>
		/// <param name="sender">�������</param>
		/// <param name="e">������ � �������</param>
		private void SaveFile(object sender, EventArgs e)
        {
            if (!_calloriesList.Any() || _calloriesList is null)
            {
                MessageBox.Show("����������� ������ ��� ����������.",
                    "������ �� ���������",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "����� (*tran.)|*.tran|��� ����� (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName.ToString();

                using (var file = File.Create(filePath))
                {
                    _serializerXml.Serialize(file, _calloriesList);
                }

                MessageBox.Show("���� ������� ��������.",
                    "���������� ���������",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}