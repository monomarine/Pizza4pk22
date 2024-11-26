﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pizza.Services;
using Pizza.Models;
using System.Collections.ObjectModel;

namespace Pizza.ViewModels
{
    class CustomerListViewModel : BindableBase
    {
         private ICustomerRepository _repository;
         public CustomerListViewModel(ICustomerRepository repository)
         {

             _repository=repository;
            Customers = new ObservableCollection<Customer>();  
            LoadCustomers();
        
             PlaceOrderCommand =new RelayCommand<Customer>(OnPlaceOrder);
             AddCustomerCommand = new RelayCommand(OnAddCustomer);
             EditCustomerCommand = new RelayCommand<Customer>(OnEditCustomer);
             ClearSearchInput = new RelayCommand(OnClearSearch);
         }
    
         private ObservableCollection<Customer>? _customers;
         public ObservableCollection<Customer>? Customers
         {
             get => _customers;
             set => SetProperty(ref _customers, value);
         }
        
         private List<Customer>? _customersList;
         public async void LoadCustomers()
         {
             _customersList = await _repository.GetCustomersAsync();
             Customers = new ObservableCollection<Customer>(_customersList);
         }
        
         private string? _searchInput;
         public string? SearchInput
         {
             get => _searchInput;
             set
             {
                 SetProperty(ref _searchInput, value);
                 FilterCustomersBuName(_searchInput);
             }
         }
        
         private void FilterCustomersBuName(string findText)
         {
             if (string.IsNullOrEmpty(findText))
             {
                 Customers = new ObservableCollection<Customer>(_customersList);
                 return;
             }
             else
             {
                 Customers = new ObservableCollection<Customer>(
                     _customersList.Where(c => c.FullName.ToLower()
                     .Contains(findText.ToLower())));
             }
         }
        
         public RelayCommand<Customer> PlaceOrderCommand { get; private set; }
         public RelayCommand AddCustomerCommand { get; private set; }
         public RelayCommand<Customer> EditCustomerCommand { get; private set; }
         public RelayCommand ClearSearchInput {  get; private set; }
        
         public event Action<Customer> PlaceOrderRequested = delegate { };
         public event Action AddCustomerRequested = delegate { };
         public event Action<Customer> EditCustomerRequested = delegate { }; 
        
         private void OnPlaceOrder(Customer customer)
         {
             PlaceOrderRequested(customer);
         }
        
         private void OnAddCustomer()
         {
            AddCustomerRequested?.Invoke();
        }
        
         private void OnEditCustomer(Customer customer)
         {
             EditCustomerRequested(customer);
         }
        
         private void OnClearSearch()
         {
             SearchInput = null;
         }
    }
}
