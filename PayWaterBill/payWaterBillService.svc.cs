using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Web;

namespace PayWaterBill
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
	public class Service1 : IService1
	{
		static string location = Path.Combine(HttpRuntime.AppDomainAppPath, @"App_Data");
		static string USER_INFO_FILENAME = Path.Combine(location, "UserData.txt");

		public string GetData(int value)
		{
			return string.Format("You entered: {0}", value);
		}

		// Make a payment for a user.
		public bool makePayment(string name, double amount)
		{
			AllUsers users = loadUserInfo(USER_INFO_FILENAME);
			bool success = users.makePayment(name, amount);
			users.saveToFile(USER_INFO_FILENAME);
			return success;
		}

		// Return a list of all previous payments made for a given user.
		public double[] getPreviousPayments(string name)
		{
			AllUsers users = loadUserInfo(USER_INFO_FILENAME);
			double[] payments = users.getUsersPayments(name).ToArray();
			if(payments != null)
			{
				return payments;
			}
			throw new Exception(string.Format("User: {0} not found.", name));
		}

		// Add a new user to the list of all users.
		public bool addNewUser(string name)
		{
			AllUsers users = loadUserInfo(USER_INFO_FILENAME);
			bool success = users.addUser(name);
			users.saveToFile(USER_INFO_FILENAME);
			return success;
		}

		// Return true if a user already is registered.
		public bool userExists(string name)
		{
			AllUsers users = loadUserInfo(USER_INFO_FILENAME);
			if(name == "")
			{
				return false;
			}
			return users.userExists(name);
		}

		// Load user info from the textfile on the server. Parses the text file at USER_INFO_FILENAME
		private AllUsers loadUserInfo(string filename)
		{
			AllUsers users = new AllUsers();
			StreamReader reader = new StreamReader(filename);
			while(!reader.EndOfStream)
			{
				string line = reader.ReadLine();	// reads [USER]
				line = reader.ReadLine();			// reads the user's name
				User currentUser = new User(line);	// Create User object for the current user being parsed.
				// If [ is found immediately follow a user's name, then the user hasn't made any payments.
				if (reader.Peek() == '[')
				{
					users.addUser(currentUser);
					continue;	
				}
				else
				{
					// Get all the payments made by a user and add them to User object.
					while(reader.Peek() != '[' && !reader.EndOfStream)
					{
						line = reader.ReadLine();
						currentUser.addPayment(Convert.ToDouble(line));
					}
				}
				users.addUser(currentUser);	// add User object to the list of all users.
			}
			reader.Close();
			return users;
		}

		public CompositeType GetDataUsingDataContract(CompositeType composite)
		{
			if (composite == null)
			{
				throw new ArgumentNullException("composite");
			}
			if (composite.BoolValue)
			{
				composite.StringValue += "Suffix";
			}
			return composite;
		}
	}
}
