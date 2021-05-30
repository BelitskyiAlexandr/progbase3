using System;
using System.Security.Cryptography;
using System.Text;

public static class Hashing
{

    public static User SignUp(string username, string fullname, string password, UserRepository userRepository)
    {
        User user = new User();
        if (!userRepository.UserExists(username))
        {
            password = HashCode(password);
            user = new User(fullname, username, password);
            userRepository.Insert(user);
        }
        else
            Console.WriteLine("Exists");
        return user;
    }

    public static User SignIn(string username, string password, UserRepository userRepository)
    {
        User user = new User();
        if (userRepository.UserExists(username))
        {
            SHA256 sha256Hash = SHA256.Create();
            string hash = GetHash(sha256Hash, password);
            if (VerifyHash(sha256Hash, password, hash))
            {
                user = userRepository.GetUserByUsername(username);
                Console.WriteLine("The hashes are the same.");
            }
            else
            {
                Console.WriteLine("The hashes are not same.");
            }
        }
        else
            Console.WriteLine("User doesnt exist");
        
        return user;
    }
    private static string HashCode(string code)
    {
        SHA256 sha256Hash = SHA256.Create();
        string hash = GetHash(sha256Hash, code);
        sha256Hash.Dispose();
        return hash;
    }
    private static string GetHash(HashAlgorithm hashAlgorithm, string input)
    {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        var sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    // Verify a hash against a string.
    private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
    {
        // Hash the input.
        var hashOfInput = GetHash(hashAlgorithm, input);

        // Create a StringComparer an compare the hashes.
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        return comparer.Compare(hashOfInput, hash) == 0;
    }
}

