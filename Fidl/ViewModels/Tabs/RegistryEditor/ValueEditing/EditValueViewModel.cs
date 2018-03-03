﻿namespace Fidl.ViewModels.Tabs.RegistryEditor.ValueEditing
{
    using Fidl.Factories.Interfaces;
    using Fidl.Models.RegistryEditor;
    using Fidl.ViewModels.Tabs.RegistryEditor.ValueEditing.Interfaces;

    using Microsoft.Win32;

    internal class EditValueViewModel : ViewModelBase, IEditValueViewModel
    {
        private readonly IRegistryFactory _registryFactory;

        public EditValueViewModel(IRegistryFactory registryFactory)
        {
            _registryFactory = registryFactory;
        }

        public IValueEditViewModel ValueEditViewModel { get; private set; }

        public void Initialise(Value value)
        {
            switch (value.Kind)
            {
                case RegistryValueKind.String:
                case RegistryValueKind.ExpandString:
                    ValueEditViewModel = _registryFactory.MakeValueEditViewModel<IStringEditViewModel>(value);
                    break;

                case RegistryValueKind.Binary:
                case RegistryValueKind.Unknown:
                case RegistryValueKind.None:
                    break;

                case RegistryValueKind.DWord:
                case RegistryValueKind.QWord:
                    break;

                case RegistryValueKind.MultiString:
                    break;
            }

            DisplayName = $"Edit {value.Kind}";
        }

        public void Ok()
        {
            ValueEditViewModel.Value.FlushStoredValue();
            TryClose();
        }

        public void Cancel()
        {
            TryClose();
        }

        protected override void OnDeactivate(bool close)
        {
            ValueEditViewModel.Value.ResetStoredValue();
        }
    }
}