using System;
using System.Collections.Generic;

// -------------------- Product --------------------
class Product
{
    private string _name;
    private string _productId;
    private double _price;
    private int _quantity;

    public Product(string name, string productId, double price, int quantity)
    {
        _name = name;
        _productId = productId;
        _price = price;
        _quantity = quantity;
    }

    public string Name { get { return _name; } }
    public string ProductId { get { return _productId; } }
    public double Price { get { return _price; } }
    public int Quantity { get { return _quantity; } }

    public double GetTotalCost()
    {
        return _price * _quantity;
    }
}

// -------------------- Address --------------------
class Address
{
    private string _street;
    private string _city;
    private string _stateProvince;
    private string _country;

    public Address(string street, string city, string stateProvince, string country)
    {
        _street = street;
        _city = city;
        _stateProvince = stateProvince;
        _country = country;
    }

    public string Country { get { return _country; } }

    public bool IsInUSA()
    {
        return _country.Trim().ToUpper() == "USA";
    }

    public string GetFullAddress()
    {
        return $"{_street}\n{_city}, {_stateProvince}\n{_country}";
    }
}

// -------------------- Customer --------------------
class Customer
{
    private string _name;
    private Address _address;

    public Customer(string name, Address address)
    {
        _name = name;
        _address = address;
    }

    public string Name { get { return _name; } }
    public Address Address { get { return _address; } }

    public bool LivesInUSA()
    {
        return _address.IsInUSA();
    }
}

// -------------------- Order --------------------
class Order
{
    private List<Product> _products;
    private Customer _customer;

    public Order(Customer customer)
    {
        _customer = customer;
        _products = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }

    public double GetTotalCost()
    {
        double total = 0;
        foreach (Product p in _products)
        {
            total += p.GetTotalCost();
        }

        // Add shipping
        if (_customer.LivesInUSA())
        {
            total += 5;
        }
        else
        {
            total += 35;
        }

        return total;
    }

    public string GetPackingLabel()
    {
        string label = "Packing Label:\n";
        foreach (Product p in _products)
        {
            label += $"{p.Name} (ID: {p.ProductId})\n";
        }
        return label;
    }

    public string GetShippingLabel()
    {
        return $"Shipping Label:\n{_customer.Name}\n{_customer.Address.GetFullAddress()}";
    }
}

// -------------------- Main Program --------------------
class Program
{
    static void Main(string[] args)
    {
        // First customer (USA)
        Address address1 = new Address("123 Maple Street", "Springfield", "IL", "USA");
        Customer customer1 = new Customer("John Doe", address1);
        Order order1 = new Order(customer1);

        order1.AddProduct(new Product("Laptop", "P001", 1200.00, 1));
        order1.AddProduct(new Product("Mouse", "P002", 25.00, 2));

        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine(order1.GetShippingLabel());
        Console.WriteLine($"Total Price: ${order1.GetTotalCost()}\n");

        // Second customer (International)
        Address address2 = new Address("456 King Road", "Toronto", "ON", "Canada");
        Customer customer2 = new Customer("Jane Smith", address2);
        Order order2 = new Order(customer2);

        order2.AddProduct(new Product("Headphones", "P003", 75.00, 1));
        order2.AddProduct(new Product("Keyboard", "P004", 100.00, 1));
        order2.AddProduct(new Product("Monitor", "P005", 250.00, 2));

        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine(order2.GetShippingLabel());
        Console.WriteLine($"Total Price: ${order2.GetTotalCost()}");
    }
}

