using Google.Cloud.Firestore;
using Server.env;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAO
{
    public class UserInter
    {
        private static UserInter instance;
        public static UserInter Instance
        {
            get { if (instance == null) instance = new UserInter(); return instance; }
            private set { instance = value; }
        }
        private UserInter() { }
        public async Task RegisterUserAsync(string username, string displayName, string email, string password, string usertype)
        {

            var db = FireStoreHelper.db;
            // Reference to the "users" collection
            CollectionReference usersRef = db.Collection("users");

            // Get all documents in the "users" collection synchronously
            QuerySnapshot usersSnapshot = await usersRef.GetSnapshotAsync();

            // Count the number of documents
            int documentCount = usersSnapshot.Documents.Count;

            // Create a new document with a unique ID based on the count
            DocumentReference docRef = db.Collection("users").Document(documentCount.ToString());
            Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "username", username },
            { "displayName", displayName },
            { "email", email },
            { "password", password }, // Note: Password should be hashed before storing in Firestore
            { "usertype", usertype}
        };
            // Set the data to Firestore
            await docRef.SetAsync(data);
            Console.WriteLine("User '" + username + "' registered successfully!");

        }
        public async Task<bool> field_exist(string collection, string field, string value)
        {
            var db = FireStoreHelper.db;
            CollectionReference usersRef = db.Collection(collection);
            QuerySnapshot querySnapshot = await usersRef.
                WhereEqualTo(field, value)
                .GetSnapshotAsync();
            return querySnapshot.Count > 0;
        }

        public async Task<int> login(string username, string password)
        {
            var db = FireStoreHelper.db;
            CollectionReference usersRef = db.Collection("users");
            QuerySnapshot snapshot = await usersRef.WhereEqualTo("username", username).GetSnapshotAsync();
            if (snapshot.Count == 0)
            {
                Console.WriteLine("User '" + username + "' Don't exist");
                return 0;
            }
            foreach (DocumentSnapshot userDoc in snapshot.Documents)
            {
                // Get user data
                Dictionary<string, object> userData = userDoc.ToDictionary();

                // Check if the password matches
                if (userData.ContainsKey("password") && (string)userData["password"] == password)
                {
                    // Password matches, user is authenticated
                    Console.WriteLine("User '" + username + "' login successfully!");
                    return 1;
                }
            }
            Console.WriteLine("User '" + username + "' Password didn't match");
            return -1;
        }

        public async Task<bool> ResetPass(string email, string passnew)
        {
            var db = FireStoreHelper.db;
            CollectionReference usersRef = db.Collection("users");
            QuerySnapshot snapshot = await usersRef.WhereEqualTo("email", email).GetSnapshotAsync();
            try
            {
                DocumentSnapshot userSnapshot = snapshot.Documents.First();
                DocumentReference userRef = userSnapshot.Reference;
                await userRef.UpdateAsync("password", passnew);
                Console.WriteLine("Password reset successfully for user with email: " + email);
                return true;
            }
            catch
            {
                Console.WriteLine("Falied to reset password for user with email: " + email);
                return false;
            }
        }
        public async Task<Dictionary<string, object>> LoadInfo(string username)
        {
            var db = FireStoreHelper.db;
            CollectionReference usersRef = db.Collection("users");

            // Query for the user by username
            QuerySnapshot snapshot = await usersRef.WhereEqualTo("username", username).GetSnapshotAsync();

            if (snapshot.Count == 0)
            {
                // User not found
                Console.WriteLine("User '" + username + "' not found.");
                return null;
            }

            // Assuming username is unique, there should be only one document in the snapshot
            foreach (DocumentSnapshot userDoc in snapshot.Documents)
            {
                // Convert user document to a dictionary
                Dictionary<string, object> userData = userDoc.ToDictionary();

                // Optionally remove sensitive data like password before returning
                if (userData.ContainsKey("password"))
                {
                    userData.Remove("password");
                }

                return userData;
            }

            return null; // If something unexpected happens
        }
        public async Task<bool> ResetDisplayName(string username, string newdisplayname)
        {
            var db = FireStoreHelper.db;
            CollectionReference usersRef = db.Collection("users");
            QuerySnapshot snapshot = await usersRef.WhereEqualTo("username", username).GetSnapshotAsync();
            try
            {
                DocumentSnapshot userSnapshot = snapshot.Documents.First();
                DocumentReference userRef = userSnapshot.Reference;
                await userRef.UpdateAsync("displayName", newdisplayname);
                Console.WriteLine("Display name reset successfully for user : " + username);
                return true;
            }
            catch
            {
                Console.WriteLine("Falied to reset displayname for user: " + username);
                return false;
            }
        }
        public async Task<string> GetPasswordByUsername(string username)
        {
            var db = FireStoreHelper.db;
            CollectionReference usersRef = db.Collection("users");

            // Query for the user by username
            QuerySnapshot snapshot = await usersRef.WhereEqualTo("username", username).GetSnapshotAsync();

            if (snapshot.Count == 0)
            {
                // User not found
                Console.WriteLine("User '" + username + "' not found.");
                return null;
            }

            // Assuming username is unique, there should be only one document in the snapshot
            foreach (DocumentSnapshot userDoc in snapshot.Documents)
            {
                // Convert user document to a dictionary
                Dictionary<string, object> userData = userDoc.ToDictionary();

                // Check if the password field exists and return the password
                if (userData.ContainsKey("password"))
                {
                    string password = userData["password"] as string;
                    return password;
                }
            }

            return "khongco"; // If something unexpected happens
        }
    }
}
