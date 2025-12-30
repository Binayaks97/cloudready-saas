using System.Security.Cryptography;
using System.Text;

var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
Console.WriteLine(key);