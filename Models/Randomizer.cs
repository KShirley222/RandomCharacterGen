using System;

namespace CharacterGenerator.Models
{
    public class Randomizer
    {
        
        //Thank you GFG, this should help out immensely.
        public static int[] Randomize(int[] arr, int n) 
        { 
        // Creating a object 
        // for Random class 
            Random r = new Random(); 
            
            // Start from the last element and 
            // swap one by one. We don't need to 
            // run for the first element  
            // that's why i > 0 
            for (int i = n - 1; i > 0; i--)  
            { 
                
                // Pick a random index 
                // from 0 to i 
                int j = r.Next(0, i+1); 
                
                // Swap arr[i] with the 
                // element at random index 
                int temp = arr[i]; 
                arr[i] = arr[j]; 
                arr[j] = temp; 
            } 
            // Prints the random array 
        return arr; 
        }
        public static string[] RandomizeString(string[] arr, int n) 
        { 
        // Creating a object 
        // for Random class 
            Random r = new Random(); 
            
            // Start from the last element and 
            // swap one by one. We don't need to 
            // run for the first element  
            // that's why i > 0 
            for (int i = n - 1; i > 0; i--)  
            { 
                
                // Pick a random index 
                // from 0 to i 
                int j = r.Next(0, i+1); 
                
                // Swap arr[i] with the 
                // element at random index 
                string temp = arr[i]; 
                arr[i] = arr[j]; 
                arr[j] = temp; 
            } 
            
            // Prints the random array 
        return arr;
        }
    }
}