using System; 
using System.IO; 
using System.Net.Http; 
using System.Net.Http.Headers; 
using System.Threading.Tasks;


// Ray : I tested this code with dotnet core v 2.1.0 runtime, therefore, this sample should works on most platforms.
namespace uploadfile {

    
    class Program {
        private static readonly string ApiUrl = $"http://the-host-name-and-port-of-api/api/v1/file/file_upload";
        
        //Ray : Please note, async Task on Main supportted by C# 7.1 only.Please check your C# version.
        static async Task Main(string[] args){

            //Ray : please login to get ApiKey CustomerCode
            string ApiKey = "apikey returned by login api";
            string CustomerCode="T023";
            string FilePath = "sample.png";
            
            //Ray : please give parking_id returned by API::add_transaction
            string ParkingId = "3";
            var result =await PostFile(ApiKey, CustomerCode, FilePath, ParkingId);
        }
        
        private static async Task < bool > PostFile(String ApiKey, String CustomerCode, String FilePath, String ParkingId, String UploadType="hsinchu_lpr") {
            
            using (HttpClientHandler handler = new HttpClientHandler()) {
                using (HttpClient client = new HttpClient(handler)) {
                try {

                    // Ray : you may provide constant via params or constant.
                    // Ray : duplicate filename is not acceptable.
                    HttpResponseMessage response = null; 

                    var ApiFullUrl = $"{ApiUrl}"; 

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data")); 

                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", ApiKey);

                    var fileContent = new ByteArrayContent(File.ReadAllBytes(FilePath)); 
                
                    using (var content = new MultipartFormDataContent()) {
                        //Ray : please add content by following this order, 
                        //file -> upload_type -> customer_code -> parking_id
                        content.Add(fileContent, "file","ray-testfilename.png"); 
                        content.Add(new StringContent(UploadType), "upload_type"); 
                        content.Add(new StringContent(CustomerCode),"customer_code");
                        content.Add(new StringContent(ParkingId), "parking_id");

                        response = await client.PostAsync(ApiUrl, content); 
                    }

                    // Ray : You can do whatever you would like to check here, I just simply output response status
                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode == true)
                        {
                            String strResult = await response.Content.ReadAsStringAsync();
                            Console.WriteLine(strResult);
                        }else{
                            Console.WriteLine(response.StatusCode);
                        }
                    }
                }
                catch (Exception ex) {
                    //Ray : please be aware how to handle exception
                    throw ex;
                }
        }
        //Ray : return everything you want here
        return true;
    }
        }
    }
}


