﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pizza.Models;
using Pizza.Services;

namespace Pizza.ViewModels
{
    public class AddEditCustomerViewModel :BindableBase
    {
        private ICustomerRepository _repository;
        public AddEditCustomerViewModel(ICustomerRepository repo)
        {
            _repository = repo;   
            SaveCommand = new RelayCommand(OnSave, CanSave); 
            CancelCommand = new RelayCommand(OnCancel);
        }
        private bool _isEditeMode;
        public bool IsEditeMode
        {
            get => _isEditeMode;
            set => SetProperty(ref _isEditeMode, value);
        }

        private Customer _editingCustomer = null;
        private ValidableCustomer _customer;
        public ValidableCustomer Customer
        {
            get => _customer;
            set => SetProperty(ref _customer, value);
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public event Action Done;

        //----------------

        private void OnCanExecuteChanges(object sender, EventArgs e)
        {
            SaveCommand.OnCanExecuteChanged();
        }

        private void CopyCustomer(Customer source, ValidableCustomer target)
        {
            target.Id = source.Id;
            if(IsEditeMode)
            {
                target.FirstName = source.FirstName;
                target.LastName = source.LastName;
                target.Email = source.Email;
                target.Phone = source.Phone;
            }
        }

        internal void SetCustomer(Customer customer)
        {
            _editingCustomer = customer;
            if (Customer!= null)
                Customer.ErrorsChanged -= OnCanExecuteChanges;
            Customer = new ValidableCustomer();
            Customer.ErrorsChanged += OnCanExecuteChanges;
            CopyCustomer(customer, Customer);
        }

        internal void OnCancel()
        {
            Done?.Invoke();
        }
        private bool CanSave() => !Customer.HasErrors;

        private void UpdateCustomer(ValidableCustomer source, Customer target)
        {
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.Email = source.Email;
            target.Phone = source.Phone;
        }

        private async void OnSave()
        {
            UpdateCustomer(Customer, _editingCustomer);
            if(IsEditeMode)
                await _repository.UpdateCustomerAsync(_editingCustomer);
            else
                await _repository.AddCustomerAsync(_editingCustomer);
            Done?.Invoke();
        }
    }
}
