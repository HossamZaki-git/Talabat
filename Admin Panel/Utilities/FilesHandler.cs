using Stripe.Radar;

namespace Admin_Panel.Utilities
{
    public static class FilesHandler
    {
        /*
           * Checks if there is a file in the Talabat.WebAPI/wwwroot/images/products has a file with the name pattern _productId_* and deletes it
           * Creates a file with the name _productId_productName
            
             That naming convention guarantees:
                - Understandable name expressing the product id & name
                - Distincit naming, by relaying on the id
                - Ease of fetching a specific product image
             
         */
        public static async Task<string> SaveProductImageAsync(int productID, string ProductName, IFormFile img)
        {
            // Get the current directory of the MVC project (usually the bin folder)
            var currentDir = Directory.GetCurrentDirectory();

            // Navigate up to the solution root
            var solutionRoot = Directory.GetParent(currentDir).FullName;

            // Build the path to the Web API's wwwroot folder
            var directoryPath = Path.Combine(solutionRoot, "Talabat.WebAPI", "wwwroot", "images","products");
            
            FileInfo OldProductImage = new DirectoryInfo(directoryPath)
                                        .GetFiles($"_{productID}_*")
                                        .FirstOrDefault();

            // If there is an already existing image for that product delete it
            if (OldProductImage is not null)
                OldProductImage.Delete();

            string FileName = $"_{productID}_{ProductName}{Path.GetExtension(img.FileName)}";

            using (var stream = new FileStream(Path.Combine(directoryPath, FileName), FileMode.Create))
                await img.CopyToAsync(stream);

            string databaseImagePath = Path.Combine("images", "products", FileName);
            databaseImagePath = databaseImagePath.Replace(@"\", @"/");
            return databaseImagePath;

        }
    }
}
