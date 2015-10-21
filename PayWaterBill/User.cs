using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace PayWaterBill
{
	public class AllUsers
	{
		private List<User> users;

		public AllUsers()
		{
			users = new List<User>();	
		}

		public void saveToFile(string filename)
		{
			StreamWriter writer = new StreamWriter(filename);
			for(int currentUser = 0; currentUser != users.Count; currentUser++)
			{
				writer.WriteLine("[USER]");
				writer.WriteLine(users[currentUser].getName());
				List<double> payments = users[currentUser].getPayments();
				for(int currentPayment = 0; currentPayment != payments.Count; currentPayment++)
				{
					writer.WriteLine(payments[currentPayment].ToString());
				}
			}
			writer.Close();
		}

		// Add a payment amount for a user.
		public bool makePayment(string name, double amount)
		{
			for (int i = 0; i != users.Count; i++)
			{ 
				if(users[i].getName() == name)
				{
					users[i].addPayment(amount);
					return true;
				}
			}
			return false;
		}

		// Return a List of payment amounts for a user.
		public List<double> getUsersPayments(string name)
		{
			for(int i = 0; i != users.Count; i++)
			{
				if(users[i].getName() == name)
				{
					return users[i].getPayments();
				}
			}
			// If the user wasn't found, return null
			return null;
		}

		// Add a new user to the list of all users given a name.
		public bool addUser(string name)
		{
			if(!userExists(name))
			{
				users.Add(new User(name));
				return true;
			}
			return false;
		}

		// Add a new user to the list of all users given a User object.
		public bool addUser(User u)
		{
			if(!userExists(u.getName()))
			{
				users.Add(u);
				return true;
			}
			return false;
		}

		// Check if a user exists in the list of all users.
		public bool userExists(string name)
		{
			for(int i = 0; i != users.Count; i++)
			{
				if(users[i].getName() == name)
				{
					return true;
				}
			}
			return false;
		}
	}

	public class User
	{
		private string name;
		private List<double> payments;

		public User(string name)
		{
			this.name = name;
			payments = new List<double>();
		}

		public void addPayment(double amount)
		{
			payments.Add(amount);
		}

		public List<double> getPayments()
		{
			return payments;
		}

		public string getName()
		{
			return name;
		}
	}
}